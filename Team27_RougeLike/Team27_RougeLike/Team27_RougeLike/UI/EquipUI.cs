//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.23
// 内容　：装備しているItemを表示するUI
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Scene;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.UI
{
    class EquipUI
    {
        private Renderer renderer;
        private InputState input;
        private Inventory playerItem;

        private Item currentItem;
        private Item[] items;

        private Vector2 position;   //位置
        private string[] parts;     //部位の文字
        private string[] equips;    //装備文字
        private Color[] colors;     //色付け

        private readonly Vector2 cellSize = new Vector2(62, 25);            //部位欄の大きさ
        private readonly Vector2 equipCellSize = new Vector2(250, 25);      //装備表示欄の大きさ

        private Button[] buttons;
        private Button removeButton;            //外すボタン
        private bool isClick = false;
        private ItemUI itemUI;

        public EquipUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            this.position = position;

            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            playerItem = gameManager.PlayerItem;

            parts = new string[7];
            parts[0] = "兜";
            parts[1] = "鎧";
            parts[2] = "籠手";
            parts[3] = "靴";
            parts[4] = "左手";
            parts[5] = "右手";
            parts[6] = "弓矢";

            colors = new Color[7];

            buttons = new Button[7];
            for (int i = 0; i < buttons.Length; i++)
            {
                Vector2 drawPos = position + new Vector2(0, i * (cellSize.Y + 5));      //描画位置
                Vector2 equipPos = drawPos + new Vector2(cellSize.X + 2, 0);            //描画位置   
                buttons[i] = new Button(equipPos, (int)equipCellSize.X, (int)equipCellSize.Y);
            }
            removeButton = new Button(new Vector2(605, 610), 100, 30);

            Initialize();
        }

        public void Initialize()
        {
            ProtectionItem[] armor = playerItem.CurrentArmor();     //装備を取得
            WeaponItem leftHand = playerItem.LeftHand();            //左手
            WeaponItem rightHand = playerItem.RightHand();          //右手
            ConsumptionItem arrow = playerItem.Arrow();
            items = new Item[7];
            for (int i = 0; i < armor.Length; i++)
            {
                items[i] = armor[i];
            }
            items[4] = leftHand;
            items[5] = rightHand;
            items[6] = arrow;

            currentItem = null;
        }

        public void SetItemUI(ItemUI ui)
        {
            this.itemUI = ui;
        }

        public void Update()
        {
            isClick = false;
            if (!input.IsLeftClick())
                return;

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);
            if (currentItem != null)
            {
                if (removeButton.IsClick(mousePos))
                {
                    RemoveEquip();
                    return;
                }
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].IsClick(mousePos))
                {
                    currentItem = items[i];
                    isClick = true;
                    return;
                }
            }
        }

        private void RemoveEquip()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                    continue;
                if (currentItem.Equals(items[i]))
                {
                    if (i < 4)
                    {
                        playerItem.RemoveArmor(i);
                        currentItem = null;
                        itemUI.Initialize();
                        return;
                    }
                    if (i == 4)
                    {
                        playerItem.RemoveLeftHand();
                        currentItem = null;
                        itemUI.Initialize();
                        return;
                    }
                    if (i == 5)
                    {
                        playerItem.RemoveRightHand();
                        currentItem = null;
                        itemUI.Initialize();
                    }
                    if (i == 6)
                    {
                        playerItem.RemoveArrow();
                        currentItem = null;
                        itemUI.Initialize();
                    }
                    return;
                }
            }
        }

        public bool IsClick()
        {
            return isClick;
        }

        public void SetNull()
        {
            currentItem = null;
        }

        public Item CurrentItem()
        {
            return currentItem;
        }

        /// <summary>
        /// 装備欄を表示
        /// </summary>
        /// <param name="alpha">透明度</param>
        public void Draw(float alpha)
        {
            SetText();      //文字設定

            for (int i = 0; i < buttons.Length; i++)
            {
                Vector2 drawPos = position + new Vector2(0, i * (cellSize.Y + 5));      //描画位置
                Vector2 center = drawPos + cellSize / 2;                                //中心部
                renderer.DrawTexture("fade", drawPos, cellSize, alpha * 0.5f);          //背景
                renderer.DrawString(
                    parts[i],       //説明欄
                    center,
                    Color.White,
                    new Vector2(1.1f, 1.1f),
                    alpha, true, true);

                Vector2 equipPos = drawPos + new Vector2(cellSize.X + 2, 0);            //描画位置       
                Vector2 equipCenter = equipPos + equipCellSize / 2;                     //中心部
                renderer.DrawTexture("fade", equipPos, equipCellSize, alpha * 0.5f);    //背景
                renderer.DrawString(
                    equips[i],      //装備欄
                    equipCenter,
                    colors[i],
                    new Vector2(1.1f, 1.1f),
                    alpha, true, true);
            }

            if (currentItem == null)
                return;

            Vector2 buttonPos = new Vector2(removeButton.Position().X, removeButton.Position().Y);
            renderer.DrawTexture("fade", buttonPos, removeButton.Size(), alpha * 0.5f);           //背景
            renderer.DrawString(
                    "はずす",
                    new Vector2(removeButton.ButtonCenter().X, removeButton.ButtonCenter().Y),
                    Color.White,
                    new Vector2(1.0f, 1.0f),
                    alpha, true, true);
        }

        /// <summary>
        /// 装備文字を設定
        /// </summary>
        private void SetText()
        {
            ProtectionItem[] armor = playerItem.CurrentArmor();     //装備を取得
            WeaponItem leftHand = playerItem.LeftHand();            //左手
            WeaponItem rightHand = playerItem.RightHand();          //右手
            ConsumptionItem arrow = playerItem.Arrow();

            equips = new string[7];                                 //装備文字初期化
            for (int i = 0; i < 4; i++)                              //防具文字を設定
            {
                colors[i] = Color.White;
                SetProtectionText(ref equips[i], armor, (ProtectionItem.ProtectionType)i);
            }

            if (leftHand == null)           //左手に武器がない場合
            {
                colors[4] = Color.White;
                EquipNull(ref equips[4]);
            }
            else
            {
                colors[4] = Color.Lerp(Color.White, Color.Gold, leftHand.GetItemRare() / 100.0f);       //レア度で色付け
                equips[4] = leftHand.GetItemName() + " + " + leftHand.GetReinforcement();
            }

            if (rightHand == null)           //右手に武器がない場合
            {
                colors[5] = Color.White;
                EquipNull(ref equips[5]);
            }
            else
            {
                colors[5] = Color.Lerp(Color.White, Color.Gold, rightHand.GetItemRare() / 100.0f);       //レア度で色付け
                equips[5] = rightHand.GetItemName() + " + " + rightHand.GetReinforcement();
            }

            if (arrow == null)                //矢を装備していない場合
            {
                colors[6] = Color.White;
                EquipNull(ref equips[6]);
            }
            else
            {
                colors[6] = Color.Lerp(Color.White, Color.Gold, arrow.GetItemRare() / 100.0f);       //レア度で色付け
                equips[6] = arrow.GetItemName() + "（" + arrow.GetStack() + "）";
            }
        }

        /// <summary>
        /// 防具の文字を設定（抜粋）
        /// </summary>
        /// <param name="text">設定する文字</param>
        /// <param name="armor">防具</param>
        /// <param name="type">部位</param>
        private void SetProtectionText(ref string text, ProtectionItem[] armor, ProtectionItem.ProtectionType type)
        {
            if (armor[(int)type] == null)
            {
                EquipNull(ref text);
            }
            else
            {
                colors[(int)type] = Color.Lerp(Color.White, Color.Gold, armor[(int)type].GetItemRare() / 100.0f);
                text = armor[(int)type].GetItemName() + " + " + armor[(int)type].GetReinforcement();
            }
        }

        /// <summary>
        /// 装備していない場合の文字
        /// </summary>
        /// <param name="text">設定する文字</param>
        private void EquipNull(ref string text)
        {
            text = "(何も装備していません)";
        }
    }
}
