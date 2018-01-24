using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object.ParticleSystem
{
    class ParticleManager
    {
        private List<Particle> pList = new List<Particle>();
        private GameDevice gameDevice;

        public ParticleManager(GameDevice gameDevice)
        { this.gameDevice = gameDevice; }

        public void Initialize()
        { Clear(); }

        public void Clear()
        { pList.Clear(); }

        public void Update(GameTime gameTime)
        {
            UpdateParticle(gameTime);
            RemoveParticle();
        }
        
        public void UpdateParticle(GameTime gameTime)
        {
            foreach (var p in pList)
                p.Update(gameTime);
        }

        public void RemoveParticle()
        { pList.RemoveAll((Particle p) => p.IsDead()); }
       
        public void AddParticle(Particle particle)
        { pList.Add(particle); }

        public void Draw()
        {
            foreach (var p in pList)
                p.Draw(gameDevice);
        }

        public int Count()
        {
            return pList.Count;
        }
    }
}
