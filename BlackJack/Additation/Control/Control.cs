using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Additation;
using BlackJack.Additation.Enums;



namespace BlackJack.Additation.Control
{
    class Control
    {
        
        private Player croupier;
        private Player player;
        private int bank;

        public int Bank { get => bank; set => bank = value; }

        public Control(Player newCroupier, Player newPlayer)
        {
            croupier = newCroupier;
            player = newPlayer;
            Bank = 0;
        }


        public List<Card> NewGameConfig()
        {
            bank = 0;
            player.Drop_Card();
            croupier.Drop_Card();
            return CreateDeck();
        }

        public Card GetCard(List<Card> Deck)
        {
            Random rand = new Random();
            int id = rand.Next(0, Deck.Count);

            Card returnedCard = Deck[id];
            Deck.RemoveAt(id);

            return returnedCard;
        }
       
        public List<Card> CreateDeck()
        {
            List<Card> returnedDeck = new List<Card>();
            int curd = 2;

            for (int i = 2; i < 54; i++)
            {
                if (i <= 10)
                {
                    AddCard(curd.ToString(), curd, returnedDeck);
                }
                if (i>10)
                {
                    if (curd == 11)
                    {
                        AddCard("Валет", 10, returnedDeck);
                    }
                    if (curd == 12)
                    {
                        AddCard("Дама", 10, returnedDeck);
                    }
                    if (curd == 13)
                    {
                        AddCard("Король", 10, returnedDeck);
                    }
                    if (curd == 14)
                    {
                        AddCard("Туз", 11, returnedDeck);
                    }
                }
                
                curd++;
            }

            Random rand = new Random();
            for (int i = 0; i < returnedDeck.Count - 1; i++)
            {
                int nom = rand.Next(52);
                if (nom % 2 == 0)
                {
                    Card prom = returnedDeck[i];
                    returnedDeck[i] = returnedDeck[nom];
                    returnedDeck[nom] = prom;
                }
            }

            return returnedDeck;
        }

        public void AddCard(string curd, int score, List<Card> deck)
        {             
            for (int j = 0; j < Enum.GetNames(typeof(Suit)).Length; j++)
            {
                Card newCard = new Card(curd, (Suit)j, score);
                deck.Add(newCard);
            }
        }

        public List<string> GetCommandList()
        {
            List<string> returnedCommand = new List<string>();

            for (int j = 0; j < Enum.GetNames(typeof(Commands)).Length; j++)
            {
                returnedCommand.Add("Чтобы " + (Commands)j + " введи: " + j);
            }

            return returnedCommand;
        }

        public string Get_Bank()
        {
            return "Банк: "+Bank*2;
        }
       
       
        public List<string> Open(List<Card> deck)
        {
            List<string> reterned = new List<string>();

            int sum_Croupier = croupier.Get_SumCards();
            int sum_Player = player.Get_SumCards();
            
           
            while (sum_Croupier < 17)
            {
                croupier.Add_CardToHand(GetCard(deck));
                sum_Croupier = croupier.Get_SumCards();
            }
            
            reterned.Add("crupe:  ");
            for (int i = 0; i < croupier.Get_Count_CardsOnHand(); i++)
            {
                reterned.Add(croupier.ShowCard(i));
            }
            reterned.Add("");
            reterned.Add("Sum is  " + sum_Croupier);
            AddFreeSpace(reterned,5);

            

            reterned.Add("You:  ");
            for (int i = 0; i < player.Get_Count_CardsOnHand(); i++)
            {
                reterned.Add(player.ShowCard(i));
            }
            reterned.Add("");
            reterned.Add("Sum is  " + sum_Player);
            AddFreeSpace(reterned,3);

           
                    if (sum_Player == sum_Croupier)
                    {
                        reterned.Add(" Nich ");
                    }
                    if ( (sum_Player > sum_Croupier && sum_Player <= 21 && sum_Croupier <= 21) ||
                         (sum_Player < 21 && sum_Croupier > 21))
                    {
                        reterned.Add("You WIN");
                        player.ChangeBalance(bank, true);
                    }
                    if ((sum_Player < sum_Croupier && sum_Player <= 21 && sum_Croupier <= 21) ||
                        (sum_Player > 21 && sum_Croupier < 21))
                    {
                        reterned.Add("Croupier WIN");
                        player.ChangeBalance(bank, false);
                    }
           
            return reterned;
        }
        public List<string> GetCardToHand(List<Card> deck)
        {
            List<string> returned = new List<string>();

            player.Add_CardToHand(GetCard(deck));
            int PlayerScore = player.Get_SumCards();
            int CroupierScore = croupier.Get_SumCards();

            if (PlayerScore == 21 &&
                CroupierScore < 21)
            {
                returned.Add("You take " + player.ShowCard(player.Get_Count_CardsOnHand() - 1));
                returned.Add("Blackjack, You WIN");               
                player.ChangeBalance(bank, true);
            }
            if (PlayerScore > 21 && CroupierScore < 21)
            {
                returned.Add("You take " + player.ShowCard(player.Get_Count_CardsOnHand() - 1));
                returned.Add(" Croupier WIN");               
                player.ChangeBalance(bank, false);
            }
            if(PlayerScore < 21)
            {
                returned.Add("OK");
            }

            return returned;
        }
        public string DropCard()
        {            
            player.Drop_Card();           
            player.ChangeBalance(bank, false);
            return "END";
        }
        public string IncreacePrice(int price)
        {
            string returned = "";
            if (price > (player.Balance - bank) && price > 0)
            {
                returned = "You can`t give more than you have";
            }
            if (price <= (player.Balance-bank) && price > 0)
            {
                bank += price;
                returned = "Bank is " + 2 * bank;
            }
                       

            return returned;
        }
        public string More_Score(Player croupier_, Player player_)
        {
            string returnedValue = "NO";

            if (player_.Get_SumCards() > 21)
            {
                returnedValue = "Croupier WIN";                
            }
            if (croupier_.Get_SumCards() > 21 && player_.Get_SumCards() <= 21)
            {
                returnedValue = "You WIN";
            }
            return returnedValue;
        }
        

        private void AddFreeSpace(List<string> list, int countOfSpace)
        {
            for (int i = 0; i < countOfSpace; i++)
            {
                list.Add(" ");
            }
        }



    }
}
