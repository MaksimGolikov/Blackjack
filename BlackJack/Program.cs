using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class Program
    {
        static List<Additation.Card> Deck;
        static Additation.Player Croupier;
        static Additation.Player Player;
        static int bank;


        static void Main(string[] args)
        {
            bool game = true;
            bool exit = false;
            bank = 0;
            Deck = new List<Additation.Card>();
           
            Croupier = new Additation.Player("Croupier", 10000000);
            
            Console.WriteLine("Enter your name");
            Player = new Additation.Player(Console.ReadLine(),1000);


            do
            {
                bank = 0;
                game = true;
                Player.Drop_Card();
                Croupier.Drop_Card();

                CreateDeck();

                Console.Clear();
                Console.WriteLine("Enter first price");
                bool next = false;
                
                do
                {
                    try
                    {
                        int money = Convert.ToInt32(Console.ReadLine());
                        if (money <= Player.Balance)
                        {
                            bank += money;
                            next = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Enter first price");
                        }
                       
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Enter first price");
                    }
                } while (!next);
                next = true;
                
                Console.WriteLine("Bank is " + 2 * bank);
                Console.ReadLine();
                Console.Clear();


                for (int i = 0; i < 2; i++)
                {
                    Player.Add_CardToHand(GetCard());
                    Croupier.Add_CardToHand(GetCard());
                }


                if(!More_Score())
                do
                {
                    Console.WriteLine("Bank is " + 2 * bank);
                    Console.WriteLine("crupe:  ");
                    Additation.Card c = Croupier.ShowCard(0);
                    Console.WriteLine(c.Name + "  " + c.Type);
                    for (int i = 0; i < 5; i++) { Console.WriteLine(" "); }


                    Console.WriteLine(Player.Name+"("+Player.Balance+") :  ");
                    for (int i = 0; i < Player.Get_Count_CardsOnHand(); i++)
                    {
                        c = Player.ShowCard(i);
                        Console.WriteLine(c.Name + "  " + c.Type);
                    }
                    for (int i = 0; i < 3; i++) { Console.WriteLine(" "); }
                    WriteCommand();

                    game = DoOperation(Console.ReadLine());
                    Console.Clear();

                } while (game);

                Console.Clear();
                Console.WriteLine("For Exit press e, for continue other key");
                string lin = Console.ReadLine();

                if (lin == "e" || Player.Balance<=0) { exit = true; }
                if (Player.Balance <= 0) { Console.WriteLine("Game Over. You louse all your money"); }

            } while (!exit);                

        }





        static Additation.Card GetCard()
        {
            Random rand = new Random();
            int id = rand.Next(0,Deck.Count);

            Additation.Card returnedCard = Deck[id];
            Deck.RemoveAt(id);

            return returnedCard;
        }
        static void CreateDeck()
        {
            int curd = 2;

            for(int i=2; i<54; i++)
            {
                if(i<=10)
                {
                    AddCard(curd.ToString(),curd);        
                }
               else
                {
                    switch (curd)
                    {
                        case 11:
                            AddCard("Валет",10);
                            break;
                        case 12:
                            AddCard("Дама",10);
                            break;
                        case 13:
                            AddCard("Король",10);
                            break;
                        case 14:
                            AddCard("Туз",11);
                            break;
                    }
                }
                curd++;
            }

            //Mix card
            Random rand = new Random();
            for (int i=0;i<Deck.Count-1;i++)
            {
                int nom = rand.Next(52);
                if (nom%2 ==0)
                {
                    Additation.Card prom = Deck[i];
                    Deck[i] = Deck[nom];
                    Deck[nom] = prom;
                }
            }

        }

        static void AddCard(string curd,int score)
        {
            for (int j = 0; j < Enum.GetNames(typeof(Additation.Suit)).Length; j++)
            {
                Additation.Card newCard = new Additation.Card(curd, (Additation.Suit)j, score);
                Deck.Add(newCard);
            }
        }

        static void WriteCommand()
        {
            for (int j = 0; j < Enum.GetNames(typeof(Additation.Commands)).Length; j++)
            {
                Console.WriteLine( "For "+ (Additation.Commands)j+" enter "+j);
            }
        }
        static bool DoOperation(string operation)
        {
            bool reterned = true;

            switch (operation)
            {
                case "0":// добрать
                    Player.Add_CardToHand(GetCard());
                    int PlayerScore = Player.Get_SumCards();
                    int CroupierScore = Croupier.Get_SumCards();


                    if (PlayerScore == 21 &&
                        CroupierScore < 21) {
                        Console.WriteLine("Blackjack, You WIN");
                        reterned = false;
                        Player.ChangeBalance(bank, true);
                    }else if (PlayerScore > 21) {
                        Console.WriteLine(" Croupier WIN");
                        reterned = false;
                        Player.ChangeBalance(bank, false);
                    }
                    
                    break;
                case "1":// сбросить
                    Player.Drop_Card();
                    Player.ChangeBalance(bank, false);
                    reterned = false;
                    break;
                case "2":// вскрываемся
                    int sum_Croupier = 0;
                    int sum_Player = 0;

                    sum_Croupier = Croupier.Get_SumCards();
                    sum_Player = Player.Get_SumCards();

                    if(sum_Croupier<17)
                    do
                    {
                      Croupier.Add_CardToHand(GetCard());
                      sum_Croupier = Croupier.Get_SumCards();

                    } while (sum_Croupier<17);



                    Console.Clear();

                    Console.WriteLine("crupe:  ");
                    for (int i = 0; i < Croupier.Get_Count_CardsOnHand(); i++)
                    {
                        Additation.Card c = Croupier.ShowCard(i);                        
                        Console.WriteLine(c.Name + "  " + c.Type);
                    }
                    Console.WriteLine("");
                    Console.WriteLine("Sum is  "+sum_Croupier);
                    for (int i = 0; i < 5; i++) { Console.WriteLine(" "); }
                    

                    Console.WriteLine("You:  ");
                    for (int i = 0; i < Player.Get_Count_CardsOnHand(); i++)
                    {
                        Additation.Card c = Player.ShowCard(i);                        
                        Console.WriteLine(c.Name + "  " + c.Type);
                    }
                    Console.WriteLine("");
                    Console.WriteLine("Sum is  " + sum_Player);
                    for (int i = 0; i < 3; i++) { Console.WriteLine(" "); }



                    if (sum_Player == sum_Croupier) { Console.WriteLine(" Nich "); reterned = false; }
                    else if ( (sum_Player > sum_Croupier &&
                             sum_Player <= 21 && sum_Croupier <= 21) ||
                              (sum_Player<21 && sum_Croupier>21) )
                         {
                             Console.WriteLine("You WIN");
                             Player.ChangeBalance(bank,true);
                             reterned = false;
                    } else {
                            Console.WriteLine("Croupier WIN");
                            Player.ChangeBalance(bank, false);
                            reterned = false;
                    }
                    Console.ReadLine();
                    break;
                case "3":// поднять
                    Console.WriteLine("Enter nesassary price");
                    bank += Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Bank is"+ 2*bank);
                    break;
                default:
                    break;
            }
            return reterned;
        }

        static bool More_Score()
        {
            bool returnedValue = false;

            if (Player.Get_SumCards() > 21 )
            {                
                Console.WriteLine("Croupier WIN");
                returnedValue = true;
            }
            else if (Croupier.Get_SumCards() > 21)
            {
                Console.WriteLine("You WIN");
                returnedValue = true;
            }
            return returnedValue;
        }

    }
}
