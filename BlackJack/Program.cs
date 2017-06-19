using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Additation;


namespace BlackJack
{
    class Program
    {
        

        static void Main(string[] args)
        {
            bool game = true;
            bool exit = false;


            List<Card> Deck = new List<Card>();
            Player Croupier = new Player("Croupier", 10000000); ;
            Player Player;
            
            
            
            Console.WriteLine("Enter your name");
            Player = new Player(Console.ReadLine(),1000);

            Control control = new Control(Croupier,Player);
            

            do
            {                              
                game = true;
                Deck = control.NewGameConfig();


                #region Set First price
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
                            control.Bank = money;
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
                
                Console.WriteLine(control.Get_Bank());
                Console.ReadLine();
                Console.Clear();
#endregion



                for (int i = 0; i < 2; i++)
                {                  
                    Player.Add_CardToHand(control.GetCard(Deck));
                    Croupier.Add_CardToHand(control.GetCard(Deck));
                }


                if(control.More_Score(Croupier,Player) == "NO")
                do
                {
                    Console.WriteLine(control.Get_Bank());
                    Console.WriteLine("crupe:  ");                    
                    Console.WriteLine(Croupier.ShowCard(0));
                    for (int i = 0; i < 5; i++) { Console.WriteLine(" "); }


                    Console.WriteLine(Player.Name+"("+Player.Balance+") :  ");
                    for (int i = 0; i < Player.Get_Count_CardsOnHand(); i++)
                    {
                        Console.WriteLine(Player.ShowCard(i));
                    }
                    for (int i = 0; i < 3; i++) { Console.WriteLine(" "); }

                    List<string> commands = control.GetCommandList();
                    for (int i = 0; i < commands.Count; i++) { Console.WriteLine(commands[i]); }
                    Console.WriteLine(" ");

                    List<string> resultOperation = new List<string>();
                    string operation = Console.ReadLine();
                    
                        switch (operation)
                        {
                            case "0":// добрать
                               resultOperation =  control.GetCardToHand(Deck);
                                if (resultOperation[0] !="OK")
                                {
                                    for (int i = 0; i < resultOperation.Count; i++)
                                    {
                                        if (resultOperation[i] != "END") { Console.WriteLine(resultOperation[i]); }
                                    }
                                }
                                break;
                            case "1":// сбросить                                
                                resultOperation.Add(control.DropCard());
                                break;
                            case "2":// вскрываемся
                                resultOperation = control.Open(Deck);
                                Console.Clear(); ;
                                for (int i = 0; i < resultOperation.Count; i++){
                                    if (resultOperation[i] != "END") { Console.WriteLine(resultOperation[i]); }
                                }
                                break;
                            case "3":// поднять
                                bool isInt = false;
                                do
                                {
                                    try
                                    {
                                        Console.WriteLine("Enter nesassary price");
                                        int price = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine(control.IncreacePrice(price));
                                        isInt = true;
                                    }
                                    catch { }
                                } while (!isInt);
                                break;
                            default:
                                resultOperation.Add("END");
                                break;
                        }
                        Console.ReadLine();
                        Console.Clear();
                    
                    if (resultOperation.Count>0 && resultOperation[resultOperation.Count-1] == "END") { game = false; }                       
                    Console.Clear();
                    if (Deck.Count<10) { Deck = control.CreateDeck(); }

                } while (game);
                
                Console.WriteLine("For Exit press e, for continue other key");
                string lin = Console.ReadLine();

                if (lin == "e" || Player.Balance<=0) { exit = true; }
                if (Player.Balance <= 0) { Console.WriteLine("Game Over. You louse all your money"); Console.ReadLine(); }

            } while (!exit);                

        }





       

    }
}
