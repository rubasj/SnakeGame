using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.ComponentModel;
///█ ■
////https://www.youtube.com/watch?v=SGZgvMwjq2U
namespace Snake
{
    class Program
    {

        private static int windowHeight = 16;
        private static int windowWidth = 32;
        private static int score = 5;
        private static int gameOver = 0;
        private static String visualRect = "■";

        /* flag for snake movement, for first direction is set to rigth */
        private static Pixel berry = new Pixel();
        private static Pixel snakeHead = new Pixel();
        private static Random randomNumber = new Random();

        private static List<Pixel> snakeBody = new List<Pixel>();
        private static List<int> xposlijf = new List<int>();
        private static List<int> yposlijf = new List<int>();
        private static string currMove = "RIGHT";

        private static void initSnake() {
            snakeHead.posX = windowWidth/2;
            snakeHead.posY = windowHeight/2;
            snakeHead.color = ConsoleColor.Red;
        }

        /*
        The method set a new random position of the berry.
        */
        private static void setBerryPosition() {
            berry.posX = randomNumber.Next(1, windowWidth-2);
            berry.posY = randomNumber.Next(1, windowHeight-2);
        }

        private static void drawGameArea()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0;i< windowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(visualRect);
                Console.SetCursorPosition(i, windowHeight -1);
                Console.Write(visualRect);
            }
            for (int i = 0; i < windowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(visualRect);
                Console.SetCursorPosition(windowWidth - 1, i);
                Console.Write(visualRect);
            }
        }

        private static void checkGameStatus()
        {
            if (snakeHead.posX == windowWidth-1 || snakeHead.posX == 0 ||snakeHead.posY == windowHeight-1 || snakeHead.posY == 0)
            { 
               gameOver = 1;
            }
        }

        private static void drawSnakeBody()
        {
            Console.SetCursorPosition(snakeHead.posX, snakeHead.posY);
            Console.ForegroundColor = snakeHead.color;
            Console.Write(visualRect);

            foreach(Pixel pix in snakeBody)
            {
                Console.SetCursorPosition(pix.posX, pix.posY);
                Console.Write(visualRect);

                if (pix.posX == snakeHead.posX
                    && pix.posY == snakeHead.posY)
                {
                    gameOver = 1;
                }
            }
        }
        
        static void Main(string[] args)
        {
            Console.WindowHeight = windowHeight;
            Console.WindowWidth = windowWidth;

            initSnake();
            setBerryPosition();

            DateTime tijd = DateTime.Now;
            DateTime tijd2 = DateTime.Now;
            string buttonpressed = "no";

            do
            {
                checkGameStatus();
                drawGameArea();
                Console.ForegroundColor = ConsoleColor.Green;

                checkBerryStatus();
                drawSnakeBody();

                Console.SetCursorPosition(berry.posX, berry.posY);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(visualRect);

                tijd = DateTime.Now;
                buttonpressed = "no";
                while (gameOver != 1)
                {
                    tijd2 = DateTime.Now;
                    if (tijd2.Subtract(tijd).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo toets = Console.ReadKey(true);
                        //Console.WriteLine(toets.Key.ToString());
                        if (toets.Key.Equals(ConsoleKey.UpArrow) && currMove != "DOWN" && buttonpressed == "no")
                        {
                            currMove = "UP";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && currMove != "UP" && buttonpressed == "no")
                        {
                            currMove = "DOWN";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && currMove != "RIGHT" && buttonpressed == "no")
                        {
                            currMove = "LEFT";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && currMove != "LEFT" && buttonpressed == "no")
                        {
                            currMove = "RIGHT";
                            buttonpressed = "yes";
                        }
                    }
                }
                // TODO
                xposlijf.Add(snakeHead.posX);
                yposlijf.Add(snakeHead.posY);
                //snakeBody.Add(snakeHead);

                changeMovement();
                
                if (xposlijf.Count() > score)
                {
                    //snakeBody.RemoveAt(0);
                    xposlijf.RemoveAt(0);
                    yposlijf.RemoveAt(0);
                }
                Console.Clear();
            } while(gameOver != 1);

            Console.SetCursorPosition(windowWidth / 5, windowHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(windowWidth / 5, windowHeight / 2 +1);
        }

        private static void changeMovement()
        {
            switch (currMove)
                {
                    case "UP":
                        snakeHead.posY--;
                        break;
                    case "DOWN":
                        snakeHead.posY++;
                        break;
                    case "LEFT":
                        snakeHead.posX--;
                        break;
                    case "RIGHT":
                        snakeHead.posX++;
                        break;
                }
        }

        private static void checkBerryStatus()
        {
            if (berry.posX == snakeHead.posX && berry.posY == snakeHead.posY)
            {
                score++;
                setBerryPosition();
            } 
        }

        class Pixel
        {
            public int posX { get; set; }
            public int posY { get; set; }
            public ConsoleColor color { get; set; }
        }
    }
}