using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using IGORR.Content;
using IGORR.Client.Logic.Body;

namespace IGORR.Client.Logic
{
    class PlayerPointer
    {
        Color color;
        Texture2D tex;
        Vector2 _textureOffset;
        string name;

        public PlayerPointer(string name, Texture2D tex)
        {
            this.tex = tex;
            this.name = name;
            int val = name.GetHashCode();
            this.color = new Color(val%256, (val /= 256)%256, (val /= 256)%256);
            _textureOffset.X = tex.Width;
            _textureOffset.Y = tex.Height/2;
        }

        public void Draw(SpriteBatch batch, Vector2 pos, Camera cam)
        {
            pos.X = (int)pos.X;
            pos.X += 8;
            pos.Y = (int)pos.Y;
            Vector2 targetEnd = pos;
            targetEnd.Y -= 8;
            Rectangle viewSpace=cam.ViewSpace;
            Vector2 diff = targetEnd - cam.Position;
            
            if(Math.Abs(diff.X)>viewSpace.Width/2 || Math.Abs(diff.Y)>viewSpace.Height/2)
            {
                if (Math.Abs(diff.X) / viewSpace.Width > Math.Abs(diff.Y) / viewSpace.Height)
                {
                    diff *= (viewSpace.Width / 2) / Math.Abs(diff.X);
                }
                else
                {
                    diff *= (viewSpace.Height / 2) / Math.Abs(diff.Y);
                }
                targetEnd = cam.Position + diff;
            }
            targetEnd.X = (int)targetEnd.X;
            targetEnd.Y = (int)targetEnd.Y;
            
            float angle = (float)Math.Atan2(pos.Y - targetEnd.Y, pos.X - targetEnd.X);
            batch.Draw(tex, targetEnd, null, color, angle, _textureOffset, 0.5f, SpriteEffects.None, 0.1f);
        }

        public void SetName(string name)
        {
            this.name = name;
            //int val = name.GetHashCode();
            //this.color = new Color(val % 256, (val /= 256) % 256, (val /= 256) % 256);
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }
    }

    public class Player : GameObject
    {
        public static SpriteFont font;
        static Texture2D _lifeBar;
        Vector2 _speed;
        Vector2 _moveVector;
        public const float gravity = 60*2.5f;
        const float baseSpeed = 60f;
        bool _onGround;
        int _maxhp = 50;
        int _hp = 50;
        float _invincibleTime = 3;
        int groupID = 1;

        int changed = 0;

        int _exp=0;
        int _targetExp=0;
        int nextLevelExp=70;
        int lastExp=0;
        int level=1;

        bool stunned = false;
        float stunTimeout;
        bool airstun = false;

        List<AnimationContainer> _attachedAnimations;

        //List<BodyPart> _bodyParts;
        //BodyPart _completeBody;

        Body.Body _body;

        AnimationController _aniControl;
        Vector4 _collisionOffset;
        bool jump = false;
        bool flying = false;
        bool Left = false;
        bool wallCollision;
        string _name = "";
        Vector2 _nameSize = Vector2.Zero;
        Vector2 _lastSpeed;
        bool _moveVectorSet = false;

        PlayerPointer _pointer;

        Inventory _inventory;

        IMap _map;

        public Player(Texture2D texture, Rectangle spawnPos, int id)
            : base(texture, spawnPos, id)
        {
            if (_lifeBar == null)
                _lifeBar = ContentInterface.LoadTexture("White");
            _pointer = new PlayerPointer(this._name, ContentInterface.LoadTexture("Arrow"));
            _speed = Vector2.Zero;
            _onGround = false;
            //_bodyParts = new List<BodyPart>();
            _inventory = new Logic.Inventory(this);
            flying = true;
            _collisionOffset = new Vector4((((16 - spawnPos.Width) % 16 + 16) % 16) + 0.6f, 0, (((16 - spawnPos.Height) % 16 + 16) % 16) + 0.99975f, 0.0015f);
            //_completeBody = new BaseBody(null);
            int dim = _texture.Bounds.Height;
            _aniControl = new AnimationController();
            _aniControl.Run = new Animation(100, dim,dim, new int[] { 0, 1, 2, 3, 4, 5, 6});
            _aniControl.Idle = _aniControl.Run;
            _aniControl.Jump = new Animation(100, dim,dim, new int[] { 7, 8, 9, 10, 11, 12 });
            _aniControl.Fly = new Animation(100, dim,dim, new int[] { 12 });
            _aniControl.Fall = _aniControl.Fly;
            _aniControl.Land = new Animation(100, dim,dim, new int[] { 13, 14, 15, 16, 17 });
            _aniControl.Wall = new Animation(100, dim,dim, new int[] { 18, 19, 20, 21, 22, 23 });
            _attachedAnimations = new List<AnimationContainer>();
            //CalculateTotalBonus();
        }

