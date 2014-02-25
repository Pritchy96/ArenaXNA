using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public class PlayerTurretSpread : PlayerTurret
    {


        public PlayerTurretSpread()
            : base(10, 150, Resources.GetSound("Laser"), Resources.GetPlayerTextures()["PlayerTurretSpread"])
        {
            
        }

        public override void Fire(IngameState state, Microsoft.Xna.Framework.Vector2 mousePos)
        {
            Vector2 vector = state.camera.relativeXY(new Vector2(mousePos.X, mousePos.Y)) - firePosition;
            vector.Normalize();
            
            //First bullet, which follows the path of the mouse.
            state.bullets.Add(new Bullet(firePosition, vector, damage, true));

            //The angle by which we rotate the bullet vectors by for spread shot.
            float angle = 0.1f;

            //Creating and using a Matrix to rotate the line by an angle.
            Matrix rotMatrix = Matrix.CreateRotationZ(angle);
            state.bullets.Add(new Bullet(firePosition, Vector2.Transform(vector, rotMatrix), damage, true));

            //Creating and using a Matrix to rotate the line by an angle (the other way).
            rotMatrix = Matrix.CreateRotationZ(-angle);
            state.bullets.Add(new Bullet(firePosition, Vector2.Transform(vector, rotMatrix), damage, true));

            gunSound.Play();
            timeSinceShot = 0;
        }
    }
