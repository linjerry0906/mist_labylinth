using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Device;
using Team27_RougeLike.UI;

namespace Team27_RougeLike.Object.Item
{
    class ItemButton
    {
        private GameDevice gameDevice;
        private Renderer renderer;
        private InputState input;
        
        private Window window;
        private Vector2 position;
        private Rectangle rect;
        private Item item;
        private bool isClick;

        public ItemButton(GameDevice gameDevice ,Vector2 position, Rectangle rect, Item item)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;

            this.position = position;
            this.rect = rect;
            this.item = item;

            window = new Window(gameDevice, position, new Vector2(rect.X, rect.Y));

            isClick = false;
        }

        //Rectangleを必要としない
        public ItemButton(GameDevice gameDevice, Vector2 position, Item item)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            renderer = gameDevice.Renderer;

            this.position = position;
            rect = new Rectangle((int)position.X, (int)position.Y, 1080 / 2 - 128, 20);
            this.item = item;

            window = new Window(gameDevice, position, new Vector2(rect.Width, rect.Height));

            isClick = false;
        }

        public void Initialize()
        {
            isClick = false;
            window.Switch();
        }

        public void Update(Point mousePos)
        {
            window.Update();

            if (rect.Contains(mousePos) && input.IsLeftClick())
            {
                isClick = true;
                return;
            }
            isClick = false;
        }

        public void DrawButton()
        {
            window.Draw();

            renderer.DrawString(item.GetItemName(),position, new Vector2(1, 1), Color.White);
            renderer.DrawString(item.GetItemPrice().ToString(),position + new Vector2(160, 0), new Vector2(1, 1), Color.White);
            string type;
            if (item is WeaponItem)
            {
                type = ((WeaponItem)item).GetWeaponType().ToString();
            }
            else if (item is ProtectionItem)
            {
                type = ((ProtectionItem)item).GetProtectionType().ToString();
            }
            else
            {
                type = ((ConsumptionItem)item).GetTypeText();
            }
            renderer.DrawString(type, position + new Vector2(256, 0), new Vector2(1, 1), Color.White);
        }

        public Item GetItem()
        {
            return item;
        }

        public bool IsClick()
        {
            return isClick;
        }
    }
}
