//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.12.14
// 内容  ：ダンジョン内のアイテム管理者
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object;
using Team27_RougeLike.UI;
using Team27_RougeLike.Scene;

namespace Team27_RougeLike.Map
{
    class MapItemManager
    {
        private List<Item3D> items;         //落ちているアイテム
        private GameDevice gameDevice;
        private ItemManager itemManager;    //アイテムデータベース
        private Inventory playerItem;

        public MapItemManager(GameManager gameManager, GameDevice gameDevice)
        {
            itemManager = gameManager.ItemManager;
            playerItem = gameManager.PlayerItem;
            this.gameDevice = gameDevice;

            items = new List<Item3D>();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            items.Clear();
        }

        /// <summary>
        /// 確率でアイテムを追加
        /// </summary>
        /// <param name="position">場所</param>
        /// <param name="general">全体の確率（0.0～1.0）</param>
        /// <param name="equip">装備品の確率（0.0～1.0）</param>
        public void AddItemByPossibility(Vector3 position, float general, float equip)
        {
            float possiblility = gameDevice.Random.Next(0, 1001) / 1000.0f;
            if (possiblility > general)     //全体の確率より大きい場合は追加しない
                return;

            possiblility = gameDevice.Random.Next(0, 1001) / 1000.0f;
            if (possiblility < equip)
            {
                AddEquip(position);
                return;
            }

            AddItem(position);
        }

        /// <summary>
        /// 落ちているアイテムを追加
        /// </summary>
        /// <param name="position">位置</param>
        public void AddItem(Vector3 position)
        {
            Item itemInfo = itemManager.GetConsuptionitem();
            if (itemInfo == null)
                return;

            position.Y = MapDef.TILE_SIZE / 2 + Item3D.GetHeight();
            Item3D addItem = new Item3D(gameDevice, itemInfo, position);
            items.Add(addItem);
        }

        /// <summary>
        /// 落ちている装備を追加
        /// </summary>
        /// <param name="position">位置</param>
        public void AddEquip(Vector3 position)
        {
            Item itemInfo = itemManager.GetEquipmentItem();
            if (itemInfo == null)
                return;

            position.Y = MapDef.TILE_SIZE / 2 + Item3D.GetHeight();
            Item3D addItem = new Item3D(gameDevice, itemInfo, position);
            items.Add(addItem);
        }

        /// <summary>
        /// 表示するために描画する
        /// </summary>
        public void Draw()
        {
            items.ForEach(i => i.Draw());
        }

        /// <summary>
        /// Playerとアイテムのあたり判定
        /// </summary>
        /// <param name="chara">Player</param>
        /// <param name="ui">UI表示用（）Debug</param>
        public void ItemCollision(CharacterBase chara, DungeonUI ui)
        {
            int index = 0;
            DungeonHintUI hint = ui.HintUI;
            hint.Switch(false);                     //UIを非表示

            items.ForEach(i => 
            {
                if (chara.Collision.IsCollision(i.Collisiton))
                {
                    string itemName = i.GetItem().GetItemName();
                    hint.Switch(true);              //当たっていれば表示
                    hint.SetMessage("拾う：Space " + itemName);     //表示するメッセージ
                    bool result = false;
                    if (hint.IsPush(Keys.Space))    //拾ったらもらう処理
                    {
                        result = GetItem(index, ui);
                    }
                    if (result)                     //道具欄に追加成功したらメッセージをOFF
                    {
                        hint.Switch(false);
                    }
                    return;
                }
                index++;
            });
        }

        /// <summary>
        /// Itemをもらう処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool GetItem(int index, DungeonUI ui)
        {
            if (!playerItem.AddTempItem(items[index].GetItem()))
            {
                ui.LogUI.AddLog("カバンのアイテムがいっぱい", Color.Red);                       //Logに追加
                return false;
            }

            Item item = items[index].GetItem();
            Color color = Color.Lerp(Color.White, Color.Gold, item.GetItemRare() / 8.0f);       //色調整
            string name = item.GetItemName();
            if (item is WeaponItem)
                name += " + " + ((WeaponItem)item).GetReinforcement();
            if (item is ProtectionItem)
                name += " + " + ((ProtectionItem)item).GetReinforcement();
            ui.LogUI.AddLog(item.GetItemName() + " を取得した", color);                           //Logに追加
            items.RemoveAt(index);
            return true;
        }
    }
}
