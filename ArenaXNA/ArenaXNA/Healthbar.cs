using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class Healthbar
    {
        public Vector2 offset;

        public Healthbar(Texture2D unitTexture)
        {
            offset = new Vector2((float)(-unitTexture.Width / 2), (float)(-unitTexture.Height / 1.8));
        }
    }
