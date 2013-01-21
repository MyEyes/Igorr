using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Client.Logic
{
    class AnimationController
    {
        Animation idle;
        Animation run;
        Animation jump;
        Animation fly;
        Animation land;
        Animation fall;
        Animation wall;
        Animation currentAnimation;
        List<Animation> _extraAnimations;
        bool keepAni;

        public AnimationController()
        {
            _extraAnimations = new List<Animation>();
        }

        void SetAnimation(Animation animation)
        {
            if (currentAnimation != animation)
            {
                if (currentAnimation != null)
                    currentAnimation.SetFrame(0);
                currentAnimation = animation;
            }
        }

        public void Update(float ms, Player player)
        {

            if (!keepAni)
            {
                if (player.OnGround && !player.Jumping && !player.Flying && player.Speed.X != 0)
                    SetAnimation(Run);
                else if (player.OnGround && !player.Jumping && !player.Flying && player.Speed.X == 0)
                    SetAnimation(Idle);
                else if (player.OnGround && player.Flying)
                    SetAnimation(Land);
                else if (!player.OnGround && player.Flying && player.Speed.Y < 0)
                    SetAnimation(Fly);
                else if (!player.OnGround && player.Flying && player.Speed.Y > 0)
                    SetAnimation(Fall);
                else if (player.Jumping)
                    SetAnimation(Jump);
                else if (player.OnWall)
                    SetAnimation(Wall);
            }

            if (currentAnimation != null)
                currentAnimation.Update(ms);
        }

        public Rectangle GetFrame()
        {
            if (currentAnimation != null)
                return currentAnimation.GetFrame();
            return Rectangle.Empty;
        }

        public void SetAnimation(bool force, int extraID)
        {
            if (force && _extraAnimations.Count > extraID)
            {
                keepAni = force;
                currentAnimation = _extraAnimations[extraID];
            }
            else
            {
                keepAni = false;
            }
        }

        public void AddAnimation(Animation ani)
        {
            _extraAnimations.Add(ani);
        }

        public Animation Idle
        {
            get;
            set;
        }

        public Animation Run
        {
            get;
            set;
        }

        public Animation Land { get; set; }
        public Animation Fly { get; set; }
        public Animation Fall { get; set; }
        public Animation Jump { get; set; }
        public Animation Wall { get; set; }
    }
}
