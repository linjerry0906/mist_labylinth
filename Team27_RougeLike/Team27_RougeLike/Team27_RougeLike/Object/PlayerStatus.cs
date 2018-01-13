using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.Device;
using Team27_RougeLike.UI;
namespace Team27_RougeLike.Object
{
    class PlayerStatus
    {
        private Status status;
        private Status baseStatus;
        private Inventory inventory;
        private ExpLoader loader;
        private Dictionary<int, int> expData;
        private GameDevice gamedevice;
        private int exp;
        private int addPower;
        private int addDefence;
        private float weight;

        public PlayerStatus(Status status, GameDevice gameDevice)
        {
            this.baseStatus = status;
            this.gamedevice = gameDevice;
            loader = new ExpLoader();
            expData = loader.LoadExp();
            inventory = new Inventory(gameDevice);
        }

        public void Initialize()
        {
            status = new Status(baseStatus.Level, baseStatus.Health, baseStatus.BasePower, baseStatus.BaseArmor, baseStatus.Attackspd, baseStatus.Movespeed);
        }

        public void CaculateStatus()
        {
            inventory.GetStatus(ref addPower, ref addDefence, ref weight);
        }

        public int GetLevel()
        {
            return status.Level;
        }

        public int GetMaxHP()
        {
            return status.MaxHealth;
        }

        public int GetHP()
        {
            return status.Health;
        }

        public int GetPower()
        {
            return status.BasePower + addPower;
        }

        public int GetDefence()
        {
            return status.BaseArmor + addDefence;
        }

        public int GetAttackSpeed()
        {
            return status.Attackspd + (int)(weight / 10 / status.BasePower);
        }

        public void Damage(int damage)
        {
            status.Health -= damage;
        }

        public float GetVelocty()
        {
            return status.Movespeed - weight / 100 / status.BasePower;
        }

        public float GetWeight()
        {
            return weight;
        }

        public void AddExp(int exp)
        {
            this.exp += exp;
            if(expData[GetLevel()] < this.exp)
            {
                this.exp -= expData[GetLevel()];
                status.LevelUp();
                //ui.LogUI.AddLog("LevelUp");
                //ui.LogUI.AddLog("Player Level is "+ status.Level);

            }
        }

        public Inventory GetInventory()
        {
            return inventory;
        }
    }
}
