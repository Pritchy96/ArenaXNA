using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

    //The main manager for the game state
    class StateManager
    {
        #region Singleton class variables
        private static StateManager manager = null;
        public static StateManager Instance
        {
            get
            {
                if (manager == null)
                {
                    manager = new StateManager();
                }

                return manager;
            }
        }
        #endregion

        private BasicGameState currentGameState;

        public BasicGameState CurrentGameState
        {
            get { return currentGameState; }
            set
            {
                currentGameState.OnExit();
                currentGameState = value;
            }
        }

        public StateManager()
        {
            //we start the game is a splash screen state
            currentGameState = new MenuState();
        }

        #region Function Explanation
        //Updates Mouse and current Game State.
        #endregion
        public void Update(GameTime gameTime)
        {
            currentGameState.Update(gameTime);
        }

        #region Function Explanation
        //Draws Current Game State.
        #endregion
        public void Draw(SpriteBatch spriteBatch)
        {
            currentGameState.Draw(spriteBatch);
        }
    }
