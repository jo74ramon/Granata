using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Media;
using NAudio.Wave;
namespace Granata
{
    public class Player
    {

        //crear diccionario de tipos de armas tipo:cantidad
        public string Symbol { get; set; }
        public int HP { get; set; }
        public int[] Position { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Actions { get; set; }

        public Dictionary<string, int> Projectiles { get; set; }

        public Player(string symbol, int hp, string name, int score, int[] position, Dictionary<string, int> projectiles)
        {
            this.Symbol = symbol;
            this.HP = hp;
            this.Position = position;
            this.Name = name;
            this.Score = score;
            this.Projectiles = projectiles;
        }

        public void Move(char direction, int playerN)
        {
            System.Console.WriteLine(direction);
            System.Console.WriteLine(playerN);
            //validar si choco con mina, obstaculo y eje de mapa
            switch (direction)
            {
                case 'W':
                    if (Stage.CheckObstacles(Stage.players[playerN].Position[0], Stage.players[playerN].Position[1] - 1))
                    {
                        return;
                    }
                    if (Stage.players[playerN].Position[1] > 0)
                    {
                        Stage.players[playerN].Position[1]--;
                    }

                    break;
                case 'A':
                    if (Stage.CheckObstacles(Stage.players[playerN].Position[0] - 1, Stage.players[playerN].Position[1]))
                    {
                        return;
                    }
                    if (Stage.players[playerN].Position[0] > 0)
                    {
                        Stage.players[playerN].Position[0]--;
                    }
                    break;
                case 'S':
                    if (Stage.CheckObstacles(Stage.players[playerN].Position[0], Stage.players[playerN].Position[1] + 1))
                    {
                        return;
                    }
                    if (Stage.players[playerN].Position[1] < Stage.gridSize - 1) Stage.players[playerN].Position[1]++;
                    break;
                case 'D':
                    if (Stage.CheckObstacles(Stage.players[playerN].Position[0] + 1, Stage.players[playerN].Position[1]))
                    {
                        return;
                    }
                    if (Stage.players[playerN].Position[0] < Stage.gridSize - 1) Stage.players[playerN].Position[0]++;
                    break;
            }
            collitionMine(playerN);
            Stage.RenderGrid();
        }

        public void collitionMine(int playerN)
        {
            if (Stage.CheckMines(Stage.players[playerN].Position[0], Stage.players[playerN].Position[1]))
            {
                Stage.players[playerN].HP -= Stage.objectMinesList[0].Damage;
                if (Stage.players[playerN].HP <= 0) 
                {
                    Stage.players[playerN].Position[0] = 200;//Remove the players from the field if they die
                    Stage.deadList.Insert(0, Stage.players[playerN].Symbol);
                }
                for (int i = 0; i < Stage.objectMinesList.Count; i++)
                {
                    if (Stage.objectMinesList[i].ProjectilePosition[0] == Stage.players[playerN].Position[0] && Stage.objectMinesList[i].ProjectilePosition[1] == Stage.players[playerN].Position[1])
                    {
                        Stage.objectMinesList.RemoveAt(i);
                        Sound("Bum.wav");
                        return;
                    }

                }
            }

        }

        public void ShowInventory(int playerN)
        {
            string[] number = {"0️⃣","1️⃣","2️⃣","3️⃣","4️⃣","5️⃣","6️⃣","7️⃣","8️⃣","9️⃣"};
            string hpStr = $"{Stage.players[playerN].HP}";
            char[] hpChar = hpStr.ToCharArray();
            string hpPrint = "";

            string offsetTitle= " ";
            for (int i = 0; i < Stage.gridSize + 12; i ++)
            {
                offsetTitle+= " ";
            }   

            foreach (char c in hpChar)
            {
                for (int i = 0; i < number.Length; i++)
                {
                    if (int.Parse(c.ToString()) == i)
                    {
                        hpPrint += " " + number[i];
                    }
                }
            }

            if (hpPrint.Length != 12)
            {
                if(hpPrint.Length == 8)
                {
                    hpPrint = " 0️⃣" + hpPrint;
                }
                else if(hpPrint.Length == 4)
                {
                    hpPrint = " 0️⃣ 0️⃣" + hpPrint;
                }
            }

            Console.WriteLine($"\n{offsetTitle}🔷🔷🔷🔷🔷🔷🔷🔷🔷🔷🔷🔷");
            System.Console.WriteLine($"{offsetTitle}🔷  ❤️  ➖➖➖{hpPrint}   🔷");
            string[] types = { "⚾", "⛔", "💠","📀" };
            foreach (var key in Stage.players[playerN].Projectiles.Keys)
            {
                string projStr = $"{Stage.players[playerN].Projectiles[key]}";
                char[] projChar = projStr.ToCharArray();
                string projPrint = "";

                foreach (char c in projChar)
                {
                    for (int i = 0; i < number.Length; i++)
                    {
                        if (int.Parse(c.ToString()) == i)
                        {
                            projPrint += " " + number[i];
                        }
                    }
                }

                if (projPrint.Length != 8)
                {

                    projPrint = " 0️⃣" + projPrint;
                }

                System.Console.WriteLine($"{offsetTitle}🔷  {Stage.players[playerN].Symbol} has {types[int.Parse(key) - 1]}: {projPrint}   🔷");
            }
            Console.WriteLine($"{offsetTitle}🔷🔷🔷🔷🔷🔷🔷🔷🔷🔷🔷🔷\n");
        }

        public bool CheckProjectileAvailible(string type, int playerN)
        {
            //validation player still has projectiles types left    
            foreach (var proj in this.Projectiles)
            {
                if (Stage.players[playerN].Projectiles[type] > 0)
                {
                    Stage.players[playerN].Projectiles[type]--;
                    return true;
                }
                else
                {
                    System.Console.WriteLine($"{Methods.offset_title}❌❌❌❌  Not enough projectiles of that type❗  ❌❌❌❌\n");
                    return false;
                }
            }
            return false;
        }
        public void Throw(int direction, string type, int playerN)
        {
            int mydelay = 250;
            switch (direction)
            {
                case 1:
                    Stage.actualProjectile.ProjectilePosition[0]--;
                    Stage.actualProjectile.ProjectilePosition[1]++;
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;

                case 2:
                    Stage.actualProjectile.ProjectilePosition[1]++;
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;

                case 3:
                    Stage.actualProjectile.ProjectilePosition[0]++;
                    Stage.actualProjectile.ProjectilePosition[1]++;
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;

                case 4:
                    Stage.actualProjectile.ProjectilePosition[0]--;
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;

                case 6:
                    Stage.actualProjectile.ProjectilePosition[0]++;
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;

                case 7:
                    Stage.actualProjectile.ProjectilePosition[0]--;
                    Stage.actualProjectile.ProjectilePosition[1]--;
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;

                case 8:
                    Stage.actualProjectile.ProjectilePosition[1]--;
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;

                case 9:
                    Stage.actualProjectile.ProjectilePosition[0]++;
                    Stage.actualProjectile.ProjectilePosition[1]--;
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;
                default:
                    Stage.RenderGrid();
                    Thread.Sleep(mydelay);
                    break;
            }
        }

        public void Refill()
        {
            Projectiles["1"] = 10;
            Projectiles["2"] = 5;
            Projectiles["3"] = 5;
            Projectiles["4"] = 1;
        }

        public bool plantMine(int[] pos)
        {
            if (Stage.actualProjectile.Tipo != "3")
                return false;
            Stage.objectMinesList.Add(Stage.actualProjectile);
            //prob mal
            return true;
        }
        public bool grenadeImpact()
        {
            if (Stage.actualProjectile.Tipo != "2")
                return false;
            int projX = Stage.actualProjectile.ProjectilePosition[0];
            int projY = Stage.actualProjectile.ProjectilePosition[1];

            for (int x = projX - 1; x < projX + 2; x++) //PRIMER FOR
            {
                for (int y = projY - 1; y < projY + 2; y++) //SEGUNDO FOR
                {
                    foreach (var player in Stage.players)
                    {
                        if (player.Position[0] == projX && player.Position[1] == projY)
                        {
                            continue;
                        }
                        else if (player.Position[0] == x && player.Position[1] == y)
                        {
                            player.HP -= Stage.actualProjectile.SplashDamage;
                            if (player.HP <= 0)
                            {
                                player.Position[0] = 200;//Remove the players from the field if they die
                                Stage.deadList.Insert(0, player.Symbol);
                            } 
                            return true;
                        }
                    }
                }
            }
            return true; //not found
                         //returns 1 for direct hit and 2 for splash damage
        }

        public static (int, bool) Collision(int dir)
        {
            bool playerCollision = false;
            foreach (var player in Stage.players)
            {
                int projX = Stage.actualProjectile.ProjectilePosition[0];
                int projY = Stage.actualProjectile.ProjectilePosition[1];

                string offsetTitle= " ";
                for (int i = 0; i < Stage.gridSize + 12; i ++)
                {
                    offsetTitle+= " ";
                }   
                //direct hit
                if (projX == player.Position[0] && projY == player.Position[1])
                {
                    System.Console.WriteLine($"\n\n{offsetTitle}❌❌❌❌❌❌❌❌❌❌❌❌");
                    Console.WriteLine($"{offsetTitle}❌{player.Symbol} was hit❗ Lost 💔❌");
                    System.Console.WriteLine($"{offsetTitle}❌❌❌❌❌❌❌❌❌❌❌❌\n");
                    player.HP -= Stage.actualProjectile.Damage;
                    playerCollision = true;
                    if (player.HP <= 0)
                    {
                        Player.Sound("Hit.wav");
                        player.Position[0] = 200;
                        Stage.deadList.Insert(0, player.Symbol);
                    }
                    string[] audios = { "Bonk.wav", "Bum.wav", "Bum.wav","Fatality.wav" };
                    Sound(audios[int.Parse(Stage.actualProjectile.Tipo) - 1]);
                    return (dir, playerCollision);

                }
            }
            dir = ChangeDirection(dir);// check collision with obstacule
            return (dir, playerCollision);
        }
        static int ChangeDirection(int dir)
        {

            int NextCoordenateProyectileX = 0;
            int NextCoordenateProyectileY = 0;
            (NextCoordenateProyectileX, NextCoordenateProyectileY) = NextCoordenate(dir);

            if (Stage.actualProjectile.ProjectilePosition[0] < 1)
            {
                Console.Beep();
                if (Stage.actualProjectile.ProjectilePosition[1] < 1 && dir == 7)
                    return 3; // upper left corner
                if (Stage.actualProjectile.ProjectilePosition[1] > Stage.gridSize - 2 && dir == 1)
                    return 9; // down left corner
                if (dir == 7)
                    return 9; // collision wall left
                if (dir == 4)
                    return 6; // collision wall left
                if (dir == 1)
                    return 3; // collision wall left
            }
            else if (Stage.actualProjectile.ProjectilePosition[0] > Stage.gridSize - 2)
            {
                Console.Beep();
                if (Stage.actualProjectile.ProjectilePosition[1] < 1 && dir == 9)
                    return 1; // upper right corner
                if (Stage.actualProjectile.ProjectilePosition[1] > Stage.gridSize - 2 && dir == 3)
                    return 7; // down right corner
                if (dir == 9)
                    return 7; // collision wall right
                if (dir == 6)
                    return 4; // collision wall right
                if (dir == 3)
                    return 1; // collision wall right
            }

            if (Stage.actualProjectile.ProjectilePosition[1] < 1)
            {
                Console.Beep();
                if (dir == 7)
                    return 1; // collision wall up
                if (dir == 8)
                    return 2; // collision wall up
                if (dir == 9)
                    return 3; // collision wall up
            }
            else if (Stage.actualProjectile.ProjectilePosition[1] > Stage.gridSize - 2)
            {
                Console.Beep();
                if (dir == 1)
                    return 7; // collision wall down
                if (dir == 2)
                    return 8; // collision wall down
                if (dir == 3)
                    return 9; // collision wall down
            }
            //check obstacules collisions




            else if (Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY))
            {
                int index = Stage.ObstacleCollision(NextCoordenateProyectileX, NextCoordenateProyectileY);
                Obstacle.HitObstacle(index);
                Console.Beep();
                if (dir == 1 && (Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY + 1)) && !(Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY - 1)))
                    return 7; // collision wall down
                if (dir == 2)
                    return 8; // collision wall down
                if (dir == 3 && (Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY + 1)) && !(Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY - 1)))
                    return 9; // collision wall down

                if (dir == 7 && (Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY - 1)) && !(Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY + 1)))
                    return 1; // collision wall up
                if (dir == 8)
                    return 2; // collision wall up
                if (dir == 9 && (Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY - 1)) && !(Stage.CheckObstacles(NextCoordenateProyectileX, NextCoordenateProyectileY + 1)))
                    return 3; // collision wall up

                if (dir == 9)
                    return 7; // collision wall right
                if (dir == 6)
                    return 4; // collision wall right
                if (dir == 3)
                    return 1; // collision wall right

                if (dir == 7)
                    return 9; // collision wall left
                if (dir == 4)
                    return 6; // collision wall left
                if (dir == 1)
                    return 3; // collision wall left  
            }
            return dir;
        }
        static (int, int) NextCoordenate(int dir)
        {
            int NextCoordenateX = Stage.actualProjectile.ProjectilePosition[0];
            int NextCoordenateY = Stage.actualProjectile.ProjectilePosition[1];
            switch (dir)
            {
                case 1:
                    NextCoordenateX--;
                    NextCoordenateY++;
                    break;
                case 2:
                    NextCoordenateY++;
                    break;
                case 3:
                    NextCoordenateX++;
                    NextCoordenateY++;
                    break;

                case 4:
                    NextCoordenateX--;
                    break;

                case 6:
                    NextCoordenateX++;
                    break;

                case 7:
                    NextCoordenateX--;
                    NextCoordenateY--;
                    break;

                case 8:
                    NextCoordenateY--;
                    break;

                case 9:
                    NextCoordenateX++;
                    NextCoordenateY--;
                    break;

                default:
                    break;
            }
            return (NextCoordenateX, NextCoordenateY);
        }
        public static void Sound(string FileName)
        {
            // Ruta al archivo de audio
            //string[] audios = { "Bonk.wav", "Bum.wav", "Bum.wav" };
            string audioFilePath = FileName;

            // Crea un objeto WaveOut para la reproducción de audio
            using (var waveOut = new WaveOutEvent())
            {
                // Crea un objeto WaveFileReader para leer el archivo de audio
                using (var audioFileReader = new WaveFileReader(audioFilePath))
                {
                    // Asigna el objeto WaveFileReader al WaveOut
                    waveOut.Init(audioFileReader);

                    // Reproduce el audio
                    waveOut.Play();

                    // Espera a que se termine la reproducción
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }
    }
}
