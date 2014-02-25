using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Spawn : Enemy
{
    float damage = 5;

    public Texture2D texture = Resources.GetEnemyTexture("Spawn");

    public Spawn(Vector2 position, float speed, IngameState gameState) :
        base(position, 40, Resources.GetEnemyTexture("Spawn"), speed, 2, gameState)
    {
    }

    public override void Update(GameTime gameTime)
    {
        #region Movement
        Vector2 vector = gameState.player.center - base.position;
        vector.Normalize();
        base.position += vector;
        #endregion

        if (alive && gameState.player.wheelRect.Contains(new Point((int)position.X, (int)position.Y)))
        {
            gameState.player.health -= damage;
            base.alive = false;
        }

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