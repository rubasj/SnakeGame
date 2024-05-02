using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        private static int windowHeight = 16;
        private static int windowWidth = 32;
        private static int score = 0;
        private static int snakeSize = 5;
        private static bool gameOver = false;
        private static string visualRect = "■";
        private static Pixel berry = new Pixel();
        private static Pixel snakeHead = new Pixel();
        private static List<Pixel> snakeBody = new List<Pixel>();
        private static Random randomNumber = new Random();
        private static string currMoveDirection = "RIGHT";
        private static int refreshingInterval = 300;

        static void Main(string[] args)
        {
            try
            {
                if (!Console.IsOutputRedirected)
                {
                    Console.WindowHeight = windowHeight;
                    Console.WindowWidth = windowWidth;
                }

                initSnake();
                spawnBerry();
                gameLoop();
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("Unable to set window size due to redirection or other issues.");
            }
        }

        private static void gameLoop()
        {
            DateTime lastUpdate = DateTime.Now;
           
            while (!gameOver)
            {
                Console.Clear();
                string buttonPressed = "no";
                checkGameStatus();
                drawGameArea();
                checkBerryStatus();
                drawSnakeBody();
                displayBerry();

                updateSnakePosition();
                
                if (!Console.IsInputRedirected && Console.KeyAvailable)
                {
                    handleInput(ref buttonPressed);
                }

                Thread.Sleep(refreshingInterval);
            }

            displayGameOver();
        }

        private static void initSnake()
        {
            snakeHead.posX = windowWidth / 2;
            snakeHead.posY = windowHeight / 2;
        }

        private static void spawnBerry()
        {
            berry.posX = randomNumber.Next(1, windowWidth - 2);
            berry.posY = randomNumber.Next(1, windowHeight - 2);
        }

        private static void drawGameArea()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < windowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(visualRect);
                Console.SetCursorPosition(i, windowHeight - 1);
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
            // check colision with boarders
            if (snakeHead.posX == windowWidth - 1
                || snakeHead.posX == 0
                || snakeHead.posY == windowHeight - 1
                || snakeHead.posY == 0)
            {
                gameOver = true;
            }
            // check colision head and body
            for (int i = 1; i < snakeBody.Count(); i++) {
                if (snakeBody[0].posX == snakeBody[i].posX && snakeBody[0].posY == snakeBody[i].posY) {
                    gameOver = true;
                }
            }
        }

        private static void drawSnakeBody()
        {

            for(int i = 0; i < snakeBody.Count(); i++)
            {
                Console.SetCursorPosition(snakeBody[i].posX, snakeBody[i].posY);
                if (i==0) {
                    Console.ForegroundColor = ConsoleColor.Red;
                } else {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write(visualRect);
            }
        }
 
        private static void handleInput(ref string buttonPressed)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow when currMoveDirection != "DOWN" && buttonPressed == "no":
                    currMoveDirection = "UP";
                    buttonPressed = "yes";
                    break;
                case ConsoleKey.DownArrow when currMoveDirection != "UP" && buttonPressed == "no":
                    currMoveDirection = "DOWN";
                    buttonPressed = "yes";
                    break;
                case ConsoleKey.LeftArrow when currMoveDirection != "RIGHT" && buttonPressed == "no":
                    currMoveDirection = "LEFT";
                    buttonPressed = "yes";
                    break;
                case ConsoleKey.RightArrow when currMoveDirection != "LEFT" && buttonPressed == "no":
                    currMoveDirection = "RIGHT";
                    buttonPressed = "yes";
                    break;
            }
        }

        private static void updateSnakePosition()
        {
            switch (currMoveDirection)
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
            updateSnakeBody();
        }

        private static void updateSnakeBody()
        {
            Pixel newBodyPart = new Pixel { posX = snakeHead.posX, posY = snakeHead.posY};
            snakeBody.Insert(0, newBodyPart);

            if (snakeBody.Count > snakeSize)
            {
                snakeBody.RemoveAt(snakeBody.Count - 1);
            }
        }

        private static void displayBerry()
        {
            Console.SetCursorPosition(berry.posX, berry.posY);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(visualRect);
        }

        private static void displayGameOver()
        {
            Console.SetCursorPosition(windowWidth / 5, windowHeight / 2);
            Console.WriteLine("Game Over, Score: " + score);
            Console.SetCursorPosition(windowWidth / 5, windowHeight / 2 + 1);
        }

        private static void checkBerryStatus()
        {
            if (berry.posX == snakeHead.posX && berry.posY == snakeHead.posY)
            {
                snakeSize++;
                score++;
                spawnBerry();
            }
        }

        class Pixel
        {
            public int posX { get; set; }
            public int posY { get; set; }
            
        }
    }
}
