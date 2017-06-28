using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadder
{
    class Program
    {
        static void Main(string[] args)
        {
            Board game = new Board();

            game.Initialize();
            game.Start();

            while (game.IsPlaying)
            {
                while (!game.Play())
                {
                    // Keep playing until theres a winner
                    Console.Write("Press enter next turn...");
                    Console.ReadKey();

                };

                // Otherwise ask to play again
                Console.Write("Do you want to play again [y/n]?");
                string res = Console.ReadLine().ToUpper();
                while(res != "Y" && res!="N")
                {
                    Console.Write("Do you want to play again [y/n]?");
                     res = Console.ReadLine().ToUpper();

                }
                if (res == "Y")
                {
                    game.Start();
                }else
                {
                    game.IsPlaying = false;
                }
            }
            Console.WriteLine("Thank you for playing");
            Console.ReadLine();

        }
    }
    class Player
    {
        public Player()
        {
            Place = 0;
            Rolled = 0;
        }
        public int Place { get; set; }
        public int Rolled { get; set; }
        public string Status { get; set; }
    }

    class Board
    {
        private int PlayersCount;
        private int FinishLine;
        private int DieCount;
        private Random RandomGen;
        public bool IsPlaying { get; set;}

        private List<Player> Players;
        private Dictionary<int, int> SnakesOrLadders;
        public Board()
        {
            FinishLine = 10 * 10;
            Players = new List<Player>();
            SnakesOrLadders = new Dictionary<int, int>();
            RandomGen = new Random();
        }
        public void Initialize()
        {
            Console.WriteLine("=================== Snake and Ladder ====================");
            Console.WriteLine("Instruction: PRESS ENTER key to roll dice. ");
            Console.WriteLine("Whoever reach 100 first wins. Goodluck!");
            Console.WriteLine("---------------------------------------------------------");


            GenerateSnakesAndLadders();

        }
        public void GenerateSnakesAndLadders()
        {
            // Snakes
            SnakesOrLadders.Add(15, 9);
            SnakesOrLadders.Add(30, 16);
            SnakesOrLadders.Add(50, 28);
            SnakesOrLadders.Add(72, 56);
            SnakesOrLadders.Add(98, 75);


            // Ladders
            SnakesOrLadders.Add(10, 20);
            SnakesOrLadders.Add(25, 40);
            SnakesOrLadders.Add(45, 62);
            SnakesOrLadders.Add(55, 80);

        }
        public void Reset()
        {
            Players.Clear();
            for(int i = 0; i<PlayersCount; i++)
            {
                Player newPlayer = new Player();
                Players.Add(newPlayer);
            }
        }
        public void Start()
        {
            IsPlaying = true;

            int playersCount = 0;
            int dieCount = 0;

            Console.WriteLine("How many players [2-4]? ");
            while (!Int32.TryParse(Console.ReadLine(), out playersCount))
            {
                Console.WriteLine("How many players 2-4 (Numbers only)?");
            }
            if (playersCount > 4 || playersCount<2) playersCount = 2;

            Console.WriteLine("How many dice to roll [1-2]? ");
            while (!Int32.TryParse(Console.ReadLine(), out dieCount))
            {
                Console.WriteLine("How many dice to roll [1-2] (Numbers only)?");

            }
            if (dieCount > 2 || dieCount < 1) dieCount = 1;


            PlayersCount = playersCount;
            DieCount = dieCount;
            Reset();

            

        }
        public bool Play()
        {
            bool winner = false;

            foreach(Player p in Players)
            {
                p.Rolled  = RollDice();
                p.Place += p.Rolled;
                int old = p.Place;
                

                if (SnakesOrLadders.ContainsKey(p.Place))
                {
                    p.Place = SnakesOrLadders[p.Place];
                }
                if (old!=p.Place)
                {
                    p.Status = string.Format("{0}->{1}", old, p.Place);
                }
          
                if(p.Place>= FinishLine)
                {
                    p.Status = string.Concat(p.Status, "Winner!");
                    winner = true;
                }
             }
            ShowTurns();
            return winner;
        }
        public int RollDice()
        {
            int diceRoll = RandomGen.Next(1, 7);
            if (DieCount > 1)
            {
                diceRoll += RandomGen.Next(1, 7);
                
            }

            return diceRoll;
        }
        public void ShowTurns()
        {
            StringBuilder msg = new StringBuilder();
            int pl = 1;
            foreach(Player player in Players)
            {

                if (pl == 1)
                {
                    msg.AppendFormat("P{0}(you) Rolled [{1}] Place[{2}] Stat[{3}] ", pl,player.Rolled, player.Place,player.Status);

                }else
                {
                    msg.AppendFormat("P{0} Rolled [{1}] Place[{2}] Stat[{3}] ", pl, player.Rolled, player.Place, player.Status);

                }
                if (!(pl == PlayersCount))
                {
                    msg.Append(" | ");
                }
                player.Status = "";
                pl++;

            }

            Console.WriteLine(msg.ToString());
        }
    }
}
