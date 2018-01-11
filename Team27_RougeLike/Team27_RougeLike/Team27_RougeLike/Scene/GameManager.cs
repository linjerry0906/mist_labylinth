//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29 ～ 2017.12.06
// 内容  ：シーンの間にゲーム情報を伝えるクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Device;
using Team27_RougeLike.Map;
using Team27_RougeLike.Object;
using Team27_RougeLike.Object.Item;

namespace Team27_RougeLike.Scene
{
    class GameManager
    {
        private PlayerStatus playerStatus;  //Playerのステータス
        private Inventory playerItem;       //Playerが持つアイテム

        private DungeonProcess dungeonProcess; //進捗状況
        private ItemManager itemManager;    //Item Dictionary

        private GameDevice gameDevice;

        private DungeonMap mapInstance;     //マップの実体
        private StageManager stageManager;  //ステージマネージャー
        private int stageItemFile;          //ステージのファイル番号

        /// <summary>
        /// シーンの間にゲーム情報を伝える仲介者
        /// </summary>
        /// <param name="gameDevice">ゲームディバイス</param>
        public GameManager(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            mapInstance = null;

            stageManager = new StageManager(gameDevice);
            itemManager = new ItemManager();
            dungeonProcess = new DungeonProcess();

            PlayerStatusLoader psLoader = new PlayerStatusLoader();       //Todo:Saveから読み取る（PassiveSkillがあるため）
            int[] status = psLoader.LoadStatus();
            Status defaultStatus = new Status(1, status[0], status[1], status[2], status[3], 1);
            playerStatus = new PlayerStatus(defaultStatus, gameDevice);
            playerStatus.Initialize();

            playerItem = playerStatus.GetInventory();                     //道具欄を取得

            Load();
        }

        #region Save関連

        /// <summary>
        /// 再開で前のセーブを読み取る処理
        /// </summary>
        private void Load()
        {
            SaveData saveData = new SaveData(this);

            if (!saveData.Load())                //失敗の場合
            {
                InitNewData();
                return;
            }

            playerItem.LoadFromFile(saveData);   //Playerアイテム復元
        }

        /// <summary>
        /// 新しいセーブデータを作る
        /// </summary>
        private void InitNewData()
        {
            SaveData saveData = new SaveData(this);
            saveData.Save();
        }

        /// <summary>
        /// セーブ
        /// </summary>
        public void Save()
        {
            SaveData saveData = new SaveData(this);
            saveData.Save();
        }

        #endregion

        #region Player関連

        /// <summary>
        /// Playerの現在情報を取得
        /// </summary>
        public PlayerStatus PlayerInfo
        {
            get { return playerStatus; }
        }

        #endregion

        #region Item関連

        /// <summary>
        /// ItemDictionary
        /// </summary>
        public ItemManager ItemManager
        {
            get { return itemManager; }
        }

        /// <summary>
        /// Playerの道具欄
        /// </summary>
        public Inventory PlayerItem
        {
            get { return playerItem; }
        }

        public int StageItemNum
        {
            get { return stageItemFile; }
            set { stageItemFile = value; }
        }

        #endregion

        #region Stage関連

        /// <summary>
        /// Stageの生成情報を設定
        /// </summary>
        /// <param name="limitSecond">攻略の制限時間</param>
        /// <param name="floor">階層目</param>
        /// <param name="totalFloor">総階層</param>
        /// <param name="bossRange">Boss出る階層</param>
        /// <param name="stageSize">ダンジョンのサイズ</param>
        public void InitStage(int dungeonNum, string dungeonName,
            int limitSecond, int floor, int totalFloor, int bossRange,int stageSize)
        {
            stageManager.Initialize(dungeonNum ,dungeonName, limitSecond, floor, totalFloor, bossRange,stageSize);
        }
        
        /// <summary>
        /// Stage管理者
        /// </summary>
        public StageManager StageManager
        {
            get { return stageManager; }
        }

        /// <summary>
        /// 進捗状況更新
        /// </summary>
        public void UpdateDungeonProcess()
        {
            dungeonProcess.UpdateProcess(stageManager.CurrentDungeonNum(), stageManager.CurrentFloor());
        }

        public DungeonProcess DungeonProcess
        {
            get { return dungeonProcess; }
        }

        #endregion

        #region Map関連
        /// <summary>
        /// マップの実体を生成
        /// </summary>
        /// <param name="mapChip">マップチップ</param>
        public void GenerateMapInstance(int[,] mapChip)
        {
            mapInstance = new DungeonMap(mapChip ,gameDevice);
        }

        /// <summary>
        /// マップを解放
        /// </summary>
        public void ReleaseMap()
        {
            mapInstance = null;
        }

        /// <summary>
        /// マップのインスタンス
        /// </summary>
        /// <returns></returns>
        public DungeonMap GetDungeonMap()
        {
            return mapInstance;
        }

        #endregion
    }
}
