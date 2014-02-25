using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Suicide : Enemy
{
    float damage = 20;

    public Texture2D texture = Resources.GetEnemyTexture("Suicide");


    public Suicide(Vector2 position, float speed, IngameState gameState) :
        base(position, 100, Resources.GetEnemyTexture("Suicide"), speed, 5, gameState)
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