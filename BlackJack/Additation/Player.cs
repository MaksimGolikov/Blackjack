using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Additation
{
    class Player
    {
        string name;
        List<Card> onHand;
        int balance;

        public int Balance { get => balance;}
        public string Name { get => name; }




        public Player(string Name, int Balance)
        {
            name = Name;
            balance = Balance;

            onHand = new List<Card>();
        }

        public int Get_SumCards()
        {
            bool existTuz = false;
            int sum = 0;
            foreach (Card card in onHand)
            {
                sum += card.Score;
                if (card.Name == "Туз") { existTuz = true; }
            }
            if (sum>21 && existTuz) { sum -= 10; }

            return sum;
        }
        public int Get_Count_CardsOnHand()
        {
            return onHand.Count;
        }
        public void Add_CardToHand(Card newCard)
        {
            onHand.Add(newCard);         
        }
        public void Drop_Card()
        {
            onHand = new List<Card>();
        }
        public void ChangeBalance(int sum, bool ShouldAdd)
        {
            if (ShouldAdd) { balance += sum; }
            else { balance -= sum; }
        }

        public string ShowCard(int idCard)
        {
            return onHand[idCard].Name+"  "+onHand[idCard].Type;
        }


    }
}
