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
        /// 落ちているアイテムを追加
        /// </summary>
        /// <param name="position">位置</param>
        public void AddItem(Vector3 position)
        {
            Item itemInfo = itemManager.GetConsuptionitem();
            float size = 2;
            Item3D addItem = new Item3D(gameDevice, itemInfo, position + new Vector3(0, size, 0));
            items.Add(addItem);
        }

        /// <summary>
        /// 落ちている装備を追加
        /// </summary>
        /// <param name="position">位置</param>
        public void AddEquip(Vector3 position)
        {
            Item itemInfo = itemManager.GetEquipmentItem();
            float size = 2;
            Item3D addItem = new Item3D(gameDevice, itemInfo, position + new Vector3(0, size, 0));
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
                    
                    hint.Switch(true);              //当たっていれば表示
                    hint.SetMessage("Press Space to get item");     //表示するメッセージ
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
                return false;

            Item item = items[index].GetItem();
            Color color = Color.Lerp(Color.White, Color.Gold, item.GetItemRare() / 100.0f);       //色調整
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
