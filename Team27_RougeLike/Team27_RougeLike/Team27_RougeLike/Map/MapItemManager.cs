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
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object;
using Team27_RougeLike.UI;

namespace Team27_RougeLike.Map
{
    class MapItemManager
    {
        private List<Item3D> items;         //落ちているアイテム
        private GameDevice gameDevice;
        private ItemManager itemManager;    //アイテムデータベース


        public MapItemManager(ItemManager itemManager, GameDevice gameDevice)
        {
            this.itemManager = itemManager;
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
        public void ItemCollision(CharacterBase chara, DungeonPopUI ui)
        {
            items.ForEach(i => 
            {
                if (chara.Collision.IsCollision(i.Collisiton))
                {
                    ui.SetItemInfo(i.GetItem());                    //当たっていればUIに通知  ToDo：オブザーバーパターン
                    return;
                }
            });
        }
    }
}
