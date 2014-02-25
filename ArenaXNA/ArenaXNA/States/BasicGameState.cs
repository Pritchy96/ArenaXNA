using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

    //The base class for every game state, contains some methods
    //that will be called by the game state manager when switching states
    public abstract class BasicGameState
    {
        //The name of the GameState this allows for debugging with OnEnter() and OnExit()
        private string name;

        //The input opject
        private Input input;

        //Used for FirstTick()
        private bool isFirstTick = true;

        public BasicGameState(string name)
        {
            this.name = name;
            input = new Input();

            //Adding input events
            input.MouseDown += MouseDown;
            input.MouseUp += MouseUp;

            input.MouseClicked += MouseClicked;
            input.MouseMoved += MouseMoved;
            input.KeyDown += KeyDown;
            input.KeyPress += KeyPress;

            OnEnter();
        }

        protected virtual void OnEnter()
        {
            Console.WriteLine("Entering: {0}", name);
        }

        public virtual void OnExit()
        {
            Console.WriteLine("Exiting: {0}", name);
        }


        //Executes the MouseClicked() method of the first component which has contains set to true,
        //Which basically returns true if the mouse is contained within it.
        public virtual void MouseClicked(int x, int y, MouseButton button)
        {

        }

        public virtual void MouseDown(int x, int y, MouseButton button)
        {

        }

        public virtual void MouseUp(int x, int y, MouseButton button)
        {

        }

        public virtual void MouseMoved(int x, int y)
        {

        }

        public virtual void KeyDown(Keys[] keys)
        {
        }

        public virtual void KeyPress(Keys[] keys)
        {
        }


        protected virtual void FirstTick(GameTime gameTime, Input input)
        {

        }


        protected Input GetInput()
        {
            return input;
        }

        //The game update method
        public virtual void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            if (isFirstTick)
            {
                isFirstTick = false;
                FirstTick(gameTime, input);
            }
        }

        //The game draw method
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }

