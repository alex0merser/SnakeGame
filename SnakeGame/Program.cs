using System.Diagnostics;
using static System.Console;

namespace SnakeGame
{
    class Program
    {
        static readonly int MapWidth = 40;
        static readonly int MapHeight = 25;

        static readonly int ScreenWidth = MapWidth * 3;
        static readonly int ScreenHeight = MapHeight * 3;

        private const int FrameMs = 200;

        private const ConsoleColor BorderColor = ConsoleColor.Gray;
        private const ConsoleColor HeadColor = ConsoleColor.DarkYellow;
        private const ConsoleColor BodyColor = ConsoleColor.DarkGray;
        private const ConsoleColor FoodColor = ConsoleColor.DarkGreen;

        private static readonly Random Random = new();

        static void Main()
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false;

            while (true)
            {
                StartGame();

                Thread.Sleep(2000);

                ReadKey();
            }
        }

        static void StartGame()
        {
            Clear();

            DrowBorder();

            var snake = new Snake(7, 10, HeadColor, BodyColor);

            var food = GenFood(snake);

            food.PixelDraw();

            Direction currentMovement = Direction.Right;

            int score = 0;

            int lagMs = 0;

            Stopwatch sw = new();

            while (true)
            {
                sw.Restart();

                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMs - lagMs)
                {
                    if (currentMovement == oldMovement)
                        currentMovement = ReadMovement(currentMovement);
                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);

                    food = GenFood(snake);
                    food.PixelDraw();

                    score++;

                    Task.Run(() => Beep(1200, 200));
                }
                else 
                    snake.Move(currentMovement);

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;
                lagMs = (int)sw.ElapsedMilliseconds;
            }

            snake.SnakeClear();

            SetCursorPosition(ScreenWidth / 3, ScreenHeight / 2);
            WriteLine($"Game Over, score: {score}");

            Task.Run(() => Beep(200, 600));

        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y
                        || snake.Body.Any(b => b.X == food.X  && b.Y == food.Y));
            return food;
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if(!KeyAvailable)
                return currentDirection;
            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };
            return currentDirection;
        }
        static void DrowBorder()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).PixelDraw();
                new Pixel(i, MapHeight - 1, BorderColor).PixelDraw();

            }


            for (int i = 0; i < MapHeight - 1; i++)
            {
                new Pixel(0, i, BorderColor).PixelDraw();
                new Pixel(MapWidth - 1, i, BorderColor).PixelDraw();
            }
        }
    }
}