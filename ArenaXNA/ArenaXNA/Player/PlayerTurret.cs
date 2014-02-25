using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections;
using Microsoft.Xna.Framework.Audio;

public class PlayerTurret
{
    public Texture2D graphic = Resources.GetPlayerTextures()["PlayerTurret"];
    public SoundEffect gunSound = Resources.GetSound("Laser");

    public Vector2 position = new Vector2();
    public Vector2 firePosition = new Vector2();

    public float timeSinceShot = 0;
    public float fireDelay = 100;
    public float angle = 0;
    public float damage = 10;

    public PlayerTurret(float damage, float fireDelay, SoundEffect gunSound,
        Texture2D graphic)
    {
        this.damage = damage;
        this.fireDelay = fireDelay;
        this.gunSound = gunSound;
        this.graphic = graphic;
    }

    public virtual void Fire(IngameState state, Vector2 mousePos)
    {
        Vector2 vector = state.camera.relativeXY(new Vector2(mousePos.X, mousePos.Y)) - firePosition;
        vector.Normalize();
        state.bullets.Add(new Bullet(firePosition, vector, damage, true));
        gunSound.Play();
        timeSinceShot = 0;
    }
}

