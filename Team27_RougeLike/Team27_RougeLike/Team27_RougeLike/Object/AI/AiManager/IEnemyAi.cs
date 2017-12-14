using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team27_RougeLike.Object.Character;
namespace Team27_RougeLike.Object.AI
{ 
    interface IEnemyAI
    {
        void HitUpdate(Player player);
    }
}
