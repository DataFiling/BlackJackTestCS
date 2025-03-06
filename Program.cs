
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security;
using System.Xml.Serialization;

class GameObj
    {

    bool gstate = true;
    bool turn = true;
    bool pstay = false;
    bool cstay = false;

    Dictionary<string, int> phand = new();
    Dictionary<string, int> chand = new();

    int pval = 0;
    int cval = 0;



    Dictionary<string, int> deck = new(); //a layout of the suits in dict form
    Dictionary<string, int> pile = new();   
    
        GameObj()
        {

        string[] facecards = new string[4]; //names of the face cards
        string[] thefour = new string[4]; //four strings for the suits

        //Console.WriteLine("GameObj Test");
        thefour[0] = "Clubs"; thefour[1] = "Spades"; thefour[2] = "Diamonds"; thefour[3] = "Hearts";
        facecards[0] = "Jack"; facecards[1] = "Queen"; facecards[2] = "King"; facecards[3] = "Ace";

        int[] vals = new int[9]; //value of cards through aces
            for (int cnt = 2; cnt <= 10; ++cnt) // filling all values of nonfacecards
        {
            vals[cnt - 2] = cnt;
            //Console.WriteLine(vals[cnt - 2]);
        }

            foreach (string c in thefour)
            {

            string nm;

            foreach (string v in facecards)
                {

                    if (v == "Ace")
                    {
                
                        nm = $"{v}" + " of " + $"{c}";
                    //Console.WriteLine(nm);
                    deck.Add(nm, 11);

                }
                else
                {
                    nm = $"{v}" + " of " + $"{c}";
                    deck.Add(nm, 10);
                    //Console.WriteLine(nm);
                }
                }

                foreach (int b in vals) 
            {

                //Console.WriteLine($"{b}" + " cards");
                nm = $"{b}" + " of " + $"{c}";

                deck.Add(nm, b);
                }
            }

        }

    public void draw(ref Dictionary<string, int> hand, ref int vl)
    {

        Random rnd = new();

        KeyValuePair<string, int> card = deck.ElementAt(rnd.Next(0,deck.Count));
        deck.Remove(card.Key);
        hand.Add(card.Key,card.Value);
        vl += card.Value;

        pile.Add(card.Key, card.Value);
   
    }
    public void clear()
    {
        pval = 0;
        cval = 0;

        phand.Clear();
        chand.Clear();

        turn = true;
    }
    public void shuffle()
    {
        deck = pile;
    }
    public void Logic()
    {
        //Console.WriteLine("Logic Test");

        Random choice = new();

        if (turn == false)
        {
            if(cval < 21 && cval > 15)
            {
              
                if(choice.Next(0,7) < 2)
                {
                    Console.WriteLine("Computer Hits");
                    draw(ref chand, ref cval);
                    Console.WriteLine($"Computer draws {chand.ElementAt(chand.Count - 1)})");
                }
                else
                {
                    Console.WriteLine("Computer Stays");
                    cstay = true;
                    turn = true;
 
                }

            }else if (cval < 21 && cval > 12)
            {

                if (choice.Next(0, 7) < 6)
                {
                    Console.WriteLine("Computer Hits");
                    draw(ref chand, ref cval);
                    Console.WriteLine($"Computer draws {chand.ElementAt(chand.Count - 1)})");
                }
                else
                {
                    Console.WriteLine("Computer Stays");

                    cstay = true;
                    turn = true;


                }
            }else if (cval < 21 && cval < 12)
            {
                Console.WriteLine("Computer Hits");
                
                draw(ref chand, ref cval);
                Console.WriteLine($"Computer draws {chand.ElementAt(chand.Count-1)})");
            }
        }
        else 
        {
            string inp = "";

            Console.WriteLine("You are holding: ");

            foreach (string inf in phand.Keys)
            {
                Console.WriteLine("The " + $"{inf}");
            }

            Console.WriteLine($"For a total value of: {pval}");
            Console.WriteLine("Would you like to Hit, Stay or Quit?");

            try
            {
                inp = Console.ReadLine().ToLower();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (inp == "hit" || inp == "stay" || inp == "quit")
            {
                Console.WriteLine("Reponse Test");

                if (inp == "hit" && deck.Count > 0)
                {
                    Console.WriteLine("Hit test");
                    draw(ref phand, ref pval);
                } else if (inp == "hit" && deck.Count == 0)
                {
                    Console.WriteLine("Reshuffling");
                    shuffle();
                    draw(ref phand, ref pval);
                } else if (inp == "stay")
                {
                    turn = false;
                    pstay = true;

                } else if (inp == "quit")
                {
                    gstate = false;
                }                         
            }
            else
            {
                throw new ArgumentException("Incorrect response, please try again");
            }
            
        }                      
    }

    public void play()
    {
        
        while(gstate == true)
        {
            //Console.WriteLine("Play test");

            if (phand.Count < 2 || chand.Count < 2)
            {
                if (deck.Count > 0 && phand.Count < 2)
                {
                    draw(ref phand, ref pval);
                }else if (deck.Count > 0 && chand.Count < 2)
                {
                    draw(ref chand, ref cval);
                }
                else
                {
                    Console.WriteLine("Reshuffling");
                    shuffle();
                }
            }else if(phand.Count >= 2 && chand.Count >= 2 && deck.Count == 0)
            {
                Console.WriteLine("Reshuffling");
                shuffle();
            }

            else if (cval == 21)
            {
                Console.WriteLine($"Computer Wins with {cval}");
                clear();
                

            } else if (pval == 21)
            {
                Console.WriteLine($"Player Wins with {pval}");
                clear();
                
  

            }else if (cval > 21)
            {
                Console.WriteLine($"Computer has gone over with {cval}");

                clear();
                
            }
            else if (pval > 21)
            {
                Console.WriteLine($"Player has gone over with {pval}");
     
                clear();
                
            }
            else if(pstay == true && cstay == true)
            {
                if (pval > cval)
                {
                    Console.WriteLine($"Player wins with {pval}");
                    pstay = false;
                    cstay = false;
                    
                    clear();
                }
                else
                {
                    Console.WriteLine($"Computer Wins with {cval}");
                    pstay = false;
                    cstay = false;
                    
                    clear();
                }
            }

            else
            {
                Logic();
            }
        }
    }  

    static void Main()
        {

        GameObj mydeck = new();
        mydeck.play();
        
    }
    }
