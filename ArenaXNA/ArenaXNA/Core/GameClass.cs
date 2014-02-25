using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
public class GameClass : Microsoft.Xna.Framework.Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    StateManager stateManager;

    //Game constants
    public const int Game_Width = 800;
    public const int Game_Height = 600;

    public GameClass()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        //Set the size of the game window
        graphics.PreferredBackBufferHeight = Game_Height;
        graphics.PreferredBackBufferWidth = Game_Width;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        this.IsMouseVisible = true;
        base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        #region Player Graphics.
        Texture2D PlayerWheels = Content.Load<Texture2D>("Player/PlayerWheels");
        PlayerWheels.Name = "PlayerWheels";
        Resources.AddPlayerTexture(PlayerWheels);

        Texture2D PlayerTurret = Content.Load<Texture2D>("Player/PlayerTurret");
        PlayerTurret.Name = "PlayerTurret";
        Resources.AddPlayerTexture(PlayerTurret);

        Texture2D PlayerTurretSpread = Content.Load<Texture2D>("Player/PlayerTurretSpread");
        PlayerTurretSpread.Name = "PlayerTurretSpread";
        Resources.AddPlayerTexture(PlayerTurretSpread);

        Texture2D PlayerBase = Content.Load<Texture2D>("Player/PlayerChassis");
        PlayerBase.Name = "PlayerChassis";
        Resources.AddPlayerTexture(PlayerBase);
        #endregion

        #region Enemies
        Texture2D Suicide = Content.Load<Texture2D>("Enemies/Suicide");
        Suicide.Name = "Suicide";
        Resources.AddEnemyTexture(Suicide);

        Texture2D EMPSuicide = Content.Load<Texture2D>("Enemies/EMPSuicide");
        EMPSuicide.Name = "EMPSuicide";
        Resources.AddEnemyTexture(EMPSuicide);
         
        Texture2D BasicShooter = Content.Load<Texture2D>("Enemies/BasicShooter");
        BasicShooter.Name = "BasicShooter";
        Resources.AddEnemyTexture(BasicShooter);

        Texture2D DeathSpawner = Content.Load<Texture2D>("Enemies/DeathSpawner");
        DeathSpawner.Name = "DeathSpawner";
        Resources.AddEnemyTexture(DeathSpawner);

        Texture2D Spawn = Content.Load<Texture2D>("Enemies/Spawn");
        Spawn.Name = "Spawn";
        Resources.AddEnemyTexture(Spawn);
        #endregion

        Texture2D Map = Content.Load<Texture2D>("Map");
        Map.Name = "Map";
        Resources.AddMapTexture(Map);

        Texture2D Bullet = Content.Load<Texture2D>("Bullet");
        Bullet.Name = "Bullet";
        Resources.AddBulletTexture(Bullet);

        #region GUI
        Texture2D Background = Content.Load<Texture2D>("Background");
        Background.Name = "Background";
        Resources.AddGUITexture(Background);

        Texture2D HealthBack = Content.Load<Texture2D>("HealthBack");
        HealthBack.Name = "HealthBack";
        Resources.AddGUITexture(HealthBack);

        Texture2D HealthFront = Content.Load<Texture2D>("HealthFront");
        HealthFront.Name = "HealthFront";
        Resources.AddGUITexture(HealthFront);

        Texture2D MainMenuPlayButton = Content.Load<Texture2D>("GUI/MainMenu/MainMenuPlayButton");
        MainMenuPlayButton.Name = "MainMenuPlayButton";
        Resources.AddGUITexture(MainMenuPlayButton);

        #region Paused Menu
        Texture2D PausedBackground = Content.Load<Texture2D>("GUI/PausedMenu/PausedBackground");
        PausedBackground.Name = "PausedBackground";
        Resources.AddGUITexture(PausedBackground);
        #endregion
        #endregion

        SoundEffect Laser = Content.Load<SoundEffect>("Laser");
        Laser.Name = "Laser";
        Resources.AddSound(Laser);

        Resources.TestFont = Content.Load<SpriteFont>("Font");

        //This must be started after all of the content is loaded so Drawing does not cause exceptions.
        stateManager = new StateManager();
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
        // Allows the game to exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            this.Exit();

        StateManager.Instance.CurrentGameState.Update(gameTime);

        base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin();
        StateManager.Instance.CurrentGameState.Draw(spriteBatch);

        base.Draw(gameTime);
        spriteBatch.End();
    }
}
