using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DeathSpawner : Enemy
{

    public Texture2D texture = Resources.GetEnemyTexture("DeathSpawner");

    public float extraSteps;
    //How many Spawns it... Spawns.
    private float spawnAmnt = 4;

    public DeathSpawner(Vector2 position, float speed, IngameState gameState) :
        base(position, 150, Resources.GetEnemyTexture("DeathSpawner"), speed, 15, gameState)
    {
        extraSteps = gameState.rand.Next(150, 200);
    }

    public override void Damaged(float damageRecieved)
    {
        //If it's going to be killed, spawn spawns.
        if (damageRecieved >= health)
        {
            for (int i = 0; i < spawnAmnt; i++)
            {
                gameState.enemies.Add(new Spawn(new Vector2(position.X + gameState.rand.Next(-50, 50),
                    position.Y + gameState.rand.Next(-50, 50)), 2, gameState));
            }
        }

        base.Damaged(damageRecieved);
    }

    public override void Update(GameTime gameTime)
    {
        if (alive)
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
            else
            {
                //If it's at it's target, self destruct to release more spawns than
                //if the player had destroyed it in time.
                spawnAmnt = spawnAmnt + (spawnAmnt / 2);
                Damaged(health);

                alive = false;
            }
            #endregion



            base.Update(gameTime);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}