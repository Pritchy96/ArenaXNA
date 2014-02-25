using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

    public class Camera
    {
        #region Variables

        //The cameras matrix that will be used in spriteBatch.Begin()
        private Matrix matrix = new Matrix();

        //The pixelPosition of the top left corner of the screen relative to the camera
        private Vector2 position = new Vector2(0, 0);

        //TODO: add support for full screen game
        private Viewport viewport = new Viewport(new Rectangle(0, 0, GameClass.Game_Width, GameClass.Game_Height));

        //We have methods that re-workout these values
        private int WorldWidth = Resources.GetMapTexture("Map").Width;
        private int WorldHeight = Resources.GetMapTexture("Map").Height;

        //allows us to disable the camera when the map is too small
        private bool enabled = true;
        #endregion

        public Matrix CameraMatrix
        {
            get { return matrix; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = ClampCamera(value); }
        }

        public Viewport Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }

        public Camera()
        {
            Position = new Vector2(0, 0);

            if (WorldWidth < viewport.Width || WorldHeight < viewport.Height)
            {
                enabled = false;
            }
            else
            {
                enabled = true;
            }
        }

        #region Function Explanation
        //Returns a vector2 that is within the Game field.
        #endregion
        private Vector2 ClampCamera(Vector2 value)
        {
            //Unfinished
            if (value.X < 0) { value.X = 0; }
            if (value.X > (WorldWidth) - GameClass.Game_Width) { value.X = (WorldWidth) - GameClass.Game_Width; }

            if (value.Y < 0) { value.Y = 0; }
            if (value.Y > (WorldHeight) - GameClass.Game_Height) { value.Y = (WorldHeight) - GameClass.Game_Height; }
            return value;
        }

        #region Function Explanation
        //Offsets the target pixelPosition by half of the screen so we can center on it.
        #endregion
        public void CenterCameraOn(Vector2 targetPixel)
        {
            Position = targetPixel - new Vector2(GameClass.Game_Width / 2, GameClass.Game_Height / 2);
        }

        #region Function Explanation
        //Changing the camera depending on our inputs.
        #endregion
        public void Update(Input input, Player player)
        {
            CenterCameraOn(new Vector2(player.position.X + player.chassis.graphic.Width/2,
                player.position.Y + player.chassis.graphic.Height/2));

            if (enabled)
            {
                #region Camera movement logic
                Vector2 movementVector = new Vector2(0, 0);
                //camera movement logic
                if (input.IsKeyDown(Keys.A))
                {
                    movementVector.X--;
                }
                if (input.IsKeyDown(Keys.D))
                {
                    movementVector.X++;
                }
                if (input.IsKeyDown(Keys.W))
                {
                    movementVector.Y--;
                }
                if (input.IsKeyDown(Keys.S))
                {
                    movementVector.Y++;
                }

                if (movementVector != Vector2.Zero)
                {
                    movementVector.Normalize();
                    Position += (movementVector * player.speed);
                }
                #endregion

                //Update the matrix
                matrix = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0));
            }
        }


        #region Function Explanation
        //A method which finds the mouse position within the
        //entire game, not just within the viewport. I made it called externally
        //so that this class can still be used with ease for menus,
        //which do not have a camera. This however means you must
        //remember to call this method on the XY variables before they
        //are used. We can easily do ingame menus now as we just don't use
        //this method, and use actual XY.
        #endregion
        public Vector2 relativeXY(Vector2 XY)
        {
            return Position + XY;
        }
    }