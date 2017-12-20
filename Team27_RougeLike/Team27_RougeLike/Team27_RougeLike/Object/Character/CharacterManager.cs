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
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.Box;
using Team27_RougeLike.Object.AI;
namespace Team27_RougeLike.Object.Character
{
    class CharacterManager
    {
        private GameDevice gamedevice;
        private List<CharacterBase> characters = new List<CharacterBase>();
        private List<HitBoxBase> hitBoxs = new List<HitBoxBase>();
        private Player player;

        private Dictionary<int, EnemyBase> enemys = new Dictionary<int, EnemyBase>();

        private string enemyFilename;
        private const int drawLength = 120;


        public CharacterManager(GameDevice gamedevice)
        {
            this.gamedevice = gamedevice;
            enemyFilename = @"Content/" + "EnemysCSV/Enemy.csv";
            Load();
        }

        public void Initialize(Vector3 position)
        {
            characters.Clear();
            hitBoxs.Clear();
            //デバッグ用、呼び出すときはプレイヤーを生成してから！
            AddPlayer(position);
            AddCharacter(enemys[1].Clone(player.Collision.Position));
            AddCharacter(enemys[2].Clone(new Vector3
                (player.Collision.Position.X + 4,
                player.Collision.Position.Y,
                player.Collision.Position.Z + 8
                )));
            AddCharacter(enemys[2].Clone(player.Collision.Position));
            AddCharacter(enemys[3].Clone(new Vector3
                         (
                         player.Collision.Position.X + 4,
                         player.Collision.Position.Y,
                         player.Collision.Position.Z + 4
                         )));
        }

        public void Update(GameTime gameTime)
        {
            foreach (var c1 in characters)
            {
                c1.Update(gameTime);

                if (c1 is EnemyBase)
                {
                    //敵の距離によってアップデートを分けた
                    if (NearPlayer(c1))
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
                        h.Effect(c);
                    }
                }
                h.Update();
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
                if (c is CharacterBase && NearPlayer(c) && !(c is Player))
                {
                    c.Draw(gamedevice.Renderer);
                }
            }
        }

        public void AddCharacter(CharacterBase character)
        {
            characters.Add(character);
        }

        public void AddPlayer(Vector3 position)
        {
            player = new Player(position, gamedevice, this);
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
        private bool NearPlayer(CharacterBase character)
        {
            //if (player == null) return false;         エラーチェック　デバッグのため未実装
            //if (player.IsDead()) return false;

            return Math.Abs(character.Collision.Position.X - player.Collision.Position.X) < drawLength && Math.Abs(character.Collision.Position.Y - player.Collision.Position.Y) < drawLength;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public List<CharacterBase> GetCharacters()
        {
            return characters;
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
                enemys.Add
                    (
                    id,
                    new EnemyBase
                        (
                        new Status(1, health, attack, diffence, attackspd, speed),
                        new CollisionSphere(Vector3.Zero, size),
                        aiType,
                        name,
                        this
                        )
                    );
            }

            enemDate.Close();
        }
    }
}