        public Player(string CharFile, Rectangle spawnPos, int id):base(null,spawnPos,id)
        {
            _inventory = new Logic.Inventory(this);
            if (_lifeBar == null)
                _lifeBar = ContentInterface.LoadTexture("White");

            string fileContent = ContentInterface.LoadFile("chars/" + CharFile);
            string[] lines = fileContent.Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.None);
            int.TryParse(lines[0], out _rect.Width);
            int.TryParse(lines[1], out _rect.Height);
            int dimX;
            int dimY;
            int.TryParse(lines[2], out dimX);
            int.TryParse(lines[3], out dimY);
            string texture = lines[4];
            _texture = ContentInterface.LoadTexture(texture);
            int.TryParse(lines[5], out _hp);
            _maxhp = _hp;

            _aniControl = new AnimationController();
            for (int x = 0; x < lines.Length - 6; x++)
            {
                string[] nums = lines[6 + x].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                int[] iNums = new int[nums.Length - 1];
                for (int y = 0; y < nums.Length - 1; y++)
                {
                    iNums[y] = int.Parse(nums[y + 1]);
                }
                int num = int.Parse(nums[0]);
                int dim = _texture.Bounds.Height;
                switch (x)
                {
                    case 0: _aniControl.Run = new Animation(num, dimX, dimY, iNums); break;
                    case 1: _aniControl.Idle = new Animation(num, dimX, dimY, iNums); break;
                    case 2: _aniControl.Jump = new Animation(num, dimX, dimY, iNums); break;
                    case 3: _aniControl.Fly = new Animation(num, dimX, dimY, iNums); break;
                    case 4: _aniControl.Fall = new Animation(num, dimX, dimY, iNums); break;
                    case 5: _aniControl.Land = new Animation(num, dimX, dimY, iNums); break;
                    case 6: _aniControl.Wall = new Animation(num, dimX, dimY, iNums); break;
                    default: _aniControl.AddAnimation(new Animation(num, dimX, dimY, iNums)); break;
                }
            }

            _speed = Vector2.Zero;
            _onGround = false;
            //_bodyParts = new List<BodyPart>();
            flying = true;
            _collisionOffset = new Vector4((((16 - _rect.Width) % 16 + 16) % 16) + 0.6f, 0, (((16 - _rect.Height) % 16 + 16) % 16) + 0.99975f, 0.0015f);
            //_completeBody = new BaseBody(null);
            //CalculateTotalBonus();
            _pointer = new PlayerPointer(this._name, ContentInterface.LoadTexture("Arrow"));
            _body = new Body.Body(this);
            _attachedAnimations = new List<AnimationContainer>();
        }

        public Player(CharTemplate info, Rectangle spawnPos, int id)
            : base(null, spawnPos, id)
        {

        }

        public void Update(IMap map, float seconds)
        {
            _map=map;
            _lastSpeed = _speed;
            if (changed > 0)
                changed--;
            if (_moveVectorSet)
            {
                Move(_moveVector.X, _moveVector.Y, true);
            }

            if (!wallCollision) _speed.Y += gravity * seconds;
            if (_speed.X < 0) Left = true;
            else if (_speed.X > 0) Left = false;

            if (seconds > 0.1f)
                if (_speed.Y * seconds > 8)
                    _speed.Y = 8 / seconds;

            _body.Update(seconds * 1000f);

            for (int x = 0; x < _attachedAnimations.Count; x++)
            {
                if (!_attachedAnimations[x].Update(seconds * 1000f))
                {
                    _attachedAnimations.RemoveAt(x);
                    x--;
                }
            }

            if (_onGround && !flying && Math.Abs(_speed.X) > 0 && !wallCollision && (_aniControl.Run.GetFrameNum() == 3 || _aniControl.Run.GetFrameNum() == 0))
            {
                Vector2 pos = _position;
                pos.Y += Rect.Height;
                pos.X += Left ? Rect.Width : 0;
            }

            wallCollision = false;
            TryMove(_speed * seconds, map);
            if (!stunned)
                _aniControl.Update(seconds * 1000, this);
            if (wallCollision)
            {
                _speed.Y = 0;
            }

            #region Animation Handling
            
            if (jump)
            {
                if (!_onGround && !flying)
                {
                    jump = false;
                }
            }
                /*
            else if (_onGround && flying)
                Land.Update(seconds * 1000);
            else
                Run.Update(seconds * 1000);
             */


            if (_aniControl.Jump.GetFrameNum() == 5 && jump)
            {
                jump = false;
            }
            if (_aniControl.Land.GetFrameNum() == 4 && flying)
            {
                flying = false;
            }
            #endregion

            EventObject obj = map.GetEvent(this);
            if (obj != null)
                obj.Event(this);

            if (_targetExp > _exp)
            {
                _exp++;
                if (_exp >= nextLevelExp)
                {
                    int add = nextLevelExp - lastExp;
                    lastExp = _exp;
                    nextLevelExp += add + add / 3;
                    level++;
                }
            }

            if (_onGround && flying && _aniControl.Land.GetFrameNum() == 0)
            {
                
            }

            if (_onGround || wallCollision)
                airstun = false;

            if (stunTimeout <= 0 && !airstun)
                stunned = false;

            _invincibleTime -= seconds;
            stunTimeout -= seconds;
            _speed.X = 0;
        }

