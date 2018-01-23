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
using Team27_RougeLike.Object.Item.ItemEffects;

namespace Team27_RougeLike.UI
{
    class ItemUI
    {
        private Renderer renderer;
        private InputState input;
        GameManager gameManager;

        private Vector2 position;               //表示位置
        private Inventory playerItem;           //Playerのアイテム
        private List<Item> itemList;            //アイテムリスト

        private List<Button> buttons;           //ボタン
        private Item currentItem;               //選択されているアイテム
        private int itemIndex;                  //特定するための添え字

        private DungeonPopUI popUI;             //PopUi
        private Button[] popButtons;            //PopUiのボタン
        private Button[] pageButtons;           //PageButtons

        private readonly int WIDTH = 280;       //ボタンの長さ
        private readonly int HEIGHT = 42;       //ボタンの高さ

        private Button equipButton;             //装備ボタン
        private Button removeButton;            //捨てるボタン

        private static readonly int PAGE_MAX_ITEM = 10;
        private int currentPage;                //現在ページ
        private int hintIndex;                  //ヒントアイテム
        private ItemInfoUI hintInfo;            //Info表示

        private bool isClick;
        private EquipUI equipUI;                //装備欄UI

        public ItemUI(Vector2 position, EquipUI equipUI, GameManager gameManager, GameDevice gameDevice)
        {
            this.position = position;
            this.equipUI = equipUI;
            this.gameManager = gameManager;
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;
            playerItem = gameManager.PlayerItem;
            itemList = playerItem.BagList();
            isClick = false;

            currentPage = 1;

            #region Button
            buttons = new List<Button>();

            InitButton();

            equipButton = new Button(position + new Vector2(550, 580), 100, 30);
            removeButton = new Button(position + new Vector2(550, 620), 100, 30);
            #endregion

            #region ItemInfo
            currentItem = null;
            itemIndex = -1;

            hintIndex = -1;
            hintInfo = new ItemInfoUI(Vector2.Zero, gameManager, gameDevice);
            #endregion

            #region PopUI
            popUI = new DungeonPopUI(gameDevice);
            popUI.SetSize(new Vector2(300, 120));
            popUI.SetAlphaLimit(0.8f);
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
            int maxPage = (itemList.Count - 1) / PAGE_MAX_ITEM + 1;
            int buttonAmount = ButtonAmount(maxPage);

            for (int i = 0; i < buttonAmount; i++)
            {
                buttons.Add(
                    new Button(position + new Vector2(0, 30 + i * (HEIGHT + 5)), WIDTH, HEIGHT));
            }

            pageButtons = new Button[2];
            pageButtons[0] = new Button(position + new Vector2(0, 30 + 11.5f * HEIGHT), WIDTH / 2 - 70, 25);
            pageButtons[1] = new Button(position + new Vector2(WIDTH / 2 + 70, 30 + 11.5f * HEIGHT), WIDTH / 2 - 70, 25);
        }

        /// <summary>
        /// Button数を計算
        /// </summary>
        /// <param name="maxPage">現在最大ページ</param>
        /// <returns></returns>
        private int ButtonAmount(int maxPage)
        {
            if (maxPage < 2)                //ページ1しかない
            {
                return itemList.Count;
            }
            if (currentPage == maxPage)     //最後のページ
            {
                return itemList.Count % PAGE_MAX_ITEM;
            }
            return PAGE_MAX_ITEM;             //中間ページ
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            itemList = playerItem.BagList();
            InitButton();
        }

        /// <summary>
        /// Popしているメッセージボックスを閉じる
        /// </summary>
        public void SwitchOff()
        {
            popUI.PopOff();
        }

        /// <summary>
        /// ボタンの更新
        /// </summary>
        public void Update()
        {
            isClick = false;
            UpdatePopUI();
            if (popUI.IsPop())      //PopUpなら以下は更新しない
                return;

            ClickList();

            ClickPage();

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

                if (buttons.Count - 1 < 0 && currentPage > 1)
                {
                    currentPage--;
                }

                currentItem = null;
                itemIndex = -1;
                equipUI.Initialize();
                Initialize();
                return;
            }

            if (popButtons[1].IsClick(mousePos))        //右ボタンをチェック
            {
                playerItem.EquipRightHand(itemIndex);
                popUI.PopOff();

                if (buttons.Count - 1 < 0 && currentPage > 1)
                {
                    currentPage--;
                }

                currentItem = null;
                itemIndex = -1;
                equipUI.Initialize();
                Initialize();
                return;
            }
        }

