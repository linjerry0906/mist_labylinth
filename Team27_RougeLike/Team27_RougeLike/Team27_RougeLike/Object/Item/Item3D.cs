using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;

namespace Team27_RougeLike.Object.Item
{
    class Item3D
    {
        private Vector3 size = new Vector3(1, 0.5f, 1);

        private GameDevice gameDevice;
        private Item item;
        private Cube cube;
        private CollisionSphere collision;

        public Item3D(GameDevice gameDevice, Item item, Vector3 position)
        {
            this.gameDevice = gameDevice;
            this.item = item;

            cube = new Cube(position, size, gameDevice);
            collision = new CollisionSphere(position, 0.5f);

            cube.SetColor(Color.Yellow);
        }

        public CollisionSphere Collisiton
        {
            get { return collision; }
        }

        public Item GetItem()
        {
            return item;
        }

        public void Draw()
        {
            cube.Draw();
        }
    }
}
