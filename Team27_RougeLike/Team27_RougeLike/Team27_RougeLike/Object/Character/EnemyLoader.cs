using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Box;
using Microsoft.Xna.Framework;
using Team27_RougeLike.Object.AI;
using Team27_RougeLike.Device;
using System.IO;
namespace Team27_RougeLike.Object.Character
{
    class EnemyLoader
    {
        private string enemyFilename;
        private Dictionary<int, EnemyBase> enemys = new Dictionary<int, EnemyBase>();

        public Dictionary<int,EnemyBase> Enemys()
        {
            return enemys;
        }

        public void Initialize(CharacterManager charactermanager)
        {
            enemyFilename = @"Content/" + "EnemysCSV/Enemy.csv";

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
                        charactermanager,
                        exp
                        )
                    );
            }
            enemDate.Close();
            datefs.Close();
        }
        public void Initialize(CharacterManager charactermanager, GameDevice gamedevice)
        {
            enemyFilename = @"Content/" + "EnemysCSV/Enemy.csv";

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
                        charactermanager,
                        exp,
                        gamedevice
                        )
                    );
            }
            enemDate.Close();
            datefs.Close();
        }
    }
}
