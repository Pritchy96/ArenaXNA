using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections;


    public class Input
    {
        //Variables
        private KeyboardState keyboardState;
        private static Keys[] keys;

        private static ArrayList keysDown = new ArrayList();
        private static ArrayList keysDownLastFrame = new ArrayList();

        private bool dragging = false;

        private MouseState mouseState;
        private static int x, y, dy, dx = 0;
        private static bool left, right, middle = false;
        private static bool leftLastFrame, rightLastFrame, middleLastFrame = false;
        //holds the x and y of the last frame
        private static int yLastFrame, xLastFrame = 0;

        private Camera camera;

        //Mouse clicked event variables
        //Stores the mouse button that was clicked last on the MouseDown event
        private MouseButton LastMouseDown = MouseButton.None;

        private bool isMouseDown;

        public bool IsMouseDown
        {
            get { return isMouseDown; }
            set { isMouseDown = value; }
        }

        private Rectangle dragRect;

        public Rectangle DragRect
        {
            get { return dragRect; }
            set { dragRect = value; }
        }

        private Vector2 dragOrigin = Vector2.Zero;

        public Vector2 DragOrigin
        {
            get { return dragOrigin; }
            set { dragOrigin = value; }
        }


        //Properties
        public bool KeyIsDown
        {
            get { return keysDown.Count > 0; }
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public Vector2 MousePos
        {
            get { return new Vector2(X, Y); }
        }

        public int DX
        {
            get { return dx; }
        }

        public int DY
        {
            get { return dy; }
        }

        public Vector2 DeltaMousePos
        {
            get { return new Vector2(DY, DX); }
        }

        public bool Left
        {
            get { return left; }
        }

        public bool Right
        {
            get { return right; }
        }

        public bool Middle
        {
            get { return middle; }
        }

        public int ScrollWheelValue
        {
            get { return mouseState.ScrollWheelValue; }
        }

        //Events
        public delegate void KeyHandler(Keys[] keys);
        public event KeyHandler KeyPress;
        public event KeyHandler KeyDown;

        public delegate void MouseHandler(int x, int y, MouseButton button);
        public event MouseHandler MouseUp;
        public event MouseHandler MouseDown;
        public event MouseHandler MouseClicked;

        public delegate void MouseMovedHandler(int x, int y);
        public event MouseMovedHandler MouseMoved;

        public Input()
        {
            //Stops first tick errors
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        //Passed during gameInstance initialisation.
        public void setup(Camera camera)
        {
            this.camera = camera;
        }

        public void Update(GameTime gameTime)
        {
            #region Storing various values
            //updates the states of the mouse, keybaord and controller.
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            //store the x and y values of the mouse
            x = mouseState.X;
            y = mouseState.Y;

            //mouse button down bools
            right = (mouseState.RightButton == ButtonState.Pressed);
            left = (mouseState.LeftButton == ButtonState.Pressed);
            middle = (mouseState.MiddleButton == ButtonState.Pressed);

            if (keyboardState.GetPressedKeys().Length > 0)
            {
                keysDown.Clear();
                keysDown.Add(keyboardState.GetPressedKeys());
                keys = keyboardState.GetPressedKeys();
            }
            else
            {
                keysDown.Clear();
            }
            #endregion

            #region MouseDown Triggering
            //If the mouse is pressed, we fire the mouse down event
            if (MouseDown != null)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && !leftLastFrame)
                {
                    MouseDown(X, Y, MouseButton.Left);
                    LastMouseDown = MouseButton.Left;
                    isMouseDown = true;
                }
                else if (mouseState.RightButton == ButtonState.Pressed && !rightLastFrame)
                {
                    MouseDown(X, Y, MouseButton.Right);
                    LastMouseDown = MouseButton.Right;
                    isMouseDown = true;
                }
                else if (mouseState.MiddleButton == ButtonState.Pressed && !middleLastFrame)
                {
                    MouseDown(X, Y, MouseButton.Middle);
                    LastMouseDown = MouseButton.Middle;
                    isMouseDown = true;
                }
            }
            #endregion

            #region MouseClicked Triggering
            if (MouseClicked != null && !dragging)
            {
                if (!left && leftLastFrame)
                {
                    MouseClicked(X, Y, MouseButton.Left);
                }

                if (!right && rightLastFrame)
                {
                    MouseClicked(X, Y, MouseButton.Right);
                }

                if (!middle && middleLastFrame)
                {
                    MouseClicked(X, Y, MouseButton.Middle);
                }
            }
            #endregion

            #region MouseUp Triggering
            //If the mouse is released, we fire the mouse up event
            if (MouseUp != null)
            {
                if (mouseState.LeftButton == ButtonState.Released && leftLastFrame)
                {
                    MouseUp(X, Y, MouseButton.Left);

                    //Mouse is up, dragging ends.
                    dragging = false;
                    dragOrigin = Vector2.Zero;
                    dragRect = Rectangle.Empty;
                }
                if (mouseState.RightButton == ButtonState.Released && rightLastFrame)
                {
                    MouseUp(X, Y, MouseButton.Right);

                    //Mouse is up, dragging ends.
                    dragging = false;
                    dragOrigin = Vector2.Zero;
                    dragRect = Rectangle.Empty;
                }
                if (mouseState.MiddleButton == ButtonState.Released && middleLastFrame)
                {
                    MouseUp(X, Y, MouseButton.Middle);

                    //Mouse is up, dragging ends.
                    dragging = false;
                    dragOrigin = Vector2.Zero;
                    dragRect = Rectangle.Empty;
                }

                if (!left && !right && !middle)
                {
                    IsMouseDown = false;
                }

            }
            #endregion

            #region KeyPress Triggering
            if (KeyPress != null && keysDownLastFrame.Count > 0)
            {
                ArrayList pressedKeys = new ArrayList();

                foreach (object k in keysDownLastFrame)
                    {
                    if (!keysDown.Contains(k))
                    {
                        pressedKeys.Add(k);
                    }
                }

                Keys[] returnVar = new Keys[pressedKeys.Count];

                for (int i = 0; i < pressedKeys.Count; i++)
                {
                    returnVar[i] = (Keys)pressedKeys[i];
                }

                KeyPress(returnVar);


                if (!right && rightLastFrame)
                {
                    MouseClicked(X, Y, MouseButton.Right);
                }

                if (!middle && middleLastFrame)
                {
                    MouseClicked(X, Y, MouseButton.Middle);
                }
            }
            #endregion

            if (keysDown.Count > 0)
            {
                KeyDown(keys);
            }

            

            #region DY and DX calculation
            //Caluclating delta x and delta y
            dy = y - yLastFrame;
            dx = x - xLastFrame;
            #endregion

            #region Drag triggering
            //if we have dragged while mouse is down, begin dragging code.
            if (isMouseDown && ((dy > 2 || dy < -2) || (dx > 2 || dx < -2)))
            {
                dragging = true;
            }
            #endregion

            #region MouseMoved Triggering
            //Fires the mouse moved event if there has been a change in position of the mouse
            if (MouseMoved != null)
            {
                if (dy > 0 || dy < 0 || dx > 0 || dx < 0)
                {
                    MouseMoved(X, Y);

                    if (dragging == true)
                    {
                        if (dragOrigin == Vector2.Zero)
                        {
                            //if there is a camera, use relative xy, otherwise assume a screen
                            //sized world.
                            if (camera != null)
                            {
                                dragOrigin = camera.relativeXY(new Vector2(X, Y));
                            }
                            else
                            {
                                dragOrigin = new Vector2(X, Y);
                            }
                        }
                        else
                        {
                            Vector2 relativeXY;

                            try
                            {
                                relativeXY = camera.relativeXY(new Vector2(X, Y));
                            }
                            catch
                            {
                                relativeXY = new Vector2(X, Y);
                            }
                            int xRect = Math.Min((int)dragOrigin.X, (int)(relativeXY.X));
                            int yRect = Math.Min((int)dragOrigin.Y, (int)(relativeXY.Y));
                            int widthRect = Math.Max((int)dragOrigin.X, (int)(relativeXY.X)) - xRect;
                            int heightRect = Math.Max((int)dragOrigin.Y, (int)(relativeXY.Y)) - yRect;

                            dragRect = new Rectangle(xRect, yRect, widthRect, heightRect);
                        }
                    }
                }
            }
            #endregion

            #region Storing last frame values
            //Stores the x and y of this frame so we cna calculate DY and DX later on
            yLastFrame = y;
            xLastFrame = x;

            leftLastFrame = left;
            rightLastFrame = right;
            middleLastFrame = middle;

            keysDownLastFrame = keysDown;
            keysDown.Clear();
            #endregion
        }

        public bool IsKeyUp(Keys key)
        {
            return keyboardState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }


    }
