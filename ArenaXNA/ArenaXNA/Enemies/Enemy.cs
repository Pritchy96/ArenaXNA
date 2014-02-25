using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Enemy
{
    public Vector2 position = new Vector2(0, 0);
    //center of sprite in world coords.
    //public Vector2 center = new Vector2(0, 0);
    public float health = 100;
    public float maxHealth = 100;
    public float speed;
    public Boolean alive = true;
    //A varible to show how many spawn "slot" sizes it takes up.
    public int spawnSlotSize = 5;

    public Rectangle rect;
    public Texture2D graphic;
    public float rotation = 0f;

    public Healthbar healthbar;

    protected IngameState gameState;
    

    public Enemy(Vector2 position, float health, Texture2D graphic, float speed, int spawnSlotSize, IngameState gameState)
    {
        this.position = position;
        this.health = health;
        maxHealth = health;
        this.graphic = graphic;
        this.speed = speed;
        this.spawnSlotSize = spawnSlotSize;
        this.gameState = gameState;

        this.healthbar = new Healthbar(graphic);

    }

    public virtual void Damaged(float damageRecieved)
    {
        health -= damageRecieved;

        //Deleting enemy if it's dead.
        if (health <= 0)
        {
            gameState.kills++;
            gameState.enemies.Remove(this);
        }

    }

    public virtual void Update(GameTime gameTime)
    {

    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {     
        rect = new Rectangle((int)(position.X - graphic.Width / 2), (int)(position.Y - graphic.Height / 2), graphic.Width, graphic.Height);
        Vector2 graphicCenter = new Vector2((graphic.Width / 2), (int)(graphic.Height / 2));
        

        spriteBatch.Draw(graphic, position, null, Color.White, rotation, graphicCenter, 1, SpriteEffects.None,0);

        //Debug
       // spriteBatch.Draw(Resources.GetEnemyTexture("Spawn"), new Vector2(position.X - 10, position.Y - 10), Color.White);

        //Top corner of sprite, for health bar drawing.
        Vector2 topCorner = position - graphicCenter;

        Rectangle healthRectBack = new Rectangle((int)(topCorner.X), (int)(topCorner.Y - graphic.Height / 3), graphic.Width, 10);
        Rectangle healthRectFront = new Rectangle((int)(topCorner.X), (int)(topCorner.Y - graphic.Height / 3), (int)(graphic.Width * (health / maxHealth)), 10);

        spriteBatch.Draw(Resources.GetGUITexture("HealthBack"), healthRectBack, Color.White);
        spriteBatch.Draw(Resources.GetGUITexture("HealthFront"), healthRectFront, Color.Red);
    }
}