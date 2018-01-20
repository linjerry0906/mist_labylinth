//--------------------------------------------------------------------------------------------------
// 作成者：林　佳叡
// 作成日：2017.11.29 ～ 2017.12.06
// 内容  ：シーンの間にゲーム情報を伝えるクラス
//--------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Device;
using Team27_RougeLike.Map;
using Team27_RougeLike.Object;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.QuestSystem;

namespace Team27_RougeLike.Scene
{
    class GameManager
    {
        private PlayerStatus playerStatus;                   //Playerのステータス
        private Inventory playerItem;                        //Playerが持つアイテム
        private PlayerQuest playerQuest;                     //Playerが受けているクエスト
        private PlayerGuildRank playerGuildRank;             //Playerのギルトレベル

        private DungeonProcess dungeonProcess;               //進捗状況
        private ItemManager itemManager;                     //Item Dictionary
        private QuestLoader questManager;                    //Quest
        private EnemyNameLoader enemyName;                   //EnemyName

        private GameDevice gameDevice;

        private BlockStyle blockStyle;                       //Blockの種類
        private DungeonMap mapInstance;                      //マップの実体
        private StageManager stageManager;                   //ステージマネージャー
        private EnemySettingManager enemySettingManager;     //Enemy配置管理者
        private int stageNumFile;                            //ステージのファイル番号

        /// <summary>
        /// シーンの間にゲーム情報を伝える仲介者
        /// </summary>
        /// <param name="gameDevice">ゲームディバイス</param>
        public GameManager(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            mapInstance = null;

            stageManager = new StageManager(gameDevice);
            enemySettingManager = new EnemySettingManager(gameDevice);
            blockStyle = new BlockStyle();
            questManager = new QuestLoader();
            questManager.Initialize();
            questManager.Load(dungeonProcess, true);
            itemManager = new ItemManager();
            dungeonProcess = new DungeonProcess();
            enemyName = new EnemyNameLoader();

            #region Player初期化
            PlayerStatusLoader psLoader = new PlayerStatusLoader();
            int[] status = psLoader.LoadStatus();
            Status defaultStatus = new Status(1, status[0], status[1], status[2], status[3], 1);
            playerStatus = new PlayerStatus(defaultStatus, gameDevice);
            playerStatus.Initialize();

            playerItem = playerStatus.GetInventory();                     //道具欄を取得
            playerQuest = new PlayerQuest();
            playerGuildRank = new PlayerGuildRank();
            #endregion

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

            playerItem.LoadFromFile(saveData);       //Playerアイテム復元
            dungeonProcess.LoadSaveData(saveData);   //攻略情報復元
            playerQuest.LoadFromSave(saveData);      //Quest情報復元
            playerGuildRank.LoadSaveData(saveData);
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


        #endregion

        #region Quest関連
        public PlayerQuest PlayerQuest
        {
            get { return playerQuest; }
        }

        public QuestLoader QuestManager
        {
            get { return questManager; }
        }

        public EnemyNameLoader EnemyName
        {
            get { return enemyName; }
        }

        public PlayerGuildRank GuildInfo
        {
            get { return playerGuildRank; }
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
        /// <param name="expandRate">拡大の比率</param>
        public void InitStage(int dungeonNum, string dungeonName,
            int limitSecond, int floor, int totalFloor, int bossRange, int stageSize, int expandRate,
            Vector3 fogColor, Vector3 constractColor, bool isParicle, string bgmName)
        {
            stageManager.Initialize(
                dungeonNum, dungeonName, limitSecond,
                floor, totalFloor, bossRange,
                stageSize, expandRate, fogColor, 
                constractColor, isParicle, bgmName);
        }

        /// <summary>
        /// Block定義をクリア
        /// </summary>
        public void ClearBlockStyle()
        {
            blockStyle.Clear();
        }


        /// <summary>
        /// Blockのテクスチャーを定義するクラス
        /// </summary>
        public BlockStyle BlockStyle
        {
            get { return blockStyle; }
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

        /// <summary>
        /// 攻略進捗
        /// </summary>
        public DungeonProcess DungeonProcess
        {
            get { return dungeonProcess; }
        }

        /// <summary>
        /// ステージ使用するアイテム情報のファイル番号
        /// </summary>
        public int StageNum
        {
            get { return stageNumFile; }
            set { stageNumFile = value; }
        }

        /// <summary>
        /// Enemy配置
        /// </summary>
        public EnemySettingManager EnemySetting
        {
            get { return enemySettingManager; }
        }

        #endregion

        #region Map関連
        /// <summary>
        /// マップの実体を生成
        /// </summary>
        /// <param name="mapChip">マップチップ</param>
        public void GenerateMapInstance(int[,] mapChip)
        {
            mapInstance = new DungeonMap(mapChip, gameDevice);
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
