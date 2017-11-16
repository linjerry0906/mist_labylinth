using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Map
{
    class MapGenerator
    {
        private enum GenerateState
        {
            GenerateRoom,
            Discrete,
            SelectMainRoom,
        }
        private readonly int MAX_ROOM_SIZE = 16;
        private readonly int MIN_ROOM_SIZE = 2;
        private int limitHeight;
        private int limitWidth;

        private GameDevice gameDevice;
        private List<MapRoom> rooms;
        private List<MapRoom> mainRoom;

        private GenerateState currentState;

        public MapGenerator(GameDevice gameDevice)
        {
            rooms = new List<MapRoom>();
            mainRoom = new List<MapRoom>();
            this.gameDevice = gameDevice;
            limitWidth = gameDevice.Random.Next(50, 151);
            limitHeight = 200 - limitWidth;

            currentState = GenerateState.GenerateRoom;
        }

        /// <summary>
        /// 正規分布
        /// </summary>
        /// <param name="radius">半径</param>
        /// <returns></returns>
        private Point RandomPointInCircle(float width, float height)
        {
            float t = (float)(2 * Math.PI * gameDevice.Random.NextDouble());
            float u = (float)(gameDevice.Random.NextDouble() + gameDevice.Random.NextDouble());
            float r = (u > 1) ? 2 - u: u;
            return new Point((int)(width * r * Math.Cos(t)), (int)(height * r * Math.Sin(t)));
        }

        public void Update()
        {
            switch (currentState)
            {
                case GenerateState.GenerateRoom:
                    UpdateGenerate();
                    break;
                case GenerateState.Discrete:
                    UpdateDiscrete();
                    break;
                case GenerateState.SelectMainRoom:
                    UpdateSelectMainRoom();
                    break;
            }
        }

        private void UpdateGenerate()
        {
            if (rooms.Count < 151)
            {
                Point pos = RandomPointInCircle(limitWidth, limitHeight);
                rooms.Add(
                    new MapRoom(
                        rooms.Count,
                        gameDevice.Random.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE) * 2,
                        gameDevice.Random.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE) * 2,
                        pos.X,
                        pos.Y,
                        gameDevice));
            }
            else
            {
                currentState = GenerateState.Discrete;
            }
        }

        private void UpdateDiscrete()
        {
            int Counter = 0;
            foreach (MapRoom r1 in rooms)
            {
                foreach (MapRoom r2 in rooms)
                {
                    if (r1 == r2 || !r1.RoomCollision(r2))
                        continue;
                    Counter++;
                    r1.Hit(r2);
                    r2.Hit(r1);
                }
            }
            if (Counter <= 0)
            {
                currentState = GenerateState.SelectMainRoom;
            }
        }

        private void UpdateSelectMainRoom()
        {
            foreach (MapRoom r in rooms)
            {
                if(r.Length > (int)(MAX_ROOM_SIZE * 2 * 0.7f) &&
                   r.Width > (int)(MAX_ROOM_SIZE * 2 * 0.7f))
                {
                    r.SetColor(Color.Red);
                    mainRoom.Add(r);
                }
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
