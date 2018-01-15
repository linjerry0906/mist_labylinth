using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Team27_RougeLike.Scene
{
    public enum SceneType
    {
        Title,
        Load,
        LoadTown,
        Town,
        ItemShop,
        Depot,
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