        /// <summary>
        /// Listにクリックされたかをチェック
        /// </summary>
        private void ClickList()
        {
            hintIndex = -1;

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

            if (index >= buttons.Count)    //最後までなかったら
            {
                return;
            }

            hintIndex = (currentPage - 1) * PAGE_MAX_ITEM + index;
            hintInfo.Position = input.GetMousePosition() + new Vector2(35, 50);

            if (!input.IsLeftClick())       //clickしていなかったら判定
                return;

            itemIndex = (currentPage - 1) * PAGE_MAX_ITEM + index;
            currentItem = itemList[itemIndex];
            isClick = true;
        }

        /// <summary>
        /// PageButtonがクリックされたか
        /// </summary>
        private void ClickPage()
        {
            if (!input.IsLeftClick())       //clickしていなかったら判定
                return;

            int maxPage = (itemList.Count - 1) / PAGE_MAX_ITEM + 1;
            if (maxPage < 2)
                return;

            Point mousePos = new Point((int)input.GetMousePosition().X, (int)input.GetMousePosition().Y);

            if(currentPage > 1)
            {
                if (pageButtons[0].IsClick(mousePos))
                {
                    currentPage--;
                    Initialize();
                    return;
                }
            }
            if (currentPage < maxPage)
            {
                if (pageButtons[1].IsClick(mousePos))
                {
                    currentPage++;
                    Initialize();
                    return;
                }
            }
        }

        public bool IsClick()
        {
            return isClick;
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
                    if (((ConsumptionItem)currentItem).GetTypeText() == "なし")
                        return;

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
        /// 現在選択したアイテム
        /// </summary>
        /// <returns></returns>
        public Item CurrentItem()
        {
            return currentItem;
        }

        /// <summary>
        /// Itemを使用
        /// </summary>
        private void Use()
        {
            string type = ((ConsumptionItem)currentItem).GetTypeText();
            ItemEffect effect = ((ConsumptionItem)currentItem).GetItemEffect();
            if (type == "回復系")
            {
                int recovery = ((Recovery)effect).GetAmount();
                gameManager.PlayerInfo.Heal(recovery);
                playerItem.RemoveItem(itemIndex);
            }
            else if (type == "ダメージ")
            {
                int damage = ((Damage)effect).GetAmount();
                gameManager.PlayerInfo.Damage(damage);
                playerItem.RemoveItem(itemIndex);
            }
            else if (type == "矢")
            {
                playerItem.EquipArrow(itemIndex);
            }

            if (buttons.Count - 1 < 0 && currentPage > 1)
            {
                currentPage--;
            }

            equipUI.Initialize();
            Initialize();
            currentItem = null;
            itemIndex = -1;
        }

        /// <summary>
        /// InfoをNullに設定
        /// </summary>
        public void SetNull()
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

            if (buttons.Count - 1 < 0 && currentPage > 1)
            {
                currentPage--;
            }

            equipUI.Initialize();
            Initialize();
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

            if (buttons.Count - 1 <= 0 && currentPage > 1)
            {
                currentPage--;
            }

            Initialize();
            currentItem = null;
            itemIndex = -1;
        }

        /// <summary>
        /// Item関連の描画
        /// </summary>
        /// <param name="alpha">透明値</param>
        public void Draw(float alpha)
        {
            DrawItemList(alpha);        //アイテムリスト
            DrawPageButton(alpha);      //ページボタン
            DrawInfoButton(alpha);      //Infoに表示するボタン
            DrawHint(alpha);            //カーソルに合わせてヒントを表示
            DrawPopUI();                //メッセージボックス表示
        }

        /// <summary>
        /// アイテムを表示
        /// </summary>
        /// <param name="alpha">透明度</param>
        private void DrawItemList(float alpha)
        {
            int currentCount = 0, maxCount = 0;
            playerItem.BagItemCount(ref currentCount, ref maxCount);
            renderer.DrawTexture("fade", position, new Vector2(WIDTH, 22), alpha * 0.6f);
            renderer.DrawString(
                "アイテム（" + currentCount +  "/" + maxCount + "）",
                position + new Vector2(WIDTH / 2, 0), Color.White,
                new Vector2(1.1f, 1.1f), alpha * 1.5f, true);

            if (itemList.Count <= 0)
                return;

            #region List描画
            for (int i = 0; i < buttons.Count; i++)
            {
                float drawAlpha = alpha;
                Color color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                int currentItemIndex = (currentPage - 1) * PAGE_MAX_ITEM + i;

                if(currentItemIndex == hintIndex)
                {
                    drawAlpha *= 0.75f;       //Mouse Onアイテムをハイライト
                }
                else if (currentItemIndex == itemIndex)
                {
                    drawAlpha *= 0.85f;       //選択されたアイテムをハイライト
                    color = Color.Yellow;
                }
                else
                {
                    drawAlpha *= 0.6f;       //選択されてないアイテムは対比するためFadeのAlphaを下げる
                }

                Vector2 drawPos = new Vector2(buttons[i].Position().X, buttons[i].Position().Y);

                renderer.DrawTexture("fade", drawPos, buttons[i].Size(), drawAlpha);
                int index = (currentPage - 1) * PAGE_MAX_ITEM + i;
                string name = itemList[index].GetItemName();
                if (itemList[index] is WeaponItem)
                    name += " + " + ((WeaponItem)itemList[index]).GetReinforcement();
                if (itemList[index] is ProtectionItem)
                    name += " + " + ((ProtectionItem)itemList[index]).GetReinforcement();

                renderer.DrawString(
                    name,
                    drawPos + new Vector2(10, HEIGHT / 2),
                    color,
                    new Vector2(1.1f, 1.1f),
                    alpha, false, true);
            }
            #endregion
        }

