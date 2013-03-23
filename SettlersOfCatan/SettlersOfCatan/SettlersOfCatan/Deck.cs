using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    class Deck
    {
        static int soldierAmnt = 14;
        static int yearOPAmnt = 2;
        static int monopolyAmnt = 2;
        static int roadBuilding = 2;
        static int victoryAmnt = 5;

        List<Card> cards = new List<Card>();
        List<Card> discard = new List<Card>();

        public Deck()
        {
            for (int i = 0; i < soldierAmnt; i++)
            { cards.Add(new Card(CardType.soldier)); }
            for (int i = 0; i < yearOPAmnt; i++)
            { cards.Add(new Card(CardType.yearOfPlenty)); }
            for (int i = 0; i < monopolyAmnt; i++)
            { cards.Add(new Card(CardType.monopoly)); }
            for (int i = 0; i < roadBuilding; i++)
            { cards.Add(new Card(CardType.roadBuilding)); }
            for (int i = 0; i < victoryAmnt; i++)
            { cards.Add(new Card(CardType.victory)); }


        }

        public void Shuffle()
        {
            for (int i = 0; i < 100; i++)
            {
                int n1 = Config.rand.Next(cards.Count);
                int n2 = Config.rand.Next(cards.Count);
                Card card = cards[n1];
                cards.RemoveAt(n1);
                cards.Insert(n2, card);
            }
        }

        public Card DrawCard()
        {
            Card c = cards[cards.Count - 1];
            cards.Remove(c);
            return c;
        }
    }
}
