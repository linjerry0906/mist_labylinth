using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.Character
{
    class CharacterManager
    {
        private GameDevice gamedevice;
        private List<CharacterBase> characters = new List<CharacterBase>();

        /// <summary>
        /// 3DモデルをDrawする場合に必要 どちらにするかあいまいな為放置
        /// </summary>
        private ContentManager content;

        private Player player;

        private const int drawLength = 100;

        public CharacterManager(GameDevice gamedevice/*, ContentManager content*/)
        {
            this.gamedevice = gamedevice;
            // this.content = content;
        }

        public void Initialize()
        {
            //AddCharacter(new TestSimpleMeleeEnemy(content.Load<Model>("testPlayer"), new Vector3(10, 0, 5)));
        }

        public void Update()
        {
            foreach (var c in characters)
            {
                c.Update();

                if (c is EnemyBase && c.HitCheck(player))
                {
                    ((EnemyBase)c).HitUpdate(player);
                }

                //コリジョン　自分通しの場合とタグが同じ場合は除く
                foreach (var d in characters)
                {
                    if (c.Tag == d.Tag || c == d) continue;
                }

            }

            characters.RemoveAll((CharacterBase c) => c.IsDead());
        }



        public void Draw()
        {
            foreach (var c in characters)
            {
                if (c is Player)
                {
                    //c.Draw(gamedevice);           元々の3D表示用
                    ((Player)c).Draw();             //2D表示用　取り急ぎ
                }
                if (c is CharacterBase && NearPlayer(c) && !(c is Player))
                {
                    c.Draw(gamedevice);
                }
            }
        }

        public void AddCharacter(CharacterBase character)
        {
            characters.Add(character);
        }

        /// <summary>
        /// リンさんの2D表示用のものです。 
        /// </summary>
        /// <param name="position"></param>
        public void AddPlayer(Vector3 position)
        {
            player = new Player(position, gamedevice);
            characters.Add(player);
        }


        /// <summary>
        /// プレイヤーが近くにいるかどうか
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool NearPlayer(CharacterBase character)
        {
            //if (player == null) return false;         エラーチェック　デバッグのため未実装
            //if (player.IsDead()) return false;

            return Math.Abs(character.transform.position.X - player.transform.position.X) < drawLength && Math.Abs(character.transform.position.Y - player.transform.position.Y) < drawLength;
        }

        public Player GetPlayer()
        {
            return player;
        }
    }
}
