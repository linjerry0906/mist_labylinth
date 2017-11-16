using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    enum GenerateState
    {
        GenerateRoom
    }
    class MapGenerator
    {
        private GameDevice gameDevice;
        private List<MapRoom> rooms = new List<MapRoom>();

        public MapGenerator(GameDevice gameDevice)
        {
            rooms = new List<MapRoom>();
            this.gameDevice = gameDevice;
        }

        private Point RandomPointInCircle(float radius)
        {
            float t = (float)(2 * Math.PI * gameDevice.Random.NextDouble());
            float u = (float)(gameDevice.Random.NextDouble() + gameDevice.Random.NextDouble());
            float r = (u > 1) ? 2 - u: u;
            return new Point((int)(radius * r * Math.Cos(t)), (int)(radius * r * Math.Sin(t)));
        }

        public void Update()
        {
            if (rooms.Count < 100)
            {
                Point pos = RandomPointInCircle(5);
                rooms.Add(
                    new MapRoom(
                        rooms.Count,
                        gameDevice.Random.Next(5, 21),
                        gameDevice.Random.Next(5, 21),
                        pos.X,
                        pos.Y,
                        gameDevice));
            }
        }

        public void Draw()
        {
            foreach (MapRoom r in rooms)
            {
                r.Draw();
            }
        }
    }
}
