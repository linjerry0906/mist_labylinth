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
        private Inventory playerItem;

        private Vector2 position;   //位置
        private string[] parts;     //部位の文字
        private string[] equips;    //装備文字
        private Color[] colors;     //色付け

        private readonly Vector2 cellSize = new Vector2(62, 25);            //部位欄の大きさ
        private readonly Vector2 equipCellSize = new Vector2(250, 25);      //装備表示欄の大きさ

        public EquipUI(Vector2 position, GameManager gameManager, GameDevice gameDevice)
        {
            this.position = position;

            renderer = gameDevice.Renderer;
            playerItem = gameManager.PlayerItem;

            parts = new string[6];
            parts[0] = "兜";
            parts[1] = "鎧";
            parts[2] = "籠手";
            parts[3] = "靴";
            parts[4] = "左手";
            parts[5] = "右手";

            colors = new Color[6];
        }

        /// <summary>
        /// 装備欄を表示
        /// </summary>
        /// <param name="alpha">透明度</param>
        public void Draw(float alpha)
        {
            SetText();      //文字設定

            for (int i = 0; i < 6; i++)
            {
                Vector2 drawPos = position + new Vector2(0, i * (cellSize.Y + 5));      //描画位置
                Vector2 center = drawPos + cellSize / 2;                                //中心部
                renderer.DrawTexture("fade", drawPos, cellSize, alpha);                 //背景
                renderer.DrawString(
                    parts[i],       //説明欄
                    center,
                    Color.White,
                    new Vector2(1.1f, 1.1f),
                    alpha, true, true);

                Vector2 equipPos = drawPos + new Vector2(cellSize.X + 2, 0);            //描画位置       
                Vector2 equipCenter = equipPos + equipCellSize / 2;                     //中心部
                renderer.DrawTexture("fade", equipPos, equipCellSize, alpha);           //背景
                renderer.DrawString(
                    equips[i],      //装備欄
                    equipCenter,
                    colors[i],
                    new Vector2(1.1f, 1.1f),
                    alpha, true, true);
            }
        }

        /// <summary>
        /// 装備文字を設定
        /// </summary>
        private void SetText()
        {
            ProtectionItem[] armor = playerItem.CurrentArmor();     //装備を取得
            WeaponItem leftHand = playerItem.LeftHand();            //左手
            WeaponItem rightHand = playerItem.RightHand();          //右手

            equips = new string[6];                                 //装備文字初期化
            for(int i = 0; i < 4; i++)                              //防具文字を設定
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
