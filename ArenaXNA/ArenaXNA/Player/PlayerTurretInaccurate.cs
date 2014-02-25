using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public class PlayerTurretInaccurate : PlayerTurret
    {

        public PlayerTurretInaccurate() 
            : base (5, 40, Resources.GetSound("Laser"), Resources.GetPlayerTextures()["PlayerTurret"])
        {
            
        }

        public override void Fire(IngameState state, Microsoft.Xna.Framework.Vector2 mousePos)
        {
            Vector2 vector = state.camera.relativeXY(new Vector2(mousePos.X, mousePos.Y)) - firePosition;
            vector.Normalize();

            //The angle by which we rotate the bullet vectors by for spread shot.
            float angle = (float)state.rand.Next(-100, 100)/1000;

            //Creating and using a Matrix to rotate the line by an angle.
            Matrix rotMatrix = Matrix.CreateRotationZ(angle);
            state.bullets.Add(new Bullet(firePosition, Vector2.Transform(vector, rotMatrix), damage, true));

            gunSound.Play();
            timeSinceShot = 0;
        }
    }
