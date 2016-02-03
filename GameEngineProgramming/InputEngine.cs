using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace CrappyBird
{
    public class InputEngine : GameComponent
    {
        private static KeyboardState previousKeyState;
        private static KeyboardState currentKeyState;

        private static Vector2 previousMousePos;
        private static Vector2 currentMousePos;

        private static MouseState previousMouseState;
        private static MouseState currentMouseState;

        private static TouchCollection currentTouchPoints;

        private static bool _isTouchEnabled = false;
        private static List<GestureSample> currentGestures = new List<GestureSample>();

        public static TouchCollection Touches { get { return currentTouchPoints; } }

        public InputEngine(Game game)
            : base(game)
        {
            currentKeyState = Keyboard.GetState();

            Game.Components.Add(this);
        }

        public override void Initialize()
        {
            previousMouseState = Mouse.GetState();
            currentMouseState = Mouse.GetState();
            currentKeyState = Keyboard.GetState();

            TouchPanelCapabilities cap = TouchPanel.GetCapabilities();
            _isTouchEnabled = cap.IsConnected;

            if (_isTouchEnabled)
            {
			//set the gestures you want available here
                TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Hold;
            }

            base.Initialize();
        }

        public static void ClearState()
        {
            previousMouseState = Mouse.GetState();
            currentMouseState = Mouse.GetState();
            currentKeyState = Keyboard.GetState();
        }

        public override void Update(GameTime delta)
        {
            previousKeyState = currentKeyState;

            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            currentMouseState = Mouse.GetState();

            if (_isTouchEnabled)
            {
                currentTouchPoints = TouchPanel.GetState();

                currentGestures.Clear();

                while (TouchPanel.IsGestureAvailable)
                {
                    currentGestures.Add(TouchPanel.ReadGesture());
                }
            }

            base.Update(delta);
        }


        public static bool IsGesturePresent(GestureType _type)
        {
            if (_isTouchEnabled)
            {
                foreach (var g in currentGestures)
                    if (g.GestureType == _type)
                    {
                        return true;
                    }
            }
            else return false;

            return false;
        }

        public static GestureSample GetGestureSample(GestureType _type)
        {
            if (IsGesturePresent(_type))
            {
                foreach (var g in currentGestures)
                    if (g.GestureType == _type)
                        return g;
            }

            return new GestureSample(GestureType.None, TimeSpan.FromDays(999), Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero);
        }

        public static bool IsKeyHeld(Keys buttonToCheck)
        {
            if (currentKeyState.IsKeyDown(buttonToCheck))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static bool IsKeyPressed(Keys keyToCheck)
        {
            if (currentKeyState.IsKeyUp(keyToCheck) && previousKeyState.IsKeyDown(keyToCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static KeyboardState CurrentKeyState
        {
            get { return currentKeyState; }
        }

        public static MouseState CurrentMouseState
        {
            get { return currentMouseState; }
        }

        public static MouseState PreviousMouseState
        {
            get { return previousMouseState; }
        }

        public static bool IsMouseLeftClick()
        {
            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else 
                return false;
        }

        public static bool IsMouseRightClick()
        {
            if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static bool IsMouseRightHeld()
        {
            if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static bool IsMouseLeftHeld()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static Vector2 MousePosition
        {
            get { return currentMousePos; }
        }

    }
}
