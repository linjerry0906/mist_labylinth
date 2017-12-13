﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Team27_RougeLike.Device;
using Team27_RougeLike.Object.Character;
using Team27_RougeLike.Object.Box;
namespace Team27_RougeLike.Object.Character
{
    class CharacterManager
    {
        private GameDevice gamedevice;
        private List<CharacterBase> characters = new List<CharacterBase>();
        private List<HitBoxBase> hitBoxs = new List<HitBoxBase>();
        private Player player;

        private const int drawLength = 100;

        public CharacterManager(GameDevice gamedevice)
        {
            this.gamedevice = gamedevice;
        }

        public void Initialize(Vector3 position)
        {
            characters.Clear();
            hitBoxs.Clear();
            //デバッグ用、呼び出すときはプレイヤーを生成してから！
            AddPlayer(position);
            AddCharacter(new TestSimpleMeleeEnemy(player.Collision.Position,this));
            AddCharacter(new TestSimpleMeleeEnemy(new Vector3
                (
                player.Collision.Position.X + 4,
                player.Collision.Position.Y,
                player.Collision.Position.Z + 4
                ), this));
        }

        public void Update(GameTime gameTime)
        {
            foreach (var c in characters)
            {
                c.Update(gameTime);

                if (c is EnemyBase && c.HitCheck(player))
                {
                    ((EnemyBase)c).HitUpdate(player, gameTime);
                }
            }
            foreach (var h in hitBoxs)
            {
                foreach (var c in characters)
                {
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
            player = new Player(position, gamedevice,this);
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
        public bool NearPlayer(CharacterBase character)
        {
            //if (player == null) return false;         エラーチェック　デバッグのため未実装
            //if (player.IsDead()) return false;

            return Math.Abs(character.Collision.Position.X - player.Collision.Position.X) < drawLength && Math.Abs(character.Collision.Position.Y - player.Collision.Position.Y) < drawLength;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public List<CharacterBase> Characters()
        {
            return characters;
        }
    }
}
