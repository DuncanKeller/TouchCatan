using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    enum CardType
    {
        soldier,
        yearOfPlenty,
        monopoly,
        roadBuilding,
        victory
    }

    class Card
    {
        CardType type;

        public Card(CardType t)
        {
            type = t;
        }

        public void Use(Player p)
        {
          
        }
    }
}
