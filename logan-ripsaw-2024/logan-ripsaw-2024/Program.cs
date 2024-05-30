using Raylib_cs;
using System.Numerics;


public static class CustomColors
{ 
    public static Color GameBackgroundColor = new Color(0, 10, 11, 255);
    public static Color CustomGreenColor = new Color(78, 133, 110, 255); // Color #4e856e
    public static Color CustomDarkPurpleColor = new Color(38, 11, 38, 255); // Color #260B26
    public static Color CustomLightGreenColor = new Color(187, 242, 172, 255); // Color #BBF2AC
    public static Color CustomBlueColor = new Color(52, 232, 235, 255); // Color #34e8eb
    public static Color CustomPinkColor = new Color(255, 0, 208, 255); // Color #ff00d0
    public static Color CustomYellowColor = new Color(251, 255, 0, 255); // Color #fbff00
    public static Color CustomOrangeColor = new Color(219, 119, 4, 255); // Color #db7704
}
public class Program
{
    // If you need variables in the Program class (outside functions), you must mark them as static
    static string title = "Stupid Game"; // Window title
    static int screenWidth = 1300; // Screen width
    static int screenHeight = 1000; // Screen height
    static int targetFps = 60; // Target frames-per-second

    // Player properties
    static Vector2 PlayerPosition = new Vector2(screenWidth / 2, screenHeight / 2);
    static float PlayerRadius = 25f;
    static float speed = 600f;
    static int playerHealth = 100;

    // Saw properties
    class Saw
    {
        public Vector2 Position;
        public Vector2 Size;
        public float Rotation;
        public float RotationSpeed;

        public Saw(Vector2 position, Vector2 size, float rotationSpeed)
        {
            Position = position;
            Size = size;
            Rotation = 0f;
            RotationSpeed = rotationSpeed;
        }
    }

    static List<Saw> saws = new List<Saw>();

    // Collectible properties
    static List<Vector2> collectibles = new List<Vector2>();
    static float collectibleRadius = 15f;
    static int score = 0; // Player score

    // Color cycle for collectibles
    static List<Color> collectibleColors = new List<Color>
    {
        Color.Gold,
        Color.Red,
        Color.Green,
        Color.Blue
    };

    static float colorChangeSpeed = 5.0f; // Speed at which colors change

    static bool playerHurt = false;
    static bool gameOver = false;
    static bool gameWon = false;
    static int currentLevel = 1;

    static void Main()
    {
        // Create a window to draw to. The arguments define width and height
        Raylib.InitWindow(screenWidth, screenHeight, title);
        // Set the target frames-per-second (FPS)
        Raylib.SetTargetFPS(targetFps);
        // Setup your game. This is a function YOU define.
        SetupLevel(currentLevel);
        // Loop so long as window should not close
        while (!Raylib.WindowShouldClose())
        {
            // Enable drawing to the canvas (window)
            Raylib.BeginDrawing();
            // Clear the canvas with one color
            Raylib.ClearBackground(Color.Black);
            // Your game code here. This is a function YOU define.

            if (!gameOver && !gameWon)
            {
                Update();
            }
            else if (gameOver)
            {
                DrawGameOver();
            }
            else if (gameWon)
            {
                DrawGameWon();
            }

            // Stop drawing to the canvas, begin displaying the frame
            Raylib.EndDrawing();
        }
        // Close the window
        Raylib.CloseWindow();
    }

    static void SetupLevel(int level)
    {
        // Your one-time setup code here
        if (level == 1)
        {
            // Initialize saw positions
            saws.Add(new Saw(new Vector2(315, 340), new Vector2(20, 60), 700f));
            saws.Add(new Saw(new Vector2(760, 175), new Vector2(20, 60), 700f));
            saws.Add(new Saw(new Vector2(512, 650), new Vector2(20, 60), 700f));
            saws.Add(new Saw(new Vector2(1223, 209), new Vector2(20, 60), 700f));
            saws.Add(new Saw(new Vector2(800, 850), new Vector2(20, 60), 700f));
            saws.Add(new Saw(new Vector2(200, 550), new Vector2(20, 60), 700f));
            saws.Add(new Saw(new Vector2(800, 500), new Vector2(20, 60), 700f));
            saws.Add(new Saw(new Vector2(1100, 800), new Vector2(20, 60), 700f));

            // Initialize collectible positions
            collectibles.Add(new Vector2(290, 200));
            collectibles.Add(new Vector2(570, 580));
            collectibles.Add(new Vector2(867, 300));
            collectibles.Add(new Vector2(780, 100));
            collectibles.Add(new Vector2(423, 403));
            collectibles.Add(new Vector2(1140, 310));
            collectibles.Add(new Vector2(707, 840));
            collectibles.Add(new Vector2(1110, 870));
            collectibles.Add(new Vector2(400, 700));
            collectibles.Add(new Vector2(1000, 900));
        }

        else if (level == 2)
        {
            // Initialize saw positions for level 2
            saws.Add(new Saw(new Vector2(315, 340), new Vector2(30, 80), 800f));
            saws.Add(new Saw(new Vector2(760, 175), new Vector2(30, 80), 800f));
            saws.Add(new Saw(new Vector2(512, 650), new Vector2(30, 80), 800f));
            saws.Add(new Saw(new Vector2(1223, 209), new Vector2(30, 80), 800f));
            saws.Add(new Saw(new Vector2(800, 850), new Vector2(30, 80), 800f));
            saws.Add(new Saw(new Vector2(200, 550), new Vector2(30, 80), 800f));
            saws.Add(new Saw(new Vector2(800, 500), new Vector2(30, 80), 800f));
            saws.Add(new Saw(new Vector2(1100, 800), new Vector2(30, 80), 800f));

            // Initialize collectible positions for level 2
            collectibles.Add(new Vector2(150, 150));
            collectibles.Add(new Vector2(300, 300));
            collectibles.Add(new Vector2(450, 450));
            collectibles.Add(new Vector2(600, 600));
            collectibles.Add(new Vector2(750, 750));
            collectibles.Add(new Vector2(900, 900));
            collectibles.Add(new Vector2(1050, 1050));
            collectibles.Add(new Vector2(1200, 1200));
        }
    }

