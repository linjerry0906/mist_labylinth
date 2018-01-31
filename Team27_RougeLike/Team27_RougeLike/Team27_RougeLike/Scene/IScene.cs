using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.Scene
{
    public enum SceneType
    {
        Logo,
        Title,
        Load,
        LoadTown,
        Town,
        UpgradeStore,
        LoadShop,
        ItemShop,
        Depot,
        Quest,
        DungeonSelect,
        LoadMap,
        Dungeon,
        LoadBoss,
        Boss,
        Pause
    }

    interface IScene
    {
        void Initialize(SceneType scene);
        void Update(GameTime gameTime);
        void Draw();
        void ShutDown();
        bool IsEnd();
        SceneType Next();
    }
}
