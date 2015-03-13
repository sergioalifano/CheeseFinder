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

            Console.ReadKey();
        }
    }


    public class Point
    {
        public enum PointStatus
        {
            Empty,Cheese,Mouse,Obstacle
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
        public Point Obstacle { get; set; }
        public int Round { get; set; }

        public CheeseNibbler()
        {
            int x, y;
            //initialize the Grid
            this.Grid = new Point[10, 10];
            for (y = 0; y < this.Grid.GetLength(1); y++)
            {
                for (x = 0; x < this.Grid.GetLength(0); x++)
                {
                    Grid[x, y] = new Point(x, y);  
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

            //create a position for the obstacle
            do
            {
                x = gnr.Next(0, 10);
                y = gnr.Next(0, 10);
            }
            while (this.Cheese.X == x || this.Mouse.X ==x);

            // initalize the Obstacle 
            this.Obstacle = new Point(x, y);

            //put the obstacle into the grid
            this.Obstacle.Status = Point.PointStatus.Obstacle;
            this.Grid[x, y].Status = this.Obstacle.Status;

        }

        public void DrawGrid()
        {
            Console.Clear();
            Console.WriteLine("Use the numeric keypad to move\n");
           // string mouse = "\u260e";
         //   Console.WriteLine(mouse);
            for (int y = 0; y < this.Grid.GetLength(1); y++)
            {
                for (int x = 0; x < this.Grid.GetLength(0); x++)
                {
                    Point myPoint = this.Grid[x, y];
                    if (myPoint.Status == Mouse.Status)
                    {
                        Console.Write("[ M ]");
                    }
                    else if (myPoint.Status == Cheese.Status)
                    {
                        Console.Write("[ ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("C");
                        Console.ResetColor();
                        Console.Write(" ]");
                    }
                    else if (myPoint.Status == Obstacle.Status)
                    {
                        Console.Write("[ | ]");
                    }
                    else
                    {
                        Console.Write("[   ]");
                    }
                }
                Console.WriteLine();
            }
        }
        ConsoleKeyInfo input; 

        /// <summary>
        /// if the input is valid call ValidMove() to check if the new position is still inside the Grid
        /// </summary>
        /// <returns></returns>
        public ConsoleKey GetUserMove()
        {
            bool validMove = false;
            
            while (!validMove)
            {
                input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.NumPad1 || input.Key == ConsoleKey.NumPad4 || input.Key == ConsoleKey.NumPad7 || input.Key == ConsoleKey.NumPad8 || input.Key == ConsoleKey.NumPad9 || input.Key == ConsoleKey.NumPad6 || input.Key == ConsoleKey.NumPad3 || input.Key == ConsoleKey.NumPad2)
                {
                    if (ValidMove(input.Key))
                    {
                        validMove = true;
                    }
                }
                else {Console.WriteLine("Invalid move");}
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
                case ConsoleKey.NumPad1:
                    if (mousePositionX == 0 || mousePositionY == 9) 
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad4:
                    if (mousePositionX == 0)
                    {
                        Console.WriteLine( "Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad7:
                    if (mousePositionX == 0 || mousePositionY == 0) 
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad8:
                     if (mousePositionY == 0)
                    {
                          Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }                   
                    break;
                case ConsoleKey.NumPad9:
                     if (mousePositionX == 9 || mousePositionY == 0) 
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad6:
                    if (mousePositionX == 9)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad3:
                    if (mousePositionX == 9 || mousePositionY == 9) 
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad2:
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
                case ConsoleKey.NumPad1: { Mouse.X -= 1; Mouse.Y += 1; }
                    break;
                case ConsoleKey.NumPad4: { Mouse.X -= 1; }
                    break;
                case ConsoleKey.NumPad7: { Mouse.X -= 1; Mouse.Y -= 1; }
                    break;
                case ConsoleKey.NumPad8: { Mouse.Y -= 1; }
                    break;
                case ConsoleKey.NumPad9: { Mouse.X += 1; Mouse.Y -= 1; }
                    break;
                case ConsoleKey.NumPad6: { Mouse.X += 1;}
                    break;
                case ConsoleKey.NumPad3: { Mouse.X += 1; Mouse.Y += 1; }
                    break;
                case ConsoleKey.NumPad2: { Mouse.Y += 1; }
                    break;

            }

            //check if the point in the Grid has the cheese
            Point checkForCheese = this.Grid[Mouse.X, Mouse.Y];

            this.Grid[previousPositionX, previousPositionY].Status = Point.PointStatus.Empty;

            if (checkForCheese.Status == Cheese.Status)
            {                
                this.Grid[Mouse.X, Mouse.Y].Status = Mouse.Status;
                return true;
            }
            else
            {                            
                //set the new position status to Mouse
                this.Grid[Mouse.X, Mouse.Y].Status = Mouse.Status;
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
            DrawGrid();
            Console.WriteLine("It took you {0} round to get the cheese",this.Round);
        }
    }


}
