using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Additation
{
    class Card
    {
        string name;
        Suit type;
        int score;

        public Card(string Name, Suit Type, int Score)
        {
            name = Name;
            type = Type;
            score = Score;
        }

        public string Name { get => name; }
        public int Score { get => score; }
        internal Suit Type { get => type;}
    }
}
