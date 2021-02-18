using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    class BattleStation : PlayerControl
    {
        public BattleStation(GameState game, int playerNum) : base(game, playerNum)
        {
        }

        public override void PerformDuty()
        {
            PickSpot();
        }

        private void PickSpot()
        {
        }
    }
}
