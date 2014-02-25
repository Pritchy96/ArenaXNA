using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class Bullet
    {
        public Texture2D graphic = Resources.GetBulletTexture("Bullet");
        public Vector2 vector;
        public Vector2 position;
        public float speed = 16;
        public float damage;
        public bool belongsToPlayer;
        public float life = 5000;

        public Bullet(Vector2 position, Vector2 vector, float damage, bool belongsToPlayer)
        {
            this.position = position;
            this.vector = vector;
            this.damage = damage;
            this.belongsToPlayer = belongsToPlayer;
        }
    }
