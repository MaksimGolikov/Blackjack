using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Additation.Control
{
    class Game
    {
        bool round;
        bool game;
        
        List<Card> deck;
        Player croupier;
        Player player;
        Control control;


        public Game()
        {
            deck = new List<Card>();
            
            game = true;
            croupier = new Player("Croupier", 10000000);
        }

        public void Run()
        {
            Console.WriteLine("Enter your name");
            player = new Player(Console.ReadLine(), 1000);

            control = new Control(croupier, player);


            while(game)
            {              
                round = true;
                deck = control.NewGameConfig();

                #region Set First price

                Console.Clear();
                Console.WriteLine("Enter first price");

                bool next = false;
                while(!next)
                {
                    int money;
                    bool canConvert = Int32.TryParse(Console.ReadLine(), out money);

                    if (canConvert)
                    {
                        if (money <= player.Balance && money > 0)
                        {
                            control.Bank = money;
                            next = true;
                        }                        
                    }
                    if (!canConvert || money > player.Balance)
                    {
                        Console.Clear();
                        Console.WriteLine("Enter first price");
                    }                    
                }
                
                Console.WriteLine(control.Get_Bank());
                Console.ReadLine();
                Console.Clear();
                #endregion

                for (int i = 0; i < 2; i++)
                {
                    player.Add_CardToHand(control.GetCard(deck));
                    croupier.Add_CardToHand(control.GetCard(deck));
                }


                if (control.More_Score(croupier, player) == "NO")
                    while (round)
                    {
                        Console.WriteLine(control.Get_Bank());
                        Console.WriteLine("crupe:  ");
                        Console.WriteLine(croupier.ShowCard(0));
                        PrintSpace(5);


                        Console.WriteLine(player.Name + "(" + player.Balance + ") :  ");
                        for (int i = 0; i < player.Get_Count_CardsOnHand(); i++)
                        {
                            Console.WriteLine(player.ShowCard(i));
                        }
                        PrintSpace(3);

                        var commands = control.GetCommandList();
                        for (int i = 0; i < commands.Count; i++)
                        {
                            Console.WriteLine(commands[i]);
                        }
                        PrintSpace(1);


                        var operation = Console.ReadLine();
                        if (operation == "0")
                        {
                            var resultOperation = control.GetCardToHand(deck);
                            if (resultOperation[0] != "OK")
                            {
                                for (int i = 0; i < resultOperation.Count; i++)
                                {                                    
                                   Console.WriteLine(resultOperation[i]);                                    
                                }
                                Console.ReadKey();
                                round = false;
                            }
                        }
                        if(operation == "1")
                        {
                          control.DropCard();
                          round = false;
                        }
                        if(operation == "2")
                        {
                            var resultOperation = control.Open(deck);
                            Console.Clear();
                            for (int i = 0; i < resultOperation.Count; i++)
                            {
                               Console.WriteLine(resultOperation[i]);                                
                            }
                            Console.ReadKey();
                            round = false;
                        }
                        if(operation == "3")
                        {
                            Console.WriteLine("Enter nesassary price");

                            int price;
                            bool isInt = Int32.TryParse(Console.ReadLine(),out price);

                            while(!isInt)
                            {
                                isInt = Int32.TryParse(Console.ReadLine(), out price);                                
                            }
                            Console.WriteLine(control.IncreacePrice(price));
                            Console.ReadKey();
                        }
                                     

                        if (deck.Count < 10)
                        {
                            deck = control.CreateDeck();
                        }
                        Console.Clear();
                    }

                Console.WriteLine("This round finished.");
                Console.WriteLine("For Exit press e, for continue other key");
                var lin = Console.ReadLine();

                if (lin == "e" || player.Balance <= 0)
                {
                    game = false;
                }
                if (player.Balance <= 0)
                {
                    Console.WriteLine("Game Over. You louse all your money");
                    Console.ReadLine();
                }
            }



        }

        private void PrintSpace (int countSpace)
        {
            while (countSpace > 0)
            {
                Console.WriteLine("");
                countSpace--;
            }
        }
  
    }
}
