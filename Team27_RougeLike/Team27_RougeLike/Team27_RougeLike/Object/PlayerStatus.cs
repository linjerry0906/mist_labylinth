using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Team27_RougeLike.Object.Item;
using Team27_RougeLike.Device;
using Microsoft.Xna.Framework;

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
        private Renderer renderer;
        private int exp;
        private int addPower;
        private int addDefence;
        private float weight;

        public PlayerStatus(Status status, GameDevice gameDevice)
        {
            this.baseStatus = status;
            this.gamedevice = gameDevice;
            renderer = gameDevice.Renderer;
            loader = new ExpLoader();
            expData = loader.LoadExp();
            inventory = new Inventory(gameDevice);
        }

        public void Initialize()
        {
            exp = 0;
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
            CaculateStatus();
            return status.BasePower + addPower;
        }

        public int GetDefence()
        {
            CaculateStatus();
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

        public void Heal(int recovery)
        {
            status.Health += recovery;
            status.Health = (status.Health > status.MaxHealth) ? status.MaxHealth : status.Health;
        }

        public float GetVelocty()
        {
            CaculateStatus();
            return status.Movespeed - weight / 100 / status.BasePower;
        }

        public float GetWeight()
        {
            CaculateStatus();
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

        public void DrawUIStatue()
        {
            Vector2 gagePos = new Vector2(40, 45);
            float hpRate = GetHP() * 1.0f / GetMaxHP();
            float expRate = exp * 1.0f / expData[GetLevel()];
            renderer.DrawTexture(
                "hp_back", gagePos);
            renderer.DrawTexture(
                "hp", gagePos,
                new Rectangle(0, 0, (int)(384 * hpRate), 64));
            renderer.DrawTexture(
                "exp", gagePos,
                new Rectangle(0, 0, (int)(384 * expRate), 64));
            renderer.DrawString(
                GetLevel().ToString(),
                gagePos + new Vector2(35, 30),
                new Color(25, 180, 255), 
                new Vector2(1.5f, 1.5f) ,1.0f,
                true, true);
            renderer.DrawTexture(
                "hp_gage", gagePos);
            renderer.DrawTexture(
                "hp_deco", gagePos);
        }
    }
}
