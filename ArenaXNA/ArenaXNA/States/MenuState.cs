using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MenuState : BasicGameState
{

    Rectangle playButton;
    Texture2D playButtonTexture = Resources.GetGUITexture("MainMenuPlayButton");

    public MenuState()
        : base("Menu")
    {

    }

    public override void Update(GameTime gameTime)
    {

        base.Update(gameTime);
    }

    public override void MouseClicked(int x, int y, MouseButton button)
    {
        if (playButton.Contains(new Point(x, y)) && button == MouseButton.Left)
        {
            StateManager.Instance.CurrentGameState = new IngameState();
        } 
    }

    public override void KeyDown(Keys[] keys)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {

        spriteBatch.Draw(Resources.GetGUITexture("Background"), new Rectangle(0, 0, GameClass.Game_Width,
            GameClass.Game_Height), Color.White);


         playButton = new Rectangle(292, 89, 196, 95);

    }
}
