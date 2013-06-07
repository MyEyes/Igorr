using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IGORR.Client
{
    enum Actions
    {
        Left,
        Right,
        Jump,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Inventory,
        Menu,
        Character
    }

    class InputManager
    {
        float _direction;
        bool _jump;

        KeyboardState _prevKeyboard;
        MouseState _prevMouse;
        GamePadState _prevPad;

        Dictionary<Actions, Keys> _keybinds;

        public InputManager()
        {
            _keybinds = new Dictionary<Actions, Keys>();

            _keybinds.Add(Actions.Left, Keys.A);
            _keybinds.Add(Actions.Right, Keys.D);
            _keybinds.Add(Actions.Jump, Keys.Space);
        }

        public void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);

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

            #region Menus
            #endregion

            _prevKeyboard = keyboard;
            _prevMouse = mouse;
            _prevPad = gamepad;
        }

        public bool Jump
        {
            get { return _jump; }
        }

        public float Direction { get { return _direction; } }
    }
}