        /// <summary>
        /// PageButtonを描画
        /// </summary>
        /// <param name="alpha"></param>
        private void DrawPageButton(float alpha)
        {
            int maxPage = (itemList.Count - 1) / PAGE_MAX_ITEM + 1;

            Vector2 pageInfoPos =
                new Vector2(pageButtons[0].Position().X, pageButtons[0].Position().Y) +
                new Vector2(WIDTH / 2, 0);
            renderer.DrawString(currentPage + " / " + maxPage, pageInfoPos,
                Color.Black, new Vector2(1.1f, 1.1f), alpha, true);

            if (maxPage < 2)
                return;

            if (currentPage != 1)
            {
                Vector2 drawPos = new Vector2(pageButtons[0].Position().X, pageButtons[0].Position().Y);
                renderer.DrawTexture(
                    "fade",
                    drawPos,
                    pageButtons[0].Size(), alpha * 0.6f);
                renderer.DrawString("←",
                    pageButtons[0].ButtonCenterVector(), Color.White,
                    new Vector2(1.1f, 1.1f), alpha * 1.5f,
                    true, true);
            }
            if(currentPage != maxPage)
            {
                Vector2 drawPos = new Vector2(pageButtons[1].Position().X, pageButtons[1].Position().Y);
                renderer.DrawTexture(
                    "fade",
                    drawPos,
                    pageButtons[1].Size(), alpha * 0.6f);

                renderer.DrawString("→",
                    pageButtons[1].ButtonCenterVector(), Color.White,
                    new Vector2(1.1f, 1.1f), alpha * 1.5f,
                    true, true);
            }
        }

        /// <summary>
        /// 選択していないなら表示しない部分
        /// </summary>
        /// <param name="alpha"></param>
        private void DrawInfoButton(float alpha)
        {
            if (currentItem == null)
                return;

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

            string buttonString = "使用";
            if (currentItem is WeaponItem || currentItem is ProtectionItem)
            {
                buttonString = "装備";
            }
            else
            {
                string type = ((ConsumptionItem)currentItem).GetTypeText();
                if (type == "なし")       //使用できない
                    return;
                if (type == "矢")
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

            renderer.DrawTexture("white",
                new Vector2(popButtons[0].Position().X, popButtons[0].Position().Y),
                new Vector2(100, 30), popUI.Alpha * 0.9f);
            renderer.DrawString(
                    "左手",
                    new Vector2(popButtons[0].ButtonCenter().X, popButtons[0].ButtonCenter().Y),
                    Color.Black,
                    new Vector2(1.0f, 1.0f),
                    popUI.Alpha * 3, true, true);

            renderer.DrawTexture("white",
                new Vector2(popButtons[1].Position().X, popButtons[1].Position().Y),
                new Vector2(100, 30), popUI.Alpha * 0.9f);
            renderer.DrawString(
                    "右手",
                    new Vector2(popButtons[1].ButtonCenter().X, popButtons[1].ButtonCenter().Y),
                    Color.Black,
                    new Vector2(1.0f, 1.0f),
                    popUI.Alpha * 3, true, true);
        }

        /// <summary>
        /// PopWindow動作中か
        /// </summary>
        /// <returns></returns>
        public bool IsPop()
        {
            return popUI.IsPop();
        }

        /// <summary>
        /// カーソルに合わせて詳細表示
        /// </summary>
        /// <param name="alpha"></param>
        private void DrawHint(float alpha)
        {
            if (hintIndex == -1)
                return;

            renderer.DrawTexture("fade", hintInfo.Position + new Vector2(-10, -15), 
                new Vector2(420, 100), alpha * 0.8f);

            hintInfo.Draw(
                itemList[hintIndex], alpha);
        }
    }
}
