namespace SnakeGame
{
    public class Snake
    {
        private readonly ConsoleColor HeadColor;
        private readonly ConsoleColor BodyColor;
        public Snake(int initialX,
            int initialY,
            ConsoleColor headColor,
            ConsoleColor bodyColor,
            int bodyLength = 3)
        {
            HeadColor = headColor;
            BodyColor = bodyColor;

            Head = new Pixel(initialX, initialY, HeadColor);

            for (int i = bodyLength; i >= 0; i--)
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, initialY, BodyColor ));
            }

            SnakeDraw();
        }

        public Pixel Head { get; private set; }

        public Queue<Pixel> Body { get; } = new Queue<Pixel>();

        public void Move(Direction direction, bool eat = false)
        {
            SnakeClear();

            Body.Enqueue(new Pixel(Head.X, Head.Y, BodyColor));

            if(!eat)
                Body.Dequeue();

            Head = direction switch
            {
                Direction.Right => new Pixel(Head.X + 1, Head.Y, HeadColor),
                Direction.Left => new Pixel(Head.X - 1, Head.Y, HeadColor),
                Direction.Up => new Pixel(Head.X, Head.Y - 1, HeadColor),
                Direction.Down => new Pixel(Head.X, Head.Y + 1, HeadColor),
                _ => Head
            };

            SnakeDraw();
        }
        public void SnakeDraw()
        {
            Head.PixelDraw();

            foreach(Pixel pixel in Body)
            {
                pixel.PixelDraw();
            }
        }
        public void SnakeClear()
        {
            Head.PixelClear();

            foreach (Pixel pixel in Body)
            {
                pixel.PixelClear();
            }
        }
    }
}
