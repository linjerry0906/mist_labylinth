using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Box;
using Team27_RougeLike.Object.ParticleSystem;
using Team27_RougeLike.Scene;
using Team27_RougeLike.UI;
using Team27_RougeLike.Map;
namespace Team27_RougeLike.Object.Character
{
    class CharacterManager
    {
        private GameDevice gameDevice;
        private DungeonUI ui;

        private List<CharacterBase> characters = new List<CharacterBase>();
        private List<Spawner> spawners = new List<Spawner>();
        private List<HitBoxBase> hitBoxs = new List<HitBoxBase>();
        private Player player;
        private EnemyLoader enemyLoader;
        private AttackLoader attackLoader;

        private Dictionary<int, EnemyBase> enemys = new Dictionary<int, EnemyBase>();

        private const int drawLength = 500;

        private MapItemManager mapItemManager;      //ステージのアイテムマネージャー
        private GameManager gameManager;            //ゲームマネージャー     リンより追加

        public CharacterManager(GameDevice gameDevice, GameManager gameManager)
        {
            this.gameDevice = gameDevice;
            enemyLoader = new EnemyLoader();
            enemyLoader.Initialize(this, gameDevice);
            attackLoader = new AttackLoader();
            attackLoader.Initialize(this);
            this.gameDevice = gameDevice;
            this.gameManager = gameManager;
        }

        public void Initialize(DungeonUI ui, MapItemManager mapItemManager)
        {
            characters.Clear();
            spawners.Clear();
            hitBoxs.Clear();
            this.ui = ui;
            this.mapItemManager = mapItemManager;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var spawner in spawners)
            {
                spawner.Update();
            }
            foreach (var c1 in characters)
            {
                if (c1 is EnemyBase)
                {
                    //敵の距離によってアップデートを分けた
                    if (NearPlayer(c1.Collision.Position) || ((EnemyBase)c1).UpdateAllRange())
                    {
                        ((EnemyBase)c1).NearUpdate(player, gameTime);
                    }
                }
                c1.Update(gameTime);
            }
            foreach (var h in hitBoxs)
            {
                foreach (var c in characters)
                {
                    //ヒットボックスのコリジョン内
                    if (h.HitCheck(c))
                    {
                        if (!h.EffectedCharacters().Contains(c))
                        {
                            //ここでcの判定確定
                            h.Effect(c);

                            //それが攻撃だった場合の判定
                            if (h is iDamageBox)
                            {
                                int damage = ((iDamageBox)h).Damage() - c.GetDiffence() <= 0 ? 1 : ((iDamageBox)h).Damage() - c.GetDiffence();
                                ui.LogUI.AddLog(c.GetName() + "に" + damage + "のダメージ");
                                Sound("damage5");
                            }
                        }
                    }

                }
                h.Update();
            }


            foreach (var c in characters)
            {
                //ここがキャラクターの死亡時です
                if (c.IsDead())
                {
                    if (c is EnemyBase)
                    {
                        ui.LogUI.AddLog(c.GetName() + "は死亡した");
                        mapItemManager.AddItemByPossibility(c.Collision.Position, 0.65f, 0.4f);     //落ちる確率65％　装備品の確率40%
                        ui.LogUI.AddLog(((EnemyBase)c).GetExp() + "の経験値を獲得");
                        if (player.GetPlayerStatus().AddExp(((EnemyBase)c).GetExp()))
                        {
                            ui.LogUI.AddLog("プレイヤーのレベルは" + player.GetPlayerStatus().GetLevel() + "に上がった");
                            ui.LogUI.AddLog("ステータスが上昇した");
                            ui.LogUI.AddLog("体力が全回復した");
                            Sound("powerup10");
                            player.GetPlayerStatus().Heal(100000);
                        }
                        gameManager.PlayerQuest.AddKill(((EnemyBase)c).GetID());
                    }
                }
            }


            characters.RemoveAll((CharacterBase c) => c.IsDead());
            hitBoxs.RemoveAll((HitBoxBase h) => h.IsEnd());
        }

        public void Draw()
        {
            foreach (var c in characters)
            {
                if (c is Player)
                {
                    c.Draw(gameDevice.Renderer);
                }
                if (c is CharacterBase && NearPlayer(c.Collision.Position) && !(c is Player))
                {
                    c.Draw(gameDevice.Renderer);
                }
            }
        }

        public void AddSpawner(Spawner spawner)
        {
            spawners.Add(spawner);
        }

        public CharacterBase AddCharacter(CharacterBase character)
        {
            characters.Add(character);
            return character;
        }

        /// <summary>
        /// 生成した敵のプレイヤー索敵範囲が莫大な数値になる
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public CharacterBase AddCharacterBossScene(CharacterBase character)
        {
            ((EnemyBase)character).InfintyRange();
            characters.Add(character);
            return character;
        }

        public void AddPlayer(Vector3 position, ParticleManager pManager, GameManager gamemanager)
        {
            player = new Player
                (
                position,
                gamemanager.PlayerInfo,
                gameDevice,
                this,
                pManager,
                gamemanager,
                ui
                );
            characters.Add(player);
        }

        public void AddHitBox(HitBoxBase hitBox)
        {
            hitBoxs.Add(hitBox);
        }

        /// <summary>
        /// プレイヤーが近くにいるかどうか
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private bool NearPlayer(Vector3 position)
        {
            //if (player == null) return false;
            //if (player.IsDead()) return false;

            return Math.Abs(position.X - player.Collision.Position.X) < drawLength && Math.Abs(position.Z - player.Collision.Position.Z) < drawLength;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public List<CharacterBase> GetCharacters()
        {
            return characters;
        }

        public Dictionary<int, EnemyBase> Enemys()
        {
            return enemyLoader.Enemys();
        }

        public List<HitBoxBase> GetHitBoxs()
        {
            return hitBoxs;
        }

        public AttackBase GetAttack(int ID)
        {
            return attackLoader.GetAttack(ID);
        }

        public bool DiedCharacters()
        {
            return LiveCharacterCnt() == 0;
        }

        public int LiveCharacterCnt()
        {
            return characters.Count;    //これで十分、全部回らなくてよい（Countは要素数変動の時のみ処理、Foreachを使わずに済む）
        }
        public void Log(string log)
        {
            ui.LogUI.AddLog(log);
        }
        public void AreaDamage(int num)
        {
            characters.ForEach(c =>
            {
                Log(c.GetName() + "に" + num + "のダメージ");
                c.TrueDamage(num);
            });
        }

        public void Sound(string name)
        {
            gameDevice.Sound.PlaySE(name);
        }
    }
}
