using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BasicShooter : Enemy
{
    //A random number of extra update cycles for which the enemy is allowed to move. 
    //This is so it doesn't stop right on the edge of the screen.
    int extraSteps;

    public float timeSinceShot = 0;
    public float fireDelay;
    public float damage = 10;

    public Texture2D texture = Resources.GetEnemyTexture("BasicShooter");
    public SoundEffect gunSound = Resources.GetSound("Laser");

    public BasicShooter(Vector2 position, float speed, IngameState gameState) :
        base(position, 100, Resources.GetEnemyTexture("BasicShooter"), speed, 10, gameState)
    {
        extraSteps = gameState.rand.Next(50, 200);
        fireDelay = gameState.rand.Next(3900, 4500);
    }

    public override void Update(GameTime gameTime)
    {
        #region Movement
        //Calculating moveVector to move along.
        Vector2 moveVector = gameState.player.position - base.position;
        moveVector.Normalize();

        base.rotation = gameState.ToAngle(new Vector2(moveVector.Y, moveVector.X));

        //Moving while not in screen.
        if (!gameState.window.Contains(new Point((int)position.X, (int)position.Y)))
        {
            base.position += moveVector;
            extraSteps = gameState.rand.Next(50, 200);
        }
        //Continuing to move while we have extra steps.
        else if (extraSteps > 0)
        {
            base.position += moveVector;
            extraSteps--;
        }
        #endregion

        #region Firing
        //If the enemy is in the game window (stops firing from offscreen)
        if (gameState.window.Contains(new Point((int)position.X, (int)position.Y)))
        {
            //If it's been long enough since last shot
            if (timeSinceShot >= fireDelay)
            {
                //Calculating a moveVector to shoot along.
                Vector2 shootVector = gameState.player.center - position;
                shootVector.Normalize();
                //Shooting and resetting timer.
                gameState.bullets.Add(new Bullet(position, shootVector, damage, false));
                gunSound.Play(1, 0.7f, 1);
                timeSinceShot = 0;
            }
        }
        //Updating timer with elapsed time since last update.
        timeSinceShot += (float)gameTime.ElapsedGameTime.Milliseconds;
        #endregion

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (base.alive)
        {
            base.Draw(spriteBatch);
        }
    }
}