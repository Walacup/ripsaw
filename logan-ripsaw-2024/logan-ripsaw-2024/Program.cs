using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

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

public enum GameState
{
    Playing,
    GameOver,
    GameWon,
    StarPage,
    NextLevel
}

public class Program
{
    // If you need variables in the Program class (outside functions), you must mark them as static
    static string title = "Stupid Game"; // Window title
    static int screenWidth = 1300; // Screen width
    static int screenHeight = 1000; // Screen height
    static int targetFps = 120; // Target frames-per-second

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
        public Vector2 Velocity;

        public Saw(Vector2 position, Vector2 size, float rotationSpeed, Vector2 velocity)
        {
            Position = position;
            Size = size;
            Rotation = 0f;
            RotationSpeed = rotationSpeed;
            Velocity = velocity;
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
        Color.Black
    };

    static float colorChangeSpeed = 1.0f; // Speed at which colors change

    static bool playerHurt = false;
    static bool gameOver = false;
    static bool gameWon = false;
    static int currentLevel = 1;

    // Enemy properties
    class Enemy
    {
        public Vector2 Position;
        public float Radius;
        public Vector2 Velocity;
        public float Speed;

        public Enemy(Vector2 position, float radius, float speed)
        {
            Position = position;
            Radius = radius;
            Speed = speed;
            Velocity = new Vector2(speed, speed);
        }
    }

    static Enemy enemy = new Enemy(new Vector2(100, 100), 25f, 300f);
    static List<Vector2> enemyProjectiles = new List<Vector2>();
    static float projectileSpeed = 200f;
    static float shootInterval = 1.4f;
    static float timeSinceLastShot = 0f;
    static GameState gameState = GameState.StarPage;

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
            switch (gameState)
            {
                case GameState.Playing:
                    Update();
                    DrawGame();
                    break;
                case GameState.GameOver:
                    DrawGameOver();
                    break;
                case GameState.GameWon:
                    DrawGameWon();
                    break;
                case GameState.StarPage:
                    DrawStarPage();
                    break;
                case GameState.NextLevel:
                    DrawNextLevel();
                    break;
            }