        public void SetMove(Vector3 move)
        {
            _moveVector.X = move.X;
            _moveVector.Y = move.Y;
            _speed.Y = move.Z;
            _moveVectorSet = true;
        }

        public void SetAnimation(bool force, int id)
        {
            _aniControl.SetAnimation(force, id);
        }

        void TryMove(Vector2 movement, IMap map)
        {
            Position += Vector2.UnitX * movement.X;
            if (map.Collides(this))
            {
                if (movement.X > 0)
                {
                    _position.X = (float)(16 * Math.Floor(_position.X / 16.0)+_collisionOffset.X);
                    Position = _position;
                }
                else
                {
                    _position.X = (float)(16 * Math.Ceiling(_position.X / 16.0)+_collisionOffset.Y);
                    Position = _position;
                }
                wallCollision = true;
                _speed.X = 0;
            }

            _onGround = false;
            Position += Vector2.UnitY * movement.Y;
            if (map.Collides(this))
            {
                if (_speed.Y > 0)
                {
                    _onGround = true;
                    _position.Y = (float)(16 * Math.Floor(_position.Y / 16.0)) + _collisionOffset.Z;
                    Position = _position;
                }
                else
                {
                    _position.Y = (float)(16 * Math.Ceiling(_position.Y / 16.0)) + _collisionOffset.W;
                    Position = _position;
                }
                _speed = Vector2.Zero;
            }
            if (!jump && !_onGround)
                flying = true;

        }

        public void GetExp(int amount)
        {
            _targetExp += amount;
        }

        public void Move(float xDiff, float yDiff, bool forced=false)
        {
            if (stunned && !forced)
                return;
            changed = (changed==2 || !forced && (_moveVector.X != xDiff || _moveVector.Y != yDiff)) ? 2 : 0;
            _moveVector = new Vector2(xDiff, yDiff);
            _moveVectorSet = forced;
            _speed.X += baseSpeed * xDiff;
            _body.Move(xDiff, yDiff);
        }

        public void Knockback(Vector2 movement)
        {
            SetMove(new Vector3(movement.X, 0, movement.Y));
            changed = 2;
            _onGround = false;
            stunned = true;
            airstun = true;
            flying = true;
            stunTimeout = 0.3f;
            _aniControl.Update(0, this);
        }

        public void Stun(float stunTime)
        {
            stunTimeout = stunTime;
            stunned = true;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (font != null && !string.IsNullOrWhiteSpace(this.Name) || CanInteract)
            {
                if (_nameSize == Vector2.Zero)
                    _nameSize = font.MeasureString(_name);
                Vector2 stringPosition = new Vector2(_rect.X + _rect.Width / 2 - 0.25f*_nameSize.X / 2, _rect.Y + _rect.Height);
                stringPosition.X = (int)(stringPosition.X*2)/2;
                stringPosition.Y = (int)(stringPosition.Y*2)/2;
                batch.DrawString(font, this.Name, stringPosition, Color.White, 0, Vector2.Zero, 0.25f, SpriteEffects.None, 0);
                if (CanInteract)
                {
                    _pointer.SetColor(Color.Gray);
                    Player player = _map.GetPlayer();
                    if (player != null && (player.Position - Position).Length() < 150)
                        _pointer.Draw(batch, new Vector2(_rect.X, _rect.Y), Camera.CurrentCam);
                }
                else
                {
                    _pointer.Draw(batch, new Vector2(_rect.X, _rect.Y), Camera.CurrentCam);
                }
            }
            
            if (_lifeBar != null && !string.IsNullOrWhiteSpace(this.Name))
            {
                batch.Draw(_lifeBar, new Rectangle(_rect.X, _rect.Y - 3, _rect.Width, 3), null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0.15f);
                batch.Draw(_lifeBar, new Rectangle(_rect.X, _rect.Y - 3, (int)(_rect.Width * _hp / _maxhp), 3), null, Color.Green, 0, Vector2.Zero, SpriteEffects.None, 0.12f);
            }
            batch.Draw(_texture, _rect, _aniControl.GetFrame(), ChangedMovement ? Color.Black : Color.White, 0, Vector2.Zero, Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.45f);
            for (int x = 0; x < _attachedAnimations.Count; x++)
                _attachedAnimations[x].Draw(batch, MidPosition,Left);
            
        }

