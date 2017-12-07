using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.AI;
namespace Team27_RougeLike.Object.Character
{
    class CharacterManager
    {
        private GameDevice gamedevice;
        private ContentManager content;
        private List<CharacterBase> characters = new List<CharacterBase>();
        private Player player;

        private const int drawLength = 100;

        public CharacterManager(GameDevice gamedevice, ContentManager content)
        {
            this.gamedevice = gamedevice;
            this.content = content;
        }

        public void Initialize()
        {
            player = new Player(content.Load<Model>("Donut"), gamedevice);
            AddCharacter(player);
            AddCharacter(new TestSimpleMeleeEnemy(content.Load<Model>("testPlayer"), new Vector3(10, 0, 5)));
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
                    c.Draw(gamedevice);
                }
                if (c is CharacterBase && NearPlayer(c))
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
        /// プレイヤーが近くにいるかどうか
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool NearPlayer(CharacterBase character)
        {
            //if (player == null) return false;
            //if (player.IsDead()) return false;

            return Math.Abs(character.transform.position.X - player.transform.position.X) < drawLength && Math.Abs(character.transform.position.Y - player.transform.position.Y) < drawLength;
        }
    }
}
