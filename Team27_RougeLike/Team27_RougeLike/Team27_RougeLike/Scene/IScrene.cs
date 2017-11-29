using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Scene
{
    public enum SceneType
    {
        Title,
        Load,
        Town,
        LoadMap,
        Dungeon,
        Pause
    }

    interface IScene
    {
        void Initialize(SceneType scene);
        void Update();
        void Draw();
        void Shutdown();
        bool IsEnd();
        SceneType Next();
    }
}
