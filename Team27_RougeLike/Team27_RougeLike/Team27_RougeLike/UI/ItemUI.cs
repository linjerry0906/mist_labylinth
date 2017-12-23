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
        private DungeonPopUI popUI;             //PopUi
        private Button[] popButtons;            //PopUiのボタン

        private EquipUI equipUI;

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
            itemList = playerItem.BagList();

            #region Button
            buttons = new List<Button>();

            InitButton();

            equipButton = new Button(position + new Vector2(450, 590), 100, 30);
            removeButton = new Button(position + new Vector2(450, 630), 100, 30);
            #endregion

            #region ItemInfo
            currentItem = null;
            itemIndex = -1;
            currentInfo = new ItemInfoUI(position + new Vector2(0, 575), gameDevice);
            #endregion

            #region EquipUI
            equipUI = new EquipUI(
                position + new Vector2(660, 485),
                gameManager, gameDevice);
            #endregion

            #region PopUI
            popUI = new DungeonPopUI(gameDevice);
            popUI.SetSize(new Vector2(300, 120));
            popButtons = new Button[2];
            popButtons[0] = new Button(popUI.Center + new Vector2(-130, 10), 100, 30);
            popButtons[1] = new Button(popUI.Center + new Vector2(20, 10), 100, 30);
            #endregion
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

        public void SwitchOff()
        {
            popUI.PopOff();
        }

        /// <summary>
        /// ボタンの更新
        /// </summary>
        public void Update()
        {
            UpdatePopUI();
            if (popUI.IsPop())      //PopUpなら以下は更新しない
                return;

            ClickList();

            CheckInfoButton();
        }

        /// <summary>
        /// PopUIの更新
        /// </summary>
        private void UpdatePopUI()
        {
            if (!popUI.IsPop())     //PopOffの状態は更新しない
                return;

            popUI.Update();         //透明度更新

            if (!input.IsLeftClick())   //クリックされなかったら判定しない
                return;

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);
            if (popButtons[0].IsClick(mousePos))        //左ボタンをチェック
            {
                playerItem.EquipLeftHand(itemIndex);
                popUI.PopOff();
                currentItem = null;
                itemIndex = -1;
                return;
            }

            if (popButtons[1].IsClick(mousePos))        //右ボタンをチェック
            {
                playerItem.EquipRightHand(itemIndex);
                popUI.PopOff();
                currentItem = null;
                itemIndex = -1;
                return;
            }
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

            if (index == buttons.Count || index >= itemList.Count)     //最後までなかったら
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
                if (!EquipWeapon())
                    return;
            }
            currentItem = null;
            itemIndex = -1;
        }

        /// <summary>
        /// 武器を装備する
        /// </summary>
        /// <returns></returns>
        private bool EquipWeapon()
        {
            WeaponItem weapon = (WeaponItem)currentItem;
            if (weapon.GetWeaponType() == WeaponItem.WeaponType.Bow)    //弓は左手
            {
                playerItem.EquipLeftHand(itemIndex);
                return true;
            }

            popUI.SetMessage("どちらに装備しますか?");
            popUI.PopUp();
            return false;
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
            DrawItemList(alpha);

            DrawInfo(alpha);

            equipUI.Draw(alpha);

            DrawPopUI();
        }

        /// <summary>
        /// アイテムを表示
        /// </summary>
        /// <param name="alpha">透明度</param>
        private void DrawItemList(float alpha)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                float drawAlpha = alpha;
                Color color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                if (i == itemIndex)
                {
                    drawAlpha *= 2.0f;       //選択されたアイテムをハイライト
                    color = Color.Yellow;
                }
                else
                {
                    drawAlpha *= 0.4f;       //選択されてないアイテムは対比するためFadeのAlphaを下げる
                }

                renderer.DrawTexture("fade", position + new Vector2(0, i * HEIGHT), new Vector2(WIDTH, HEIGHT - 2), drawAlpha);
                renderer.DrawString(
                    itemList[i].GetItemName(),
                    position + new Vector2(0, i * HEIGHT),
                    color,
                    new Vector2(1.1f, 1.1f),
                    alpha, false, false);
            }
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

            renderer.DrawTexture(
                "fade",
                new Vector2(equipButton.Position().X, equipButton.Position().Y),
                equipButton.Size(), alpha * 0.5f);
            renderer.DrawString(
                    buttonString,
                    new Vector2(equipButton.ButtonCenter().X, equipButton.ButtonCenter().Y),
                    Color.White,
                    new Vector2(1.0f, 1.0f),
                    alpha, true, true);

            renderer.DrawTexture(
                "fade",
                new Vector2(removeButton.Position().X, removeButton.Position().Y),
                removeButton.Size(), alpha * 0.5f);
            renderer.DrawString(
                    "捨てる",
                    new Vector2(removeButton.ButtonCenter().X, removeButton.ButtonCenter().Y),
                    Color.White,
                    new Vector2(1.0f, 1.0f),
                    alpha, true, true);

            currentInfo.Draw(currentItem, alpha);
        }

        /// <summary>
        /// PopUiを描画
        /// </summary>
        /// <param name="alpha">透明度</param>
        private void DrawPopUI()
        {
            if (!popUI.IsPop())
                return;

            popUI.Draw();

            renderer.DrawTexture("fade",
                new Vector2(popButtons[0].Position().X, popButtons[0].Position().Y),
                new Vector2(100, 30), popUI.Alpha * 2);
            renderer.DrawString(
                    "左手",
                    new Vector2(popButtons[0].ButtonCenter().X, popButtons[0].ButtonCenter().Y),
                    Color.White,
                    new Vector2(1.0f, 1.0f),
                    popUI.Alpha * 2, true, true);

            renderer.DrawTexture("fade",
                new Vector2(popButtons[1].Position().X, popButtons[1].Position().Y),
                new Vector2(100, 30), popUI.Alpha * 2);
            renderer.DrawString(
                    "右手",
                    new Vector2(popButtons[1].ButtonCenter().X, popButtons[1].ButtonCenter().Y),
                    Color.White,
                    new Vector2(1.0f, 1.0f),
                    popUI.Alpha * 2, true, true);
        }
    }
}
