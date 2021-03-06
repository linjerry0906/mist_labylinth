﻿//--------------------------------------------------------------------------------------------------
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
using Team27_RougeLike.Scene;

namespace Team27_RougeLike.UI
{
    class ItemInfoUI
    {
        private Inventory playerItem;
        private Renderer renderer;
        private Vector2 position;

        private readonly int LINE_HEIGHT = 24;      //行の間隔
        private readonly int COLUME_WIDTH = 100;    //項目の間隔

        private string[] info;                      //詳細文字
        private Color[] colors;

        public ItemInfoUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            renderer = gameDevice.Renderer;
            playerItem = gameManager.PlayerItem;

            this.position = position;
            colors = new Color[2];
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// 詳細を描画
        /// </summary>
        /// <param name="item">描画アイテム</param>
        /// <param name="alpha">透明度</param>
        public void Draw(Item item, float alpha)
        {
            SetInfo(item);      //文字設定

            Color nameColor = Color.Lerp(Color.White, Color.Gold, item.GetItemRare() / 8.0f);     //レア度で名前の色を決める

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
                info[5], position + new Vector2(0, LINE_HEIGHT * 1.5f),
                Color.White, new Vector2(1.0f, 1.0f),
                alpha, false, false);

            //使用アイテムはここまで
            if (info.Length < 7)
                return;

            //攻撃
            renderer.DrawString(
                info[6], position + new Vector2(0, LINE_HEIGHT * 3),
                colors[0], new Vector2(1.0f, 1.0f),
                alpha, false, true);
            //防御
            renderer.DrawString(
                info[7], position + new Vector2(COLUME_WIDTH * 2, LINE_HEIGHT * 3),
                colors[1], new Vector2(1.0f, 1.0f),
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

            if (item is AccessaryItem)
            {
                AccessaryInfo((AccessaryItem)item);
                return;
            }

            ConsumptionInfo((ConsumptionItem)item);
        }

        /// <summary>
        /// 武器の詳細設定
        /// </summary>
        /// <param name="weapon">武器</param>
        private void WeaponInfo(WeaponItem weapon)
        {
            info = new string[8];
            WeaponItem currentWeapon = playerItem.LeftHand();
            int[] diff = new int[2];            //現在装備している武器（左手）との差を取得
            if (currentWeapon != null)
            {
                diff[0] = weapon.GetPower() - currentWeapon.GetPower();
                diff[1] = weapon.GetDefense() - currentWeapon.GetDefense();
            }
            else
            {
                diff[0] = weapon.GetPower();
                diff[1] = weapon.GetDefense();
            }
            colors[0] = GetColor(diff[0]);      //色付け
            colors[1] = GetColor(diff[1]);      //色付け

            info[0] = weapon.GetItemName() + " + " + weapon.GetReinforcement();
            info[1] = "レア度 " + weapon.GetItemRare();
            info[2] = "買値 " + weapon.GetItemPrice();
            info[3] = "重量 " + weapon.GetItemWeight();
            info[4] = "タイプ " + weapon.GetWeaponType();
            info[5] = weapon.GetItemExplanation();

            info[6] = "攻撃力 " + weapon.GetPower() + "(" + diff[0] + ")";
            info[7] = "守備力 " + weapon.GetDefense() + "(" + diff[1] + ")";
        }

        /// <summary>
        /// 防具の詳細設定
        /// </summary>
        /// <param name="armor">防具</param>
        private void ArmorInfo(ProtectionItem armor)
        {
            info = new string[8];
            ProtectionItem currentArmor = playerItem.CurrentArmor()[(int)armor.GetProtectionType() - (int)(ProtectionItem.ProtectionType.Helm)];
            int[] diff = new int[2];            //現在装備しているアーマーとの差を取得
            if (currentArmor != null)
            {
                diff[0] = armor.GetPower() - currentArmor.GetPower();
                diff[1] = armor.GetDefense() - currentArmor.GetDefense();
            }
            else
            {
                diff[0] = armor.GetPower();
                diff[1] = armor.GetDefense();
            }
            colors[0] = GetColor(diff[0]);      //色付け
            colors[1] = GetColor(diff[1]);      //色付け

            info[0] = armor.GetItemName() + " + " + armor.GetReinforcement();
            info[1] = "レア度 " + armor.GetItemRare();
            info[2] = "買値 " + armor.GetItemPrice();
            info[3] = "重量 " + armor.GetItemWeight();
            info[4] = "タイプ " + armor.GetProtectionType();
            info[5] = armor.GetItemExplanation();

            info[6] = "攻撃力 " + armor.GetPower() + "(" + diff[0] + ")";
            info[7] = "守備力 " + armor.GetDefense() + "(" + diff[1] + ")";
        }

        /// <summary>
        /// 差で色を付ける
        /// </summary>
        /// <param name="diff">差</param>
        /// <returns></returns>
        private Color GetColor(int diff)
        {
            if (diff > 0)
                return Color.PaleVioletRed;
            if (diff == 0)
                return Color.White;
            return Color.CornflowerBlue;
        }

        /// <summary>
        /// 使用アイテムの詳細設定
        /// </summary>
        /// <param name="item">アイテム</param>
        private void ConsumptionInfo(ConsumptionItem item)
        {
            info = new string[6];

            info[0] = item.GetItemName();
            info[1] = "レア度 " + item.GetItemRare();
            info[2] = "買値 " + item.GetItemPrice();
            info[3] = "重量 " + item.GetItemWeight();
            info[4] = "タイプ " + item.GetTypeText();
            info[5] = item.GetItemExplanation();

            if (item.GetTypeText() == "矢")
                info[0] += "（" + item.GetStack() + "）";
        }

        /// <summary>
        /// 装飾品の詳細設定
        /// </summary>
        /// <param name="item">アイテム</param>
        private void AccessaryInfo(AccessaryItem item)
        {
            info = new string[6];

            info[0] = item.GetItemName();
            info[1] = "レア度 " + item.GetItemRare();
            info[2] = "買値 " + item.GetItemPrice();
            info[3] = "重量 " + item.GetItemWeight();
            info[4] = "タイプ 装飾品";
            info[5] = item.GetItemExplanation();
        }
    }
}
