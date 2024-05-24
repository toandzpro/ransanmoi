using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Baitap24
{
    public static int width = 40;
    public static int height = 20;
    public static int score = 0;
    public static int delay = 100;
    public static bool gameOver = false;
    private static Direction direction = Direction.Right;
    public static Random random = new Random();
    public static List<Position> snake = new List<Position>();
    public static Position food = new Position();
    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public struct Position
    {
        public int x;
        public int y;
    }

    public static void Main()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(width + 1, height + 1);
        Console.SetBufferSize(width + 1, height + 1);
        Console.Title = "Rắn Săn Mồi";
        Console.Clear();
        
        InitializeSnake();
        GenerateFood();
        Thread inputThread = new Thread(ReadInput);
        inputThread.Start();

        while (!gameOver)
        {
            MoveSnake();
            Draw();
            Thread.Sleep(delay);
        }
        Console.SetCursorPosition(width / 2 - 5, height / 2);
        Console.WriteLine("Game Over!");
        Console.SetCursorPosition(width / 2 - 5, height / 2 + 1);
        Console.WriteLine($"Score: {score}");
        Console.ReadLine();
    }

    public static void InitializeSnake()
    {
        snake.Clear();
        snake.Add(new Position { x = width / 2, y = height / 2 });
    }

    public static void GenerateFood()
    {
        food.x = random.Next(0, width);
        food.y = random.Next(0, height);
    }

    public static void ReadInput()
    {
        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.Right;
                        break;
                    case ConsoleKey.UpArrow:
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Direction.Down;
                        break;
                }
            }
        }
    }

    public static void MoveSnake()
    {
        Position head = snake.First();
        Position newHead = new Position();
        switch (direction)
        {
            case Direction.Left:
                newHead.x = head.x - 1;
                newHead.y = head.y;
                break;
            case Direction.Right:
                newHead.x = head.x + 1;
                newHead.y = head.y;
                break;
            case Direction.Up:
                newHead.x = head.x;
                newHead.y = head.y - 1;
                break;
            case Direction.Down:
                newHead.x = head.x;
                newHead.y = head.y + 1;
                break;
        }
        if (newHead.x < 0 || newHead.x >= width || newHead.y < 0 || newHead.y >= height || snake.Contains(newHead))
        {
            gameOver = true;
            return;
        }
        snake.Insert(0, newHead);
        if (newHead.x == food.x && newHead.y == food.y)
        {
            score++;
            GenerateFood();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }
    public static void Draw()
    {
        Console.Clear();
        foreach (var segment in snake)
        {
            Console.SetCursorPosition(segment.x, segment.y);
            Console.Write("*");
        }
        Console.SetCursorPosition(food.x, food.y);
        Console.Write("#");
        Console.SetCursorPosition(0, height);
        Console.Write($"Score: {score}");
    }
}