using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            CheeseNibbler myCheese = new CheeseNibbler();
            myCheese.PlayGame();
        }
    }


    public class Point
    {
        public enum PointStatus
        {
            Empty,Cheese,Mouse
        }
        public int X { get; set; }
        public int Y { get; set; }

        public PointStatus Status { get; set; }

        public Point(int x,int y)
        {
            this.X = x;
            this.Y = y;
            this.Status = PointStatus.Empty;
        }
    }

    public class CheeseNibbler
    {
        public Point[,] Grid { get; set; }
        public Point Mouse { get; set; }
        public Point Cheese { get; set; }
        public int Round { get; set; }

        public CheeseNibbler()
        {
            int x, y;
            //initialize the Grid
            this.Grid = new Point[10, 10];
            for (y = 0; y < 10; y++)
            {
                for (x = 0; x < 10; x++)
                {
                    Grid[x, y] = new Point(x, y);  //all the points have status=Empty
                }
            }

            //initialize the Mouse
            Random gnr = new Random();
            x = gnr.Next(0, 10);
            y = gnr.Next(0, 10);

            //select a random point into the Grid
            this.Mouse = new Point(x,y);
            this.Mouse.Status = Point.PointStatus.Mouse;

            //put mouse in the grid
            this.Grid[x, y].Status = Mouse.Status;

            //be sure to use a different coordinate for x
            int xCheese;
            do
            {
                xCheese = gnr.Next(0, 10);
            }
            while (xCheese == x);

            int yCheese = gnr.Next(0, 10);

            //initialize the Cheese
            this.Cheese = new Point(xCheese, yCheese);
            this.Cheese.Status = Point.PointStatus.Cheese;

            //put cheese in the grid
            this.Grid[xCheese,yCheese].Status=this.Cheese.Status;

        }

        public void DrawGrid()
        {
            Console.Clear();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Point myPoint = this.Grid[x, y];
                    if (myPoint.Status == Mouse.Status)
                    {
                        Console.Write("[M]");
                    }
                    else if (myPoint.Status == Cheese.Status)
                    {
                        Console.Write("[C]");
                    }
                    else
                    {
                        Console.Write("[ ]");
                    }
                }
                Console.WriteLine();
            }
        }
        ConsoleKeyInfo input; 
        //if the input is valid call ValidMove() to check if the new position is still inside the Grid
        public ConsoleKey GetUserMove()
        {
            bool validMove = false;
            
            while (!validMove)
            {
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.LeftArrow || input.Key == ConsoleKey.RightArrow || input.Key == ConsoleKey.DownArrow || input.Key == ConsoleKey.UpArrow)
                {
                    if (ValidMove(input.Key))
                    {
                        validMove = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid move");
                    
                }

            }
                                      
            return input.Key;
        }


        //change mouse position if the move is not outside the grid
        public bool ValidMove(ConsoleKey input)
        {
            int mousePositionX = Mouse.X;
            int mousePositionY = Mouse.Y;

            switch (input)
            {
                case ConsoleKey.LeftArrow:
                    if (mousePositionX == 0)
                    {
                        Console.WriteLine( "Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (mousePositionX == 9)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (mousePositionY == 0)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }                   
                    break;
                case ConsoleKey.DownArrow:
                    if (mousePositionY == 9)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }                   
                    break;
            }
            return true;
        }

        //Return true if the cheese was found
        public bool MoveMouse(ConsoleKey input)
        {
            //save the original position of the Mouse
            int previousPositionX = Mouse.X;
            int previousPositionY = Mouse.Y;

            //change coordinates of the Mouse to move position
            switch (input)
            {
                case ConsoleKey.LeftArrow: Mouse.X -= 1;
                    break;
                case ConsoleKey.RightArrow: Mouse.X += 1;
                    break;
                case ConsoleKey.DownArrow: Mouse.Y += 1;
                    break;
                case ConsoleKey.UpArrow: Mouse.Y -= 1;
                    break;
            }
            
            //check if the point in the Grid has the cheese
            Point checkForCheese = this.Grid[Mouse.X, Mouse.Y];
            if (checkForCheese.Status == Cheese.Status)
            {
                return true;
            }
            else
            {
                //set previous point status to Empty
                this.Grid[previousPositionX, previousPositionY].Status = Point.PointStatus.Empty;

                //set the new position status to Mouse
                this.Grid[Mouse.X, Mouse.Y].Status = Point.PointStatus.Mouse;
                return false;
            }
        }

        public void PlayGame()
        {
            bool found = false;

            while (!found)
            {
                DrawGrid();
                ConsoleKey userMove = GetUserMove();
                if (MoveMouse(userMove))
                {
                    found = true;
                    break;
                }
                this.Round++;
            }
        }
    }


}
