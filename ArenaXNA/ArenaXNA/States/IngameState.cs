using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class IngameState : BasicGameState
{
    Texture2D map;

    public Random rand = new Random();

    public Player player;
    public Vector2 mousePos;
    public Camera camera = new Camera();
    public Boolean paused = false;

    //All enemies.
    public ArrayList enemies = new ArrayList();
    //Pool for spawning, contains an instance of each enemy.
    public Enemy[] enemyPoolSize;
    public int waveNumber = 1;
    public int kills = 0;

    //ALL bullets, both Players and Enemies.
    //can be sorted using a boolean within bullet. (BelongsToPlayer)
    public ArrayList bullets = new ArrayList();

    //Spawning variables.
    public float timeSinceSpawn = 0;
    public float timeBetweenSpawn = 1000;

    //The current position of the window in world coords.
    public Rectangle window;

    public IngameState()
        : base("Ingame")
    {
        player = new Player(this);
        map = Resources.GetMapTexture("Map");

    enemyPoolSize = new Enemy[]
        {
            new Suicide(new Vector2(-20, -20), 0, this),
            new EMPSuicide(new Vector2(-20, -20), 0, this),
            new BasicShooter(new Vector2(-20, -20), 0, this),
            new DeathSpawner(new Vector2(-20, -20), 0, this),
        };

        //Initial Wave.
        NewWave();
    }

    public override void Update(GameTime gameTime)
    {
        if (!paused)
        {
            #region Bullet hit detection and Bullet despawning.
            foreach (Bullet b in bullets.ToArray())
            {
                //Reducing life of bullets.
                b.life -= gameTime.ElapsedGameTime.Milliseconds;

                //Bullet despawning.
                if (b.life <= 0)
                {
                    bullets.Remove(b);
                }

                //Enemy hit detection.
                foreach (Enemy e in enemies.ToArray())
                {
                    if (e.rect.Contains(new Point((int)b.position.X, (int)b.position.Y)) && b.belongsToPlayer)
                    {
                        bullets.Remove(b);
                        e.Damaged(b.damage);
                    }
                }

                //Player hit detection.
                if (player.wheelRect.Contains(new Point((int)b.position.X, (int)b.position.Y)) && !b.belongsToPlayer)
                {
                    bullets.Remove(b);
                    player.health -= b.damage;
                }
            }


            #endregion

            #region Firing
            //If mouse is down.
            if (base.GetInput().IsMouseDown)
            {
                #region Left
                if (base.GetInput().Left == true && player.turretLeft.timeSinceShot >= player.turretLeft.fireDelay)
                {
                    player.turretLeft.Fire(this, new Vector2(GetInput().X, GetInput().Y));
                }
                #endregion

                #region Right
                if (base.GetInput().Right == true && player.turretRight.timeSinceShot >= player.turretRight.fireDelay)
                {
                    player.turretRight.Fire(this, new Vector2(GetInput().X, GetInput().Y));
                }
                #endregion
            }
            player.turretLeft.timeSinceShot += (float)gameTime.ElapsedGameTime.Milliseconds;
            player.turretRight.timeSinceShot += (float)gameTime.ElapsedGameTime.Milliseconds;
            #endregion

            #region Spawning
            if (enemies.Count == 0)
            {
                waveNumber++;
                NewWave();
            }
            #endregion

            //Reducing EMP time if needed.
            if (player.EMPTime > 0)
                player.EMPTime -= gameTime.ElapsedGameTime.Milliseconds;

            //Window rectangle within world coords.
            window = new Rectangle((int)camera.Position.X, (int)camera.Position.Y, 
                GameClass.Game_Width, GameClass.Game_Height);

            //Update other classes.
            camera.Update(GetInput(), player);
            

            foreach (Enemy e in enemies.ToArray())
            {
                e.Update(gameTime);
            }
        }

        base.Update(gameTime);
    }

    public void NewWave()
    {
        int wavePool = (int)Math.Ceiling(waveNumber * 5.2);

        while (wavePool > enemyPoolSize[0].spawnSlotSize)
        {
            int enemySelector = rand.Next(0, enemyPoolSize.Count());
            enemies.Add(enemyPoolSize[enemySelector]);
            wavePool -= enemyPoolSize[enemySelector].spawnSlotSize;

            #region Positioning
            Vector2 origin = camera.relativeXY(Vector2.Zero);
            Rectangle rect = new Rectangle((int)(origin.X), (int)(origin.Y), camera.Viewport.Width, camera.Viewport.Height);

            Vector2 newPosition = new Vector2(rand.Next((int)(origin.X - 50), (int)(origin.X + camera.Viewport.Width + 50)),
                rand.Next((int)(origin.Y - 50), (int)(origin.Y + camera.Viewport.Height + 50)));

            while (rect.Contains(new Point((int)newPosition.X, (int)newPosition.Y)))
            {
                newPosition = new Vector2(rand.Next((int)(origin.X - 50), (int)(origin.X + camera.Viewport.Width + 50)),
                    rand.Next((int)(origin.Y - 50), (int)(origin.Y + camera.Viewport.Height + 50)));
            }

            enemyPoolSize[enemySelector].position = newPosition;

            #endregion

            #region Recreating Spawnpool

            enemyPoolSize = new Enemy[]
        {
            new Suicide(new Vector2(-20, -20), 0, this),
            new EMPSuicide(new Vector2(-20, -20), 0, this),
            new BasicShooter(new Vector2(-20, -20), 0, this),
            new DeathSpawner(new Vector2(-20, -20), 0, this),
        };
            #endregion
        }
    }

    /// <summary>
    /// Converts a normalised Vector into an Angle.
    /// </summary>
    public float ToAngle(Vector2 vector)
    {
        float angle = (float)(Math.Atan((vector.X / vector.Y)));

        if (vector.X < 0 && vector.Y > 0)
        {
            angle += (float)((Math.PI));
        }

        if (vector.X < 0 || vector.Y < 0)
        {
            angle += (float)((Math.PI));
        }

        return angle;
    }

    public override void MouseMoved(int x, int y)
    {
        if (!paused)
        {
            Vector2 relativeXY = camera.relativeXY(new Vector2(x, y));

            #region Chassis Rotation.
            float dY = ((float)relativeXY.X - (float)player.position.X);
            float dX = ((float)relativeXY.Y - (float)player.position.Y);


            player.chassis.angle = ToAngle(new Vector2(dX, dY));
            #endregion


            #region Turret Rotation.

            #region Left
            dY = ((float)relativeXY.X - (float)player.turretLeft.position.X);
            dX = ((float)relativeXY.Y - (float)player.turretLeft.position.Y);
            player.turretLeft.angle = ToAngle(new Vector2(dX, dY));
            #endregion

            #region Right
            dY = ((float)relativeXY.X - (float)player.turretRight.position.X);
            dX = ((float)relativeXY.Y - (float)player.turretRight.position.Y);
            player.turretRight.angle = (float)(Math.Atan((dX / dY)));

            if (dX < 0 && dY > 0)
            {
                player.turretRight.angle += (float)((Math.PI));
            }

            if (dX < 0 || dY < 0)
            {
                player.turretRight.angle += (float)((Math.PI));
            }
            #endregion

            #endregion

            mousePos = new Vector2(x, y);
        }
    }

    //Player Movement
    public override void KeyDown(Keys[] keys)
    {
        #region Player Movement
        if (player.EMPTime <= 0 && !paused)
        {
            Vector2 movement = Vector2.Zero;
            bool moveKey = false;

            if (keys.Contains<Keys>(Keys.W) && player.position.Y > 20)
            {
                movement.Y--;
                moveKey = true;
            }
            if (keys.Contains<Keys>(Keys.S) && player.position.Y < map.Height - 20)
            {
                movement.Y++;
                moveKey = true;
            }
            if (keys.Contains<Keys>(Keys.A) && player.position.X > 20)
            {
                movement.X--;
                moveKey = true;
            }
            if (keys.Contains<Keys>(Keys.D) && player.position.X < map.Width - 20)
            {
                movement.X++;
                moveKey = true;
            }

            if (moveKey && movement.Length() != 0)
            {
                movement.Normalize();
                player.position += movement * player.speed;
                player.wheels.angle = ToAngle(new Vector2(movement.Y, movement.X));
                moveKey = false;
            }
        }
        #endregion
    }

    public override void KeyPress(Keys[] keys)
    {
        if (keys.Contains<Keys>(Keys.Space))
        {
            paused = !paused;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        #region Camera Relative Drawing
        //Ending spritebatch in order to begin a camera relative one.
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            null, null, null, null, camera.CameraMatrix);

        //Draw map.
        spriteBatch.Draw(map, new Vector2(0, 0), Color.White);

        #region Bullets
        foreach (Bullet b in bullets)
        {
            b.position += b.vector * b.speed;

            //Colouring the bullet depending on who it belongs to.
            if (b.belongsToPlayer)
            {
                spriteBatch.Draw(b.graphic, new Rectangle((int)(b.position.X - b.graphic.Width / 2),
                    (int)(b.position.Y - b.graphic.Height / 2), b.graphic.Width, b.graphic.Height), Color.Cyan);
            }
            else
            {
                spriteBatch.Draw(b.graphic, new Rectangle((int)(b.position.X - b.graphic.Width / 2),
                    (int)(b.position.Y - b.graphic.Height / 2), b.graphic.Width, b.graphic.Height), Color.Orange);
            }
        }
        #endregion

        foreach (Enemy e in enemies)
        {
            e.Draw(spriteBatch);
        }

        player.Draw(spriteBatch);
        #endregion

        #region GUI drawing
        //Ending spritebatch in order to begin a non camera relative one so 
        //the GUI can be drawn over everything else.
        spriteBatch.End();
        spriteBatch.Begin();

        //DELETE: Debug wave and kills.
        spriteBatch.DrawString(Resources.TestFont, "Wave: " + waveNumber.ToString(), new Vector2(0, 0), Color.White);
        spriteBatch.DrawString(Resources.TestFont, "Kills: " + kills, new Vector2(0, 20), Color.White);


        #region Paused screen.
        if (paused)
        {
            spriteBatch.Draw(Resources.GetGUITexture("PausedBackground"), new Rectangle(0, 0, GameClass.Game_Width, GameClass.Game_Height), Color.White);
        }
        #endregion
        #endregion
    }
}
