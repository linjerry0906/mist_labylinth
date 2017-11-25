using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team27_RougeLike.Device
{
    public enum SceneType
    {
        Title,
        Load,
        Town,
        Dungeon,
        Pause
    }

    interface IScene
    {
        void Initialize(SceneType scene);
        void Update();
        void Draw();
        void IsEnd();
        void Shutdown();

        bool isEnd();
        SceneType Next();
    }
}