            // Stop drawing to the canvas, begin displaying the frame
            Raylib.EndDrawing();
        }
        // Close the window
        Raylib.CloseWindow();
    }

    static void SetupLevel(int level)
    {
        // Clear previous level data
        saws.Clear();
        collectibles.Clear();

        // Reset player properties
        PlayerPosition = new Vector2(screenWidth / 2, screenHeight / 2);
        playerHealth = 100;

        // Your one-time setup code here
        if (level == 1)
        {
            // Initialize saw positions and velocities
            saws.Add(new Saw(new Vector2(315, 340), new Vector2(20, 60), 700f, new Vector2(200, 150)));
            saws.Add(new Saw(new Vector2(760, 175), new Vector2(20, 60), 700f, new Vector2(150, -200)));
            saws.Add(new Saw(new Vector2(512, 650), new Vector2(20, 60), 700f, new Vector2(-100, 100)));
            saws.Add(new Saw(new Vector2(1223, 209), new Vector2(20, 60), 700f, new Vector2(-200, 150)));
            saws.Add(new Saw(new Vector2(800, 850), new Vector2(20, 60), 700f, new Vector2(250, -100)));
            saws.Add(new Saw(new Vector2(200, 550), new Vector2(20, 60), 700f, new Vector2(100, 200)));
            saws.Add(new Saw(new Vector2(800, 500), new Vector2(20, 60), 700f, new Vector2(-150, -150)));
            saws.Add(new Saw(new Vector2(1100, 800), new Vector2(20, 60), 700f, new Vector2(200, -200)));

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
            // Initialize saw positions and velocities for level 2
            saws.Add(new Saw(new Vector2(315, 340), new Vector2(30, 80), 800f, new Vector2(200, 150)));
            saws.Add(new Saw(new Vector2(760, 175), new Vector2(30, 80), 800f, new Vector2(150, -200)));
            saws.Add(new Saw(new Vector2(512, 650), new Vector2(30, 80), 800f, new Vector2(-100, 100)));
            saws.Add(new Saw(new Vector2(1223, 209), new Vector2(30, 80), 800f, new Vector2(-200, 150)));
            saws.Add(new Saw(new Vector2(800, 850), new Vector2(30, 80), 800f, new Vector2(250, -100)));
            saws.Add(new Saw(new Vector2(200, 550), new Vector2(30, 80), 800f, new Vector2(100, 200)));
            saws.Add(new Saw(new Vector2(800, 500), new Vector2(30, 80), 800f, new Vector2(-150, -150)));
            saws.Add(new Saw(new Vector2(1100, 800), new Vector2(30, 80), 800f, new Vector2(200, -200)));

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
            // Rotate the saw
            saw.Rotation += saw.RotationSpeed * deltaTime;
            if (saw.Rotation > 360f)
            {
                saw.Rotation -= 360f;
            }

            // Move the saw
            saw.Position += saw.Velocity * deltaTime;

            // Bounce the saw off the screen edges
            if (saw.Position.X - saw.Size.X / 2 < 0 || saw.Position.X + saw.Size.X / 2 > screenWidth)
            {
                saw.Velocity.X = -saw.Velocity.X;
            }
            if (saw.Position.Y - saw.Size.Y / 2 < 0 || saw.Position.Y + saw.Size.Y / 2 > screenHeight)
            {
                saw.Velocity.Y = -saw.Velocity.Y;
            }

            // Check collision with player
            if (Raylib.CheckCollisionCircleRec(PlayerPosition, PlayerRadius, new Rectangle(saw.Position.X - saw.Size.X / 2, saw.Position.Y - saw.Size.Y / 2, saw.Size.X, saw.Size.Y)))
            {
                playerHurt = true;
                playerHealth -= 1; // Decrease health when colliding with a saw
                if (playerHealth <= 0)
                {
                    gameOver = true;
                    gameState = GameState.GameOver;
                }
            }
        }

        // Enemy movement
        enemy.Position += enemy.Velocity * deltaTime;

        // Bounce the enemy off the screen edges
        if (enemy.Position.X - enemy.Radius < 0 || enemy.Position.X + enemy.Radius > screenWidth)
        {
            enemy.Velocity.X = -enemy.Velocity.X;
        }
        if (enemy.Position.Y - enemy.Radius < 0 || enemy.Position.Y + enemy.Radius > screenHeight)
        {
            enemy.Velocity.Y = -enemy.Velocity.Y;
        }

        // Enemy shooting
        timeSinceLastShot += deltaTime;
        if (timeSinceLastShot >= shootInterval)
        {
            ShootAtPlayer();
            timeSinceLastShot = 0f;
        }

        // Update enemy projectiles
        for (int i = enemyProjectiles.Count - 1; i >= 0; i--)
        {
            enemyProjectiles[i] += Vector2.Normalize(PlayerPosition - enemyProjectiles[i]) * projectileSpeed * deltaTime;
            if (Raylib.CheckCollisionCircles(PlayerPosition, PlayerRadius, enemyProjectiles[i], 5f))
            {
                playerHealth -= 10; // Decrease health when hit by projectile
                enemyProjectiles.RemoveAt(i);
                if (playerHealth <= 0)
                {
                    gameOver = true;
                    gameState = GameState.GameOver;
                }
            }
            else if (enemyProjectiles[i].X < 0 || enemyProjectiles[i].X > screenWidth || enemyProjectiles[i].Y < 0 || enemyProjectiles[i].Y > screenHeight)
            {
                enemyProjectiles.RemoveAt(i); // Remove projectiles that go off-screen
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
                    if (currentLevel == 1)
                    {
                        gameState = GameState.NextLevel;
                        System.Console.WriteLine("Level 1 Complete!");
                    }
                    else if (currentLevel == 2)
                    {
                        gameWon = true;
                        gameState = GameState.GameWon;
                        System.Console.WriteLine("Game Won!");
                    }
                }
            }
        }
    }

    static void DrawGame()
    {
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

        // Draw enemy
        DrawPlayer(enemy.Position, Color.Blue);

        // Draw enemy projectiles
        foreach (var projectile in enemyProjectiles)
        {
            Raylib.DrawCircleV(projectile, 10f, Color.Red);
        }

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

    static void ShootAtPlayer()
    {
        enemyProjectiles.Add(enemy.Position);
    }

    static void DrawPlayer(Vector2 position, Color color)
    {
        // grey colors
        Color darkGrey = new Color(105, 105, 105, 255); // Dark grey for the body
        Color lightGrey = new Color(169, 169, 169, 255); // Light grey for the dome

        // main body(ellipse)
        float ufoWidth = PlayerRadius * 2.5f;
        float ufoHeight = PlayerRadius * 1.2f;
        Raylib.DrawEllipse((int)position.X, (int)position.Y, (int)ufoWidth, (int)ufoHeight, darkGrey);

        // (ellipse)
        float domeWidth = PlayerRadius * 1.5f;
        float domeHeight = PlayerRadius * 0.8f;
        Vector2 domePosition = new Vector2(position.X, position.Y - PlayerRadius * 0.5f);
        Raylib.DrawEllipse((int)domePosition.X, (int)domePosition.Y, (int)domeWidth, (int)domeHeight, lightGrey);

        // (line)
        Vector2 domeBaseLeft = new Vector2(domePosition.X - domeWidth / 2, domePosition.Y);
        Vector2 domeBaseRight = new Vector2(domePosition.X + domeWidth / 2, domePosition.Y);
        Raylib.DrawLineEx(domeBaseLeft, domeBaseRight, 2, lightGrey);
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
        Raylib.DrawRectangle(barX, barY, barWidth, barHeight, Color.Red);

        // Draw health
        int healthWidth = (int)((playerHealth / 100.0f) * barWidth);
        Raylib.DrawRectangle(barX, barY, healthWidth, barHeight, Color.Green);
    }

    static void DrawGameOver()
    {
        int textWidth = Raylib.MeasureText("Game Over", 40);
        Raylib.DrawText("Game Over", screenWidth / 2 - textWidth / 2, screenHeight / 2 - 20, 40, Color.Red);

        textWidth = Raylib.MeasureText($"Final Score: {score}", 20);
        Raylib.DrawText($"Final Score: {score}", screenWidth / 2 - textWidth / 2, screenHeight / 2 + 20, 20, Color.White);
    }

    static void DrawGameWon()
    {
        int textWidth = Raylib.MeasureText("You Win!", 40);
        Raylib.DrawText("You Win!", screenWidth / 2 - textWidth / 2, screenHeight / 2 - 20, 40, Color.Green);

        textWidth = Raylib.MeasureText($"Final Score: {score}", 20);
        Raylib.DrawText($"Final Score: {score}", screenWidth / 2 - textWidth / 2, screenHeight / 2 + 20, 20, Color.White);
    }

    static void DrawStarPage()
    {
        int textWidth = Raylib.MeasureText("RIPSAW", 80);
        Raylib.DrawText("RIPSAW", screenWidth / 2 - textWidth / 2, screenHeight / 2 - 80, 80, Color.RayWhite);

        textWidth = Raylib.MeasureText("Press ENTER to Start", 20);
        Raylib.DrawText("Press ENTER to Start", screenWidth / 2 - textWidth / 2, screenHeight / 2 + 80, 20, Color.RayWhite);

        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            gameState = GameState.Playing;
            System.Console.WriteLine("Game Started");
        }
    }

    static void DrawNextLevel()
    {
        int textWidth = Raylib.MeasureText("Level 1 Complete!", 40);
        Raylib.DrawText("Level 1 Complete!", screenWidth / 2 - textWidth / 2, screenHeight / 2 - 20, 40, Color.Green);

        textWidth = Raylib.MeasureText("Press ENTER to Start Level 2", 20);
        Raylib.DrawText("Press ENTER to Start Level 2", screenWidth / 2 - textWidth / 2, screenHeight / 2 + 20, 20, Color.White);

        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            currentLevel = 2;
            SetupLevel(currentLevel);
            gameState = GameState.Playing;
            System.Console.WriteLine("Level 2 Started");
        }
    }
}
