using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Threading.Tasks;
namespace Team27_RougeLike.Object.Character
{
    class Spawner
    {
        private int spawnrate;
        private int currentTime;
        private int spawnCharacterID;
        private int[] spawnCharacterIDs;
        private CharacterManager characterManager;
        private List<CharacterBase> liveCharacters;
        private Vector3 position;
        private int seed = Environment.TickCount;
        private Random rand = new Random();

        /// <summary>
        /// 最大スポーン数
        /// </summary>
        private int maxNum;

        /// <summary>
        /// 同時スポーン数
        /// </summary>
        private int spawnNum;

        /// <summary>
        /// 一種類
        /// </summary>
        /// <param name="spawnrate"></param>
        /// <param name="position"></param>
        /// <param name="spawnCharacterID"></param>
        /// <param name="maxNum"></param>
        /// <param name="spawnNum"></param>
        /// <param name="characterManager"></param>
        public Spawner(int spawnrate, Vector3 position, int spawnCharacterID, int maxNum, int spawnNum, CharacterManager characterManager)
        {
            this.spawnrate = spawnrate;
            this.position = position;
            this.spawnCharacterID = spawnCharacterID;
            this.maxNum = maxNum;
            this.spawnNum = spawnNum;
            this.characterManager = characterManager;
            liveCharacters = new List<CharacterBase>();
        }
        
        /// <summary>
        /// 数種類からランダム
        /// </summary>
        /// <param name="spawnrate"></param>
        /// <param name="position"></param>
        /// <param name="spawnCharacterIDs"></param>
        /// <param name="maxNum"></param>
        /// <param name="spawnNum"></param>
        /// <param name="characterManager"></param>
        public Spawner(int spawnrate, Vector3 position, int[] spawnCharacterIDs, int maxNum, int spawnNum, CharacterManager characterManager)
        {
            this.spawnrate = spawnrate;
            this.position = position;
            this.spawnCharacterIDs = new int[spawnCharacterIDs.Length];
            spawnCharacterIDs.CopyTo(this.spawnCharacterIDs, 0);
            this.spawnNum = spawnNum;
            this.maxNum = maxNum;
            this.characterManager = characterManager;
            liveCharacters = new List<CharacterBase>();
        }

        public void Update()
        {
            currentTime++;

            if (currentTime >= spawnrate)
            {
                for (int i = 0; i < spawnNum; i++)
                {
                    if (!(liveCharacters.Count() < maxNum)) break;

                    //ランダムを消したい場合はこちらを消してください
                    Vector3 randpos = new Vector3(rand.Next(-10, 10), 0, rand.Next(-10, 10));

                    currentTime = 0;
                    if (spawnCharacterIDs == null)
                    {
                        liveCharacters.Add(characterManager.AddCharacter(characterManager.Enemys()[spawnCharacterID].Clone(position + randpos)));
                    }
                    else
                    {
                        int ID = spawnCharacterIDs[rand.Next(0, spawnCharacterIDs.Length)];
                        liveCharacters.Add(characterManager.AddCharacter(characterManager.Enemys()[ID].Clone(position + randpos)));

                    }
                }
            }

            liveCharacters.RemoveAll((CharacterBase c) => c.IsDead());
        }
    }
}
