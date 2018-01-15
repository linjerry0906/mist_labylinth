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
        private GameDevice gamedevice;
        private DungeonUI ui;
        private List<CharacterBase> characters = new List<CharacterBase>();
        private List<Spawner> spawners = new List<Spawner>();
        private List<HitBoxBase> hitBoxs = new List<HitBoxBase>();
        private Player player;

        private Dictionary<int, EnemyBase> enemys = new Dictionary<int, EnemyBase>();

        private string enemyFilename;
        private const int drawLength = 120;

        private MapItemManager mapItemManager;      //ステージのアイテムマネージャー

        public CharacterManager(GameDevice gamedevice)
        {
            this.gamedevice = gamedevice;

            enemyFilename = @"Content/" + "EnemysCSV/Enemy.csv";
            Load();
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
                c1.Update(gameTime);

                if (c1 is EnemyBase)
                {
                    //敵の距離によってアップデートを分けた
                    if (NearPlayer(c1.Collision.Position))
                    {
                        ((EnemyBase)c1).NearUpdate(player, gameTime);
                    }
                }
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
                                ui.LogUI.AddLog(c.Tag + "に" + ((iDamageBox)h).Damage() + "の damageChance");
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
                    ui.LogUI.AddLog(c.Tag + " is Dead");
                    if (c is EnemyBase)
                    {
                        mapItemManager.AddItemByPossibility(c.Collision.Position, 0.65f, 0.4f);     //落ちる確率65％　装備品の確率40%
                        player.GetPlayerStatus().AddExp(((EnemyBase)c).GetExp());
                        ui.LogUI.AddLog(((EnemyBase)c).GetExp() + "exp ");
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
                    c.Draw(gamedevice.Renderer);
                }
                if (c is CharacterBase && NearPlayer(c.Collision.Position) && !(c is Player))
                {
                    c.Draw(gamedevice.Renderer);
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

        public void AddPlayer(Vector3 position, ParticleManager pManager, GameManager gamemanager)
        {
            player = new Player
                (
                position,
                gamemanager.PlayerInfo,
                gamedevice,
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
            return enemys;
        }

        public bool DiedCharacters()
        {
            return LiveCharacterCnt() == 0;
        }

        public int LiveCharacterCnt()
        {
            //リンより修正
            //int cnt = 0;
            //foreach (var c in characters)
            //{
            //    if (c == player) continue;
            //    cnt++;
            //}
            //return cnt;
            return characters.Count;    //これで十分、全部回らなくてよい（Countは要素数変動の時のみ処理、Foreachを使わずに済む）
        }

        private void Load()
        {
            FileStream datefs = new FileStream(enemyFilename, FileMode.Open);
            StreamReader enemDate = new StreamReader(datefs, Encoding.GetEncoding("shift_jis"));

            //ごみ捨て

            var Dust = enemDate.ReadLine();

            while (!enemDate.EndOfStream)
            {
                var line = enemDate.ReadLine();
                string[] data = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var id = int.Parse(data[0]);
                var name = data[1];
                var health = int.Parse(data[2]);
                var attack = int.Parse(data[3]);
                var diffence = int.Parse(data[4]);
                var speed = float.Parse(data[5]);
                var aiType = data[6];
                var size = int.Parse(data[7]);
                var attackspd = int.Parse(data[8]);
                var exp = int.Parse(data[9]);
                enemys.Add
                    (
                    id,
                    new EnemyBase
                        (
                        new Status(1, health, attack, diffence, attackspd, speed),
                        new CollisionSphere(Vector3.Zero, size),
                        aiType,
                        name,
                        this,
                        exp
                        )
                    );
            }

            enemDate.Close();
        }
    }
}
