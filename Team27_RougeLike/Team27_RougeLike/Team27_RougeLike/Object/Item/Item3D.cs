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
        private Vector3 size = new Vector3(2.0f, 2.0f, 2.0f);

        private GameDevice gameDevice;
        private Renderer renderer;
        private Item item;
        private Vector3 position;

        public Item3D(GameDevice gameDevice, Item item, Vector3 position)
        {
            this.gameDevice = gameDevice;
            this.item = item;

            renderer = gameDevice.Renderer;
            this.position = position;
        }

        public BoundingBox Collisiton
        {
            get { return new BoundingBox(position - size, position + size); }
        }

        public Item GetItem()
        {
            return item;
        }

        public void Draw()
        {
            renderer.DrawModel("ItemModel", position, size, new Color(1.0f, 1.0f, 0.0f));
        }
    }
}
