using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Granata
{
    class Program
    {
        //Config parameters here:
        static int maxTurnCount { get; set; }
        static int playerCount { get; set; }
        static int supplyCooldown { get; set; }
        public static int stageSize { get; set; }

        //Max values for config parameters:
        static int maxTurnCountMax = 60;
        static int playerCountMax = 4;
        static int supplyCooldownMax = 61;
        static int stageSizeMax = 35;
        static int stageSizeMin = 15;
        static string filePath { get; } = "config.txt";

        static void Main(string[] args)
        {
            InitConfig();
            ReadConfig();
            bool done = false;
            while (!done)
            {
               string input = Methods.IntroSound().ToUpper();
                switch (input)
                {
                    case "\r":
                        Game();
                        break;
                    case "A":
                        UpdateConfig();
                        break;
                    case "B":
                        done = true;
                        break;
                }
            }
            System.Console.WriteLine("\n\n\n\n\n\n                                     🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥");
            System.Console.WriteLine("\n\n\n\n                                       🟥 ⚒️  🟥   Thanks for playing   🟥 ⚒️  🟥\n\n\n\n");
            System.Console.WriteLine("                                     🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥\n\n\n\n\n\n");
        }

        static void InitConfig()
        {
            bool fileExists = File.Exists(filePath);
            if (fileExists)
            {
                return;
            }
            maxTurnCount = 30;
            playerCount = 2;
            supplyCooldown = 10;
            stageSize = 30;
            System.Console.WriteLine("                       ❌❌❌❌❌❌  Config.txt file doesn't exist, creating one...  ❌❌❌❌❌❌");
            WriteConfig(maxTurnCount, playerCount, supplyCooldown, stageSize);
        }

        static void UpdateConfig()
        {
            int temp = maxTurnCount;
            System.Console.WriteLine("\n\n\n                       🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣");
            System.Console.WriteLine("                       🟣   Update the following settings  🟣");
            System.Console.WriteLine("                       🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣🟣\n");
            System.Console.WriteLine("\n                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸");
            System.Console.WriteLine("                       🔸  Maximum number of turns: (default is 3️⃣ 0️⃣ )  🔸");
            System.Console.WriteLine("                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸\n");
            Methods.ValidateConfigInput(ref temp, 30, maxTurnCountMax);
            maxTurnCount = temp;
            System.Console.WriteLine("\n                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸");
            System.Console.WriteLine("                       🔸  Number of players (up to 4️⃣  default is 2️⃣ )  🔸");
            System.Console.WriteLine("                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸\n");
            Methods.ValidateConfigInput(ref temp, 2, playerCountMax);
            playerCount = temp;
            System.Console.WriteLine("\n                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸");
            System.Console.WriteLine("                       🔸  Number of turns between each projectile supply (default is 1️⃣ 0️⃣ )  🔸");
            System.Console.WriteLine("                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸\n");
            Methods.ValidateConfigInput(ref temp, 10, supplyCooldownMax);
            supplyCooldown = temp;
            System.Console.WriteLine("\n                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸");
            System.Console.WriteLine("                       🔸  Size of stage, in spaces (default is 3️⃣ 0️⃣ )  🔸");
            System.Console.WriteLine("                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸\n");
            Methods.ValidateConfigInput(ref temp, 30, stageSizeMax);
            if(temp > stageSizeMin) stageSize = temp;
            else stageSize = stageSizeMin;
            ClearConfig();
            WriteConfig(maxTurnCount, playerCount, supplyCooldown, stageSize);
            System.Console.WriteLine("\n                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸");
            System.Console.WriteLine("                       🔸  Config updated successfully ! 🔸");
            System.Console.WriteLine("                       🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸🔸\n");
            System.Threading.Thread.Sleep(3000);
        }
        static void ClearConfig()
        {
            File.Delete(filePath);
        }
        static void ReadConfig()
        {
            using (var sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        var split = line.Split("=");
                        var selector = split[0].Trim();
                        switch (selector)
                        {
                            case "maxTurnCount":
                                maxTurnCount = int.Parse(split[1].Trim());
                                break;
                            case "playerCount":
                                playerCount = int.Parse(split[1].Trim());
                                break;
                            case "supplyCooldown":
                                supplyCooldown = int.Parse(split[1].Trim());
                                break;
                            case "stageSize":
                                stageSize = int.Parse(split[1].Trim());                              
                                break;
                        }
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }

                }
            }

        }

        static void WriteConfig(int maxTurnCount, int playerCount, int supplyCooldown, int stageSize)
        {
            try
            {
                //Pass the filepath to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine($"maxTurnCount = {maxTurnCount}");
                sw.WriteLine($"playerCount = {playerCount}");
                sw.WriteLine($"supplyCooldown = {supplyCooldown}");
                sw.WriteLine($"stageSize = {stageSize}");
                //Close the file
                sw.Close();
                System.Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("                       🔸🔸🔸🔸  Exception: " + e.Message + "  🔸🔸🔸🔸\n");
            }
        }
        static void Game()
        {
            Stage.gridSize = stageSize;           
            Stage.InitializePlayer(playerCount);
            Stage.InitializeObstacule();//This sets property selectionOfObstacle
            Stage.SetListObstacle();
            Stage.RandomSetPosition();
            Stage.RenderGrid();

            for (int turnCounter = 0; turnCounter < maxTurnCount; turnCounter++)
            {
                if (turnCounter % supplyCooldown == 0) //Each 10 turns we refill, we count turn 0 as also one were we supply the projectiles 
                {
                    Methods.supplyProjectiles();

                }
                Methods.PlayerTurn(turnCounter % playerCount);
                Stage.RenderGrid();
                if (CheckWinner()) break;
            }
            CleanBoard();
            
        }
        static bool CheckWinner()
        {
            int aliveCount = 0;
            string alive = "";
            foreach (var player in Stage.players)
            {
                if (player.HP <= 0)
                {
                    continue;
                }
                aliveCount += 1;
                alive = player.Symbol;

            }
            if (aliveCount == 1)
            {
                Console.Clear();
                string spacestr = "                                           ";
                System.Console.WriteLine($"\n\n\n{spacestr} 🟨🟨🟨🟨🟨🟨🟨🟨🟨🟨🟨");
                System.Console.WriteLine($"{spacestr} 🟨                  🟨");
                System.Console.WriteLine($"{spacestr} 🟨  👑              🟨");
                System.Console.WriteLine($"{spacestr} 🟨  {alive} won!  🍾🏆   🟨");
                System.Console.WriteLine($"{spacestr} 🟨                  🟨");
                System.Console.WriteLine($"{spacestr} 🟨🟨🟨🟨🟨🟨🟨🟨🟨🟨🟨\n\n\n");
                string[] trophySymbols={"🥈", "🥉", "💩"};

                System.Console.WriteLine($"{spacestr}🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫");
                System.Console.WriteLine($"{spacestr}🟫                    🟫");

                for (int i = 0; i < Stage.deadList.Count;i++)    
                {
                    System.Console.WriteLine($"{spacestr}🟫     {Stage.deadList[i]} ➖➖ {trophySymbols[i]}     🟫");
                }
                System.Console.WriteLine($"{spacestr}🟫                    🟫");
                System.Console.WriteLine($"{spacestr}🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫\n\n\n");
                
                Player.Sound("Victory.wav");
                return true;
            }
            return false;
        }
        static void CleanBoard()
        {
            Stage.players.Clear();
            Stage.deadList.Clear();
            Stage.objectObstacleList.Clear();
            Stage.objectMinesList.Clear();
        }

    }
}

