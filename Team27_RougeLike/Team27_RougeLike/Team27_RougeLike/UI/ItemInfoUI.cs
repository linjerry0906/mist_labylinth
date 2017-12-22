//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.21
// 内容  ：Itemの詳細を表示するUI
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.UI
{
    class ItemInfoUI
    {
        private Renderer renderer;
        private Vector2 position;
        private readonly int LINE_HEIGHT = 24;      //行の間隔
        private readonly int COLUME_WIDTH = 100;    //項目の間隔

        private string[] info;                      //詳細文字

        public ItemInfoUI(Vector2 position, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;

            this.position = position;
        }

        /// <summary>
        /// 詳細を描画
        /// </summary>
        /// <param name="item">描画アイテム</param>
        /// <param name="alpha">透明度</param>
        public void Draw(Item item, float alpha)
        {
            SetInfo(item);      //文字設定

            Color nameColor = Color.Lerp(Color.White, Color.Gold, item.GetItemRare() / 100.0f);     //レア度で名前の色を決める
            //名前
            renderer.DrawString(
                info[0], position, nameColor, new Vector2(1.2f, 1.2f),
                alpha, false, true);

            //共通情報
            for (int i = 1; i <= 4; i++)
            {
                renderer.DrawString(
                    info[i], position + new Vector2((i - 1) * COLUME_WIDTH, LINE_HEIGHT), 
                    Color.White, new Vector2(1.0f, 1.0f),
                    alpha, false, true);
            }

            //説明文
            renderer.DrawString(
                info[5], position + new Vector2(0, LINE_HEIGHT * 2), 
                Color.White, new Vector2(1.0f, 1.0f),
                alpha, false, true);

            //使用アイテムはここまで
            if (info.Length < 7)
                return;

            //攻撃
            renderer.DrawString(
                info[6], position + new Vector2(0, LINE_HEIGHT * 3),
                Color.White, new Vector2(1.0f, 1.0f),
                alpha, false, true);
            //防御
            renderer.DrawString(
                info[7], position + new Vector2(COLUME_WIDTH * 2, LINE_HEIGHT * 3),
                Color.White, new Vector2(1.0f, 1.0f),
                alpha, false, true);
        }

        /// <summary>
        /// 文字設定
        /// </summary>
        /// <param name="item">詳細を表示したいアイテム</param>
        private void SetInfo(Item item)
        {
            if (item is WeaponItem)
            {
                WeaponInfo((WeaponItem)item);
                return;
            }

            if (item is ProtectionItem)
            {
                ArmorInfo((ProtectionItem)item);
                return;
            }

            ConsumptionInfo((ConsumptionItem)item);
        }

        private void WeaponInfo(WeaponItem weapon)
        {
            info = new string[8];

            info[0] = weapon.GetItemName() + " + " + weapon.GetReinforcement();
            info[1] = "レア度 " + weapon.GetItemRare();
            info[2] = "買値 " + weapon.GetItemPrice();
            info[3] = "重量 " + weapon.GetItemWeight();
            info[4] = "タイプ " + weapon.GetWeaponType();
            info[5] = weapon.GetItemExplanation();

            info[6] = "攻撃力 " + weapon.GetPower();
            info[7] = "守備力 " + weapon.GetDefense();
        }

        private void ArmorInfo(ProtectionItem armor)
        {
            info = new string[8];

            info[0] = armor.GetItemName() + " + " + armor.GetReinforcement();
            info[1] = "レア度 " + armor.GetItemRare();
            info[2] = "買値 " + armor.GetItemPrice();
            info[3] = "重量 " + armor.GetItemWeight();
            info[4] = "タイプ " + armor.GetProtectionType();
            info[5] = armor.GetItemExplanation();

            info[6] = "攻撃力 " + armor.GetPower();
            info[7] = "守備力 " + armor.GetDefense();
        }

        private void ConsumptionInfo(ConsumptionItem item)
        {
            info = new string[6];

            info[0] = item.GetItemName();
            info[1] = "レア度 " + item.GetItemRare();
            info[2] = "買値 " + item.GetItemPrice();
            info[3] = "重量 " + item.GetItemWeight();
            info[4] = "タイプ " + item.GetTypeText();
            info[5] = item.GetItemExplanation();
        }
    }
}