        public void GetDamage(int damage)
        {
            _hp -= damage;
            _hp = _maxhp > _hp ? _hp : _maxhp;
        }

        /*
        public void CalculateTotalBonus()
        {
            _completeBody.Clear();
            //_completeBody.jumpBonus = 100f;
            _completeBody.speedBonus = 60f;
            for (int x = 0; x < _bodyParts.Count; x++)
                _completeBody.Add(_bodyParts[x]);
        }
         */

        public void GivePart(BodyPart part, bool autoequip)
        {
            //bool newPart = true;
            /*
            for (int x = 0; x < _bodyParts.Count; x++)
                if (_bodyParts[x].GetID() == part.GetID())
                    newPart = false;
            if (newPart)
            {
             */
            if (!autoequip || !_body.TryEquip(-1, part))
                _inventory.Add(part);
            //_bodyParts.Add(part);
            //CalculateTotalBonus();
            //}
        }
        /*
        public void RemovePart(BodyPart part)
        {
            _bodyParts.Remove(part);
            CalculateTotalBonus();
        }
         */

        public void Jump()
        {
            if (!stunned)
            {
                _body.Jump(1);
                changed = 2;
            }
            /*
            if (_onGround)
            {
                _speed.Y = -_completeBody.jumpBonus;
                _onGround = false;
                jump = false;
                flying = true;
            }
            else if (_completeBody.airJumpCount < _completeBody.airJumpMax)
            {
                 _completeBody.airJumpCount++;
                _speed.Y -= _completeBody.airJumpStrength;
                _speed.Y = _speed.Y > -_completeBody.jumpBonus ? _speed.Y : -_completeBody.jumpBonus;
            }
             */
        }

        public void AttachAnimation(string animationFile)
        {
            _attachedAnimations.Add(new AnimationContainer(animationFile));
        }

        public void SetGroup(int id)
        {
            groupID = id;
            switch(id)
            {
                case 1: _pointer.SetColor(Color.Green); break;
                case 2: _pointer.SetColor(Color.Red); break;
                case 3: _pointer.SetColor(Color.RoyalBlue); break;
            }
        }

        public int HP
        {
            get { return _hp; }
            set { _hp = value; }
        }

        public int MaxHP
        {
            get { return _maxhp; }
            set { _maxhp = value; }
        }

        public bool OnGround
        {
            get { return _onGround; }
        }

        public bool OnWall
        {
            get { return wallCollision; }
        }

        public bool Flying
        {
            get { return flying; }
        }

        public bool Jumping
        {
            get { return jump; }
        }

        public int Exp
        {
            set { _exp = value; _targetExp = _exp; }
            get { return _exp; }
        }

        public int NextLevelExp
        {
            set { nextLevelExp = value; }
            get { return nextLevelExp; }
        }

        public int LastLevelExp
        {
            set { lastExp = value; }
            get { return lastExp; }
        }

        public int Level
        {
            get { return level; }
        }

        public Vector2 Speed
        {
            set { _speed = value; }
            get { return _speed; }
        }

        public Vector2 LastSpeed
        {
            get { return _lastSpeed; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; _pointer.SetName(_name); }
        }

        /*
        public List<BodyPart> Parts
        {
            get { return _bodyParts; }
        }
         */

        public Vector2 Movement
        {
            get { return _moveVector; }
        }

        public Body.Body Body
        {
            get { return _body; }
        }

        public Inventory Inventory
        {
            get { return _inventory; }
        }

        public IMap Map
        { get { return _map; } }


        public bool ChangedMovement
        {
            get { return changed > 0; }
            set { changed = value ? 2 : 0; }
        }
    }
}