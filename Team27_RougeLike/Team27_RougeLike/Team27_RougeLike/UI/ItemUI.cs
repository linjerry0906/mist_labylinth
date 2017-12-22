//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.21
// 内容  ：Itemを表示するUI
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.Scene;

namespace Team27_RougeLike.UI
{
    class ItemUI
    {
        private Renderer renderer;
        private InputState input;

        private Vector2 position;               //表示位置
        private Inventory playerItem;           //Playerのアイテム
        private List<Item> itemList;            //アイテムリスト

        private List<Button> buttons;           //ボタン
        private Item currentItem;               //選択されているアイテム
        private int itemIndex;                  //特定するための添え字

        private ItemInfoUI currentInfo;         //選択されているアイテムの表示

        private readonly int WIDTH = 150;       //ボタンの長さ
        private readonly int HEIGHT = 22;       //ボタンの高さ

        private Button equipButton;             //装備ボタン
        private Button removeButton;            //捨てるボタン

        public ItemUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            this.position = position;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            playerItem = gameManager.PlayerItem;

            buttons = new List<Button>();
            itemList = playerItem.BagList();

            InitButton();

            equipButton = new Button(position + new Vector2(450, 580), 100, 30);
            removeButton = new Button(position + new Vector2(450, 620), 100, 30);

            currentItem = null;
            itemIndex = -1;
            currentInfo = new ItemInfoUI(position + new Vector2(0, 575), gameDevice);
        }

        /// <summary>
        /// Buttonの数を更新
        /// </summary>
        private void InitButton()
        {
            buttons.Clear();
            for (int i = 0; i < itemList.Count; i++)
            {
                buttons.Add(
                    new Button(position + new Vector2(0, i * HEIGHT), WIDTH, HEIGHT));
            }
        }

        /// <summary>
        /// ボタンの更新
        /// </summary>
        public void Update()
        {
            ClickList();

            CheckInfoButton();
        }

        /// <summary>
        /// Listにクリックされたかをチェック
        /// </summary>
        private void ClickList()
        {
            if (!input.IsLeftClick())       //clickしていなかったら判定
                return;

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);
            int index = 0;
            foreach (Button b in buttons)
            {
                if (b.IsClick(mousePos))    //クリックされたかを確認
                {
                    break;
                }
                index++;
            }

            InitButton();

            if (index == buttons.Count)     //最後までなかったら
            {
                return;
            }

            itemIndex = index;
            currentItem = itemList[itemIndex];
        }

        /// <summary>
        /// Infoのボタンが押されたかをチェック
        /// </summary>
        private void CheckInfoButton()
        {
            if (!input.IsLeftClick())       //clickしていなかったら判定
                return;

            if (currentItem == null || itemIndex == -1)     //エラー対策
                return;

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);

            if (equipButton.IsClick(mousePos))      //装備、使用のボタンをチェック
            {
                if (currentItem is ConsumptionItem)     //使用アイテム
                {
                    Use();
                    return;
                }

                Equip();                                //装備アイテム
                return;
            }

            if (removeButton.IsClick(mousePos))     //捨てるボタンのチェック
            {
                Remove();
            }
        }

        /// <summary>
        /// Itemを使用　まだ未実装
        /// </summary>
        private void Use()
        {
            currentItem = null;
            itemIndex = -1;
        }

        /// <summary>
        /// 装備する
        /// </summary>
        private void Equip()
        {
            if (currentItem is ProtectionItem)
            {
                playerItem.EquipArmor(itemIndex);
            }
            else
            {
                EquipWeapon();
            }
            currentItem = null;
            itemIndex = -1;
        }

        private void EquipWeapon()
        {
            WeaponItem weapon = (WeaponItem)currentItem;
            if (weapon.GetWeaponType() == WeaponItem.WeaponType.Bow)    //弓は左手
            {
                playerItem.EquipLeftHand(itemIndex);
                return;
            }
        }

        /// <summary>
        /// アイテムを捨てる
        /// </summary>
        private void Remove()
        {
            playerItem.RemoveItem(itemIndex);
            buttons.RemoveAt(buttons.Count - 1);

            currentItem = null;
            itemIndex = -1;
        }

        /// <summary>
        /// Item関連の描画
        /// </summary>
        /// <param name="alpha">透明値</param>
        public void Draw(float alpha)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                renderer.DrawTexture("fade", position + new Vector2(0, i * HEIGHT), new Vector2(WIDTH, HEIGHT - 2), 0.3f);
                renderer.DrawString(
                    itemList[i].GetItemName(),
                    position + new Vector2(0, i * HEIGHT),
                    Color.White,
                    new Vector2(1.1f, 1.1f),
                    alpha, false, false);
            }

            DrawInfo(alpha);
        }

        /// <summary>
        /// 選択していないなら表示しない部分
        /// </summary>
        /// <param name="alpha"></param>
        private void DrawInfo(float alpha)
        {
            if (currentItem == null)
                return;

            string buttonString = "使用";

            if (currentItem is WeaponItem || currentItem is ProtectionItem)
            {
                buttonString = "装備";
            }

            renderer.DrawString(
                    buttonString,
                    new Vector2(equipButton.ButtonCenter().X, equipButton.ButtonCenter().Y),
                    Color.White,
                    new Vector2(1.0f, 1.0f),
                    alpha, true, true);

            renderer.DrawString(
                    "捨てる",
                    new Vector2(removeButton.ButtonCenter().X, removeButton.ButtonCenter().Y),
                    Color.White,
                    new Vector2(1.0f, 1.0f),
                    alpha, true, true);

            currentInfo.Draw(currentItem, alpha);
        }
    }
}
