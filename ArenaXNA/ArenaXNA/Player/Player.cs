using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class Player
    {
        IngameState game;

        public Vector2 position = new Vector2(100, 100);

        //Center position of player, updated in draw method.
        public Vector2 center;

        public PlayerWheels wheels;
        public PlayerChassis chassis;
        public PlayerTurret turretLeft;
        public PlayerTurret turretRight;

        public Healthbar healthbar;
        
        public float speed = 2;
        public float EMPTime; 
        public float health = 500;
        public float maxHealth = 500;

        public Rectangle wheelRect;
        

        public Player(IngameState game)
        {
            this.game = game;

            wheels = new PlayerWheels();
            chassis = new PlayerChassis();
            turretLeft = new PlayerTurretInaccurate();
            turretRight = new PlayerTurretSpread();

            this.healthbar = new Healthbar(chassis.graphic);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            #region Finding values for offsetting.
            //Finding center for use in the next setions.
            Vector2 wheelCenter = new Vector2(wheels.graphic.Width / 2, wheels.graphic.Height / 2);
            Vector2 chassisCenter = new Vector2(chassis.graphic.Width / 2, chassis.graphic.Height / 2);
            Vector2 turretCenter = new Vector2(turretLeft.graphic.Width / 2, turretLeft.graphic.Height / 2);

            //Offset of the turretLeft relative to the chassis.
            float turretOffset = 35;
            #endregion

            #region Turret Positioning
            float turretHeight = (float)(Math.Cos(-chassis.angle) * -turretOffset) + position.Y;
            float turretWidth = (float)(Math.Sin(-chassis.angle) * -turretOffset) + position.X;

            //The position of the turretLeft in world space.
            turretLeft.position = new Vector2((turretWidth + chassisCenter.X), (turretHeight + chassisCenter.Y));

            //The position from which the turret should fire from in world space. Basic Trig.
            turretLeft.firePosition = new Vector2(
                (int) (turretLeft.position.X + (turretLeft.graphic.Width * Math.Cos(turretLeft.angle))),
                (int) (turretLeft.position.Y + (turretLeft.graphic.Width * Math.Sin(turretLeft.angle)))
                );

            turretHeight = (float)(Math.Cos(-chassis.angle) * turretOffset) + position.Y;
            turretWidth = (float)(Math.Sin(-chassis.angle) * turretOffset) + position.X;

            //The position of turretRight in world space.
            turretRight.position = new Vector2((turretWidth + chassisCenter.X), (turretHeight + chassisCenter.Y));

            //The position from which the turret should fire from in world space. Basic Trig.
            turretRight.firePosition = new Vector2(
            (int)(turretRight.position.X + (turretRight.graphic.Width * Math.Cos(turretRight.angle))),
            (int)(turretRight.position.Y + (turretRight.graphic.Width * Math.Sin(turretRight.angle)))
            );
            #endregion

            #region Rectangle Creation.
            //Creating the rectangle to draw each object in.
             wheelRect = new Rectangle((int)(position.X), (int)(position.Y), wheels.graphic.Width, wheels.graphic.Height);
            Rectangle chassisRect = new Rectangle((int)(position.X + wheelCenter.X), (int)(position.Y + wheelCenter.Y), chassis.graphic.Width, chassis.graphic.Height);
            Rectangle turretLeftRect = new Rectangle((int)(turretLeft.position.X), (int)(turretLeft.position.Y), turretLeft.graphic.Width, turretLeft.graphic.Height);
            Rectangle turretRightRect = new Rectangle((int)(turretRight.position.X), (int)(turretRight.position.Y), turretRight.graphic.Width, turretRight.graphic.Height);
            Rectangle healthRectBack = new Rectangle((int)(position.X), (int)(position.Y - chassis.graphic.Height / 3), chassis.graphic.Width, 10);
            Rectangle healthRectFront = new Rectangle((int)(position.X), (int)(position.Y - chassis.graphic.Height / 3), (int)(chassis.graphic.Width * (health / maxHealth)), 10);
            #endregion

            #region Drawing the objects.
            /*This is further offset so that the wheelrect is actually encapsulating the player sprite, for hit detection.
            //It should do anyway, but I think the offset in the draw call that allows it to rotate correctly is offsetting 
            it away from the rectangle, so we are offsetting it back. Confusing stuff.*/
            spriteBatch.Draw(wheels.graphic, new Rectangle((int)(wheelRect.X + wheelCenter.X), (int)(wheelRect.Y + wheelCenter.Y), wheelRect.Width, wheelRect.Height), null, Color.White, wheels.angle, wheelCenter, SpriteEffects.None, 0f);
            spriteBatch.Draw(turretLeft.graphic, turretLeftRect, null, Color.White, turretLeft.angle, new Vector2(turretCenter.X / 4, turretCenter.Y), SpriteEffects.None, 0f);
            spriteBatch.Draw(turretRight.graphic, turretRightRect, null, Color.White, turretRight.angle, new Vector2(turretCenter.X / 4, turretCenter.Y), SpriteEffects.None, 0f);
            spriteBatch.Draw(chassis.graphic, chassisRect, null, Color.White, chassis.angle, chassisCenter, SpriteEffects.None, 0f);

            //Draw Healthbar.
            spriteBatch.Draw(Resources.GetGUITexture("HealthBack"), healthRectBack, Color.White);
            spriteBatch.Draw(Resources.GetGUITexture("HealthFront"), healthRectFront, Color.Orange);

            //spriteBatch.Draw(Resources.GetGUITexture("HealthBack"), wheelRect, Color.White);
            // spriteBatch.Draw(Resources.GetGUITexture("HealthBack"), new Rectangle((int)position.X - 4, (int)position.Y - 4, 8,8), Color.Cyan);
            #endregion

            center = position + wheelCenter;
        }
    }
