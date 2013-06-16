using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IGORR.Client
{

    //When adding a new Action increment InputManager.TotalActions
    enum Actions
    {
        Left,
        Right,
        Jump,
        Interact,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Inventory,
        Menu,
        Character,
        SwitchAttackLeft,
        SwitchAttackRight
    }

    class InputManager
    {
        const int TotalActions = 13;
        float _direction;
        bool _jump;

        KeyboardState _prevKeyboard;
        MouseState _prevMouse;
        GamePadState _prevPad;

        Dictionary<Actions, Keys> _keybinds;

        bool[] _activated;

        Actions _leftAttack;
        Actions _rightAttack;

        public InputManager()
        {
            _keybinds = new Dictionary<Actions, Keys>();
            _activated = new bool[TotalActions];
            _keybinds.Add(Actions.Left, Keys.A);
            _keybinds.Add(Actions.Right, Keys.D);
            _keybinds.Add(Actions.Jump, Keys.Space);
            _keybinds.Add(Actions.Character, Keys.C);
            _keybinds.Add(Actions.Inventory, Keys.I);
            _keybinds.Add(Actions.Interact, Keys.Enter);
            _keybinds.Add(Actions.Menu, Keys.Escape);
            _keybinds.Add(Actions.SwitchAttackLeft, Keys.Q);
            _keybinds.Add(Actions.SwitchAttackRight, Keys.E);
            _leftAttack = Actions.Attack1;
            _rightAttack = Actions.Attack2;
        }

        public void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);

            ClearState();

            #region Movement Direction
            _direction = 0;

            if (keyboard.IsKeyDown(_keybinds[Actions.Left]) || gamepad.DPad.Left== ButtonState.Pressed)
                _direction += -1;
            if (keyboard.IsKeyDown(_keybinds[Actions.Right]) || gamepad.DPad.Right==ButtonState.Pressed)
                _direction += 1;
            _direction += gamepad.ThumbSticks.Left.X;
            if (_direction < -1) _direction = -1;
            if (_direction > 1) _direction = 1;
            #endregion

            #region Jumping
            _jump = false;

            if ((keyboard.IsKeyDown(_keybinds[Actions.Jump]) && !_prevKeyboard.IsKeyDown(_keybinds[Actions.Jump])) || ((gamepad.Buttons.A == ButtonState.Pressed)&&_prevPad.Buttons.A!=ButtonState.Pressed))
            {
                _jump = true;
            }
            #endregion

            #region Attacks

            if (mouse.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton != ButtonState.Pressed)
            {
                _activated[(int)_leftAttack] = true;
            }
            if (mouse.RightButton == ButtonState.Pressed && _prevMouse.RightButton != ButtonState.Pressed)
            {
                _activated[(int)_rightAttack] = true;
            }
            #endregion

            #region Menus

            if ((keyboard.IsKeyDown(_keybinds[Actions.Inventory]) && !_prevKeyboard.IsKeyDown(_keybinds[Actions.Inventory])))
                _activated[(int)Actions.Inventory] = true;
            if ((keyboard.IsKeyDown(_keybinds[Actions.Character]) && !_prevKeyboard.IsKeyDown(_keybinds[Actions.Character])))
                _activated[(int)Actions.Character] = true;
            if ((keyboard.IsKeyDown(_keybinds[Actions.Interact]) && !_prevKeyboard.IsKeyDown(_keybinds[Actions.Interact])))
                _activated[(int)Actions.Interact] = true;
            if ((keyboard.IsKeyDown(_keybinds[Actions.Menu]) && !_prevKeyboard.IsKeyDown(_keybinds[Actions.Menu])))
                _activated[(int)Actions.Menu] = true;
            #endregion

            #region AttackSwitching
            if((keyboard.IsKeyDown(_keybinds[Actions.SwitchAttackLeft]) && !_prevKeyboard.IsKeyDown(_keybinds[Actions.SwitchAttackLeft])))
            {
                _leftAttack = _rightAttack;
                if (_rightAttack != Actions.Attack4)
                {
                    int next = (int)_rightAttack;
                    next++;
                    _rightAttack = (Actions)next;
                }
                else
                    _rightAttack = Actions.Attack1;
            }

            if ((keyboard.IsKeyDown(_keybinds[Actions.SwitchAttackRight]) && !_prevKeyboard.IsKeyDown(_keybinds[Actions.SwitchAttackRight])))
            {
                _rightAttack = _leftAttack;
                if (_leftAttack != Actions.Attack1)
                {
                    int next = (int)_leftAttack;
                    next--;
                    _leftAttack = (Actions)next;
                }
                else
                    _leftAttack = Actions.Attack4;
            }
            #endregion

            _prevKeyboard = keyboard;
            _prevMouse = mouse;
            _prevPad = gamepad;
        }

        public bool isActive(Actions action)
        {
            return _activated[(int)action];
        }

        public void ClearState()
        {
            for (int x = 0; x < _activated.Length; x++)
                _activated[x] = false;
        }

        public bool Jump
        {
            get { return _jump; }
        }

        public float Direction { get { return _direction; } }

        public Actions LeftAttack { get { return _leftAttack; } }
        public Actions RightAttack { get { return _rightAttack; } }
    }
}
