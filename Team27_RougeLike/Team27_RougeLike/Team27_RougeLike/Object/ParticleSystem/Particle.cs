﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object.ParticleSystem
{
    abstract class Particle
    {
        protected Vector3 position;
        protected string name;
        protected float alpha;
        protected Vector2 size;
        protected bool isDead;

        public Particle()
        {}

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameDevice gameDevice);

        public bool IsDead()
        { return isDead; }
    }
}
