using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public static class Resources
    {
        #region Variables
        private static Dictionary<String, Texture2D> MapTextures = new Dictionary<string,Texture2D>();
        private static Dictionary<String, Texture2D> EnemyTextures = new Dictionary<string, Texture2D>();
        private static Dictionary<String, Texture2D> PlayerTextures = new Dictionary<string, Texture2D>();
        private static Dictionary<String, Texture2D> GUITextures = new Dictionary<string, Texture2D>();
        private static Dictionary<String, Texture2D> BulletTextures = new Dictionary<String, Texture2D>();
        private static Dictionary<String, SoundEffect> Sounds = new Dictionary<String, SoundEffect>();

        //TEMP: Font used for testing
        public static SpriteFont TestFont;
        #endregion

        #region Background Textures
        public static void AddMapTexture(Texture2D textureToAdd)
        {

            MapTextures.Add(textureToAdd.Name, textureToAdd);
        }

        public static Texture2D GetMapTexture(String requestedTextureName)
        {
            return MapTextures[requestedTextureName];
        }
        #endregion

        #region Unit Textures
        public static void AddEnemyTexture(Texture2D textureToAdd)
        {
            EnemyTextures.Add(textureToAdd.Name, textureToAdd);
        }

        public static Dictionary<String, Texture2D> GetEnemyTextures()
        {
            return EnemyTextures;
        }

        public static Texture2D GetEnemyTexture(String requestedTextureName)
        {
            return EnemyTextures[requestedTextureName];
        }
        #endregion

        #region Player Textures
        public static void AddPlayerTexture(Texture2D textureToAdd)
        {
            PlayerTextures.Add(textureToAdd.Name, textureToAdd);
        }

        public static Dictionary<String, Texture2D> GetPlayerTextures()
        {
            return PlayerTextures;
        }
        #endregion

        #region GUI Textures
        public static void AddGUITexture(Texture2D textureToAdd)
        {
            GUITextures.Add(textureToAdd.Name, textureToAdd);
        }

        public static Texture2D GetGUITexture(String requestedTextureName)
        {
            return GUITextures[requestedTextureName];
        }
        #endregion

        #region Bullet Textures
        public static void AddBulletTexture(Texture2D textureToAdd)
        {
            BulletTextures.Add(textureToAdd.Name, textureToAdd);
        }

        public static Texture2D GetBulletTexture(String requestedTextureName)
        {
            return BulletTextures[requestedTextureName];
        }
        #endregion

        #region Sounds
        public static void AddSound(SoundEffect Sound)
        {
            Sounds.Add(Sound.Name, Sound);
        }

        public static SoundEffect GetSound(String requestedSoundName)
        {
            return Sounds[requestedSoundName];
        }
        #endregion
        
    }