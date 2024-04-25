using System;
using System.Collections.Generic;

namespace Snake
{
    class SnakeGame
    {
        private static int WindowHeight = 16;
        private static int WindowWidth = 32;
        private static int Score = 5;
        private static int GameOver = 0;
        private static string Movement = "RIGHT";
        private static List<int> XPosLijf = new List<int>();
        private static List<int> YPosLijf = new List<int>();
        private static DateTime lastMoveTime = new DateTime();

        static void Main(string[] args)
        {
            SetupConsole();

            Random randomNummer = new Random();
            Pixel hoofd = new Pixel
            {
                Xpos = Console.WindowWidth / 2,
                Ypos = Console.WindowHeight / 2,
                Schermkleur = ConsoleColor.Red
            };

            int berryX = randomNummer.Next(0, Console.WindowWidth);
            int berryY = randomNummer.Next(0, Console.WindowHeight);

            lastMoveTime = DateTime.Now;
            string buttonPressed = "no";

            while (true)
            {
                DrawGameArea();
                HandleCollision(ref hoofd, ref berryX, ref berryY, randomNummer);

                if (GameOver == 1)
                {
                    break;
                }

                HandleInput(ref buttonPressed);

                MoveSnake(ref hoofd);

                if (XPosLijf.Count > Score)
                {
                    XPosLijf.RemoveAt(0);
                    YPosLijf.RemoveAt(0);
                }

                System.Threading.Thread.Sleep(50);
            }

            DisplayGameOver();
        }

        private static void SetupConsole()
        {
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;
        }

        private static void DrawGameArea()
        {
            Console.Clear();
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        private static void HandleCollision(ref Pixel hoofd, ref int berryX, ref int berryY, Random randomNummer)
        {
            if (hoofd.Xpos == Console.WindowWidth - 1 || hoofd.Xpos == 0 ||
                hoofd.Ypos == Console.WindowHeight - 1 || hoofd.Ypos == 0)
            {
                GameOver = 1;
            }

            if (berryX == hoofd.Xpos && berryY == hoofd.Ypos)
            {
                Score++;
                berryX = randomNummer.Next(1, Console.WindowWidth - 2);
                berryY = randomNummer.Next(1, Console.WindowHeight - 2);
            }

            for (int i = 0; i < XPosLijf.Count; i++)
            {
                if (XPosLijf[i] == hoofd.Xpos && YPosLijf[i] == hoofd.Ypos)
                {
                    GameOver = 1;
                }
            }
        }

        private static void HandleInput(ref string buttonPressed)
        {
            DateTime currentTime = DateTime.Now;
            if (currentTime.Subtract(lastMoveTime).TotalMilliseconds > 150)
            {
                buttonPressed = "no";
                lastMoveTime = currentTime;
            }

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.UpArrow && Movement != "DOWN" && buttonPressed == "no")
                {
                    Movement = "UP";
                    buttonPressed = "yes";
                }
                if (keyPressed.Key == ConsoleKey.DownArrow && Movement != "UP" && buttonPressed == "no")
                {
                    Movement = "DOWN";
                    buttonPressed = "yes";
                }
                if (keyPressed.Key == ConsoleKey.LeftArrow && Movement != "RIGHT" && buttonPressed == "no")
                {
                    Movement = "LEFT";
                    buttonPressed = "yes";
                }
                if (keyPressed.Key == ConsoleKey.RightArrow && Movement != "LEFT" && buttonPressed == "no")
                {
                    Movement = "RIGHT";
                    buttonPressed = "yes";
                }
            }
        }

        private static void MoveSnake(ref Pixel hoofd)
        {
            XPosLijf.Add(hoofd.Xpos);
            YPosLijf.Add(hoofd.Ypos);

            switch (Movement)
            {
                case "UP":
                    hoofd.Ypos--;
                    break;
                case "DOWN":
                    hoofd.Ypos++;
                    break;
                case "LEFT":
                    hoofd.Xpos--;
                    break;
                case "RIGHT":
                    hoofd.Xpos++;
                    break;
            }
        }

        private static void DisplayGameOver()
        {
            Console.SetCursorPosition(Console.WindowWidth / 5, Console.WindowHeight / 2);
            Console.WriteLine("Game over, Score: " + Score);
            Console.SetCursorPosition(Console.WindowWidth / 5, Console.WindowHeight / 2 + 1);
        }

        class Pixel
        {
            public int Xpos { get; set; }
            public int Ypos { get; set; }
            public ConsoleColor Schermkleur { get; set; }
        }
    }
}