    static void Update()
    {
        // Your game code run each frame here

        // Player movement
        float deltaTime = Raylib.GetFrameTime();

        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            PlayerPosition.Y -= speed * deltaTime;
        }
        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            PlayerPosition.Y += speed * deltaTime;
        }
        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            PlayerPosition.X -= speed * deltaTime;
        }
        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            PlayerPosition.X += speed * deltaTime;
        }

        // Boundary checks
        if (PlayerPosition.X - PlayerRadius < 0)
        {
            PlayerPosition.X = PlayerRadius;
        }
        if (PlayerPosition.Y - PlayerRadius < 0)
        {
            PlayerPosition.Y = PlayerRadius;
        }
        if (PlayerPosition.X + PlayerRadius > screenWidth)
        {
            PlayerPosition.X = screenWidth - PlayerRadius;
        }
        if (PlayerPosition.Y + PlayerRadius > screenHeight)
        {
            PlayerPosition.Y = screenHeight - PlayerRadius;
        }

        // Saw rotation and collision
        playerHurt = false;
        foreach (var saw in saws)
        {
            saw.Rotation += saw.RotationSpeed * deltaTime;
            if (saw.Rotation > 360f)
            {
                saw.Rotation -= 360f;
            }

            if (Raylib.CheckCollisionCircleRec(PlayerPosition, PlayerRadius, new Rectangle(saw.Position.X - saw.Size.X / 2, saw.Position.Y - saw.Size.Y / 2, saw.Size.X, saw.Size.Y)))
            {
                playerHurt = true;
                playerHealth -= 1; // Decrease health when colliding with a saw
                if (playerHealth <= 0)
                {
                    gameOver = true;
                }
            }
        }
        

        // Check for collision with collectibles
        for (int i = collectibles.Count - 1; i >= 0; i--)
        {
            if (Raylib.CheckCollisionCircles(PlayerPosition, PlayerRadius, collectibles[i], collectibleRadius))
            {
                collectibles.RemoveAt(i); // Remove collectible if collected
                score++; // Increment score

                // Check if all collectibles are collected
                if (collectibles.Count == 0)
                {
                    gameWon = true;
                }
            }
        
        }

        // Draw Saws
        foreach (var saw in saws)
        {
            Raylib.DrawRectanglePro(
               new Rectangle(saw.Position.X, saw.Position.Y, saw.Size.X, saw.Size.Y),
               new Vector2(saw.Size.X / 2, saw.Size.Y / 2),
               saw.Rotation,
               Color.LightGray
           );
        }

        // Draw player
        DrawPlayer(PlayerPosition, playerHurt ? Color.Red : Color.RayWhite);

        // Draw collectibles
        float elapsedTime = (float)Raylib.GetTime();
        int colorIndex = (int)(elapsedTime * colorChangeSpeed) % collectibleColors.Count;
        Color currentColor = collectibleColors[colorIndex];
        foreach (var collectible in collectibles)
        {
            Raylib.DrawCircleV(collectible, collectibleRadius, currentColor);
        }

        DrawScore();
        DrawHealthBar();
    }

    static void DrawPlayer(Vector2 position, Color color)
    {

        Raylib.DrawCircleV(position, PlayerRadius, color);
    }

    static void DrawScore()
    {
        Raylib.DrawText($"Score: {score}", 10, 10, 80, Color.White);
    }

    static void DrawHealthBar()
    {
        int barWidth = 400;
        int barHeight = 40;
        int barX = 10;
        int barY = 100;

        // Draw background
        Raylib.DrawRectangle(barX, barY, barWidth, barHeight, Color.Gray);

        // Draw health
        int healthWidth = (int)((playerHealth / 100.0f) * barWidth);
        Raylib.DrawRectangle(barX, barY, healthWidth, barHeight, Color.Red);
    }

    static void DrawGameOver()
    {
        Raylib.DrawText("Game Over", screenWidth / 2 - 100, screenHeight / 2 - 20, 40, Color.Red);
        Raylib.DrawText($"Final Score: {score}", screenWidth / 2 - 100, screenHeight / 2 + 20, 20, Color.White);
    }

    static void DrawGameWon()
    {
        Raylib.DrawText("You Win!", screenWidth / 2 - 100, screenHeight / 2 - 20, 40, Color.Green);
        Raylib.DrawText($"Final Score: {score}", screenWidth / 2 - 100, screenHeight / 2 + 20, 20, Color.White);
    }

}