﻿using Raylib_cs;
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

    static float colorChangeSpeed = 0.8f; // Speed at which colors change

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

    static List<Enemy> enemies = new List<Enemy>();

    // Projectile properties
    class Projectile
    {
        public Vector2 Position;
        public float Speed;
        public float TimeSinceFired;

        public Projectile(Vector2 position, float speed)
        {
            Position = position;
            Speed = speed;
            TimeSinceFired = 0f;
        }
    }

    static List<Projectile> enemyProjectiles = new List<Projectile>();
    static float projectileSpeed = 200f; // Define projectile speed
    static float shootInterval = 1.4f;
    static float timeSinceLastShot = 0f;
    static GameState gameState = GameState.StarPage;

    static void Main()
    {
        Raylib.InitWindow(screenWidth, screenHeight, title);
        Raylib.SetTargetFPS(targetFps);
        SetupLevel(currentLevel);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
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
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }

    static void SetupLevel(int level)
    {
        saws.Clear();
        collectibles.Clear();
        enemies.Clear();
        PlayerPosition = new Vector2(screenWidth / 2, screenHeight / 2);
        playerHealth = 100;

        if (level == 1)
        {
            saws.Add(new Saw(new Vector2(315, 340), new Vector2(20, 60), 700f, new Vector2(200, 150)));
            saws.Add(new Saw(new Vector2(760, 175), new Vector2(20, 60), 700f, new Vector2(150, -200)));
            saws.Add(new Saw(new Vector2(512, 650), new Vector2(20, 60), 700f, new Vector2(-100, 100)));
            saws.Add(new Saw(new Vector2(1223, 209), new Vector2(20, 60), 700f, new Vector2(-200, 150)));
            saws.Add(new Saw(new Vector2(800, 850), new Vector2(20, 60), 700f, new Vector2(250, -100)));
            saws.Add(new Saw(new Vector2(200, 550), new Vector2(20, 60), 700f, new Vector2(100, 200)));
            saws.Add(new Saw(new Vector2(800, 500), new Vector2(20, 60), 700f, new Vector2(-150, -150)));
            saws.Add(new Saw(new Vector2(1100, 800), new Vector2(20, 60), 700f, new Vector2(200, -200)));

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

            enemies.Add(new Enemy(new Vector2(100, 100), 25f, 300f));
        }
        else if (level == 2)
        {
            saws.Add(new Saw(new Vector2(315, 340), new Vector2(30, 80), 800f, new Vector2(200, 150)));
            saws.Add(new Saw(new Vector2(760, 175), new Vector2(30, 80), 800f, new Vector2(150, -200)));
            saws.Add(new Saw(new Vector2(512, 650), new Vector2(30, 80), 800f, new Vector2(-100, 100)));
            saws.Add(new Saw(new Vector2(1223, 209), new Vector2(30, 80), 800f, new Vector2(-200, 150)));
            saws.Add(new Saw(new Vector2(800, 850), new Vector2(30, 80), 800f, new Vector2(250, -100)));
            saws.Add(new Saw(new Vector2(200, 550), new Vector2(30, 80), 800f, new Vector2(100, 200)));
            saws.Add(new Saw(new Vector2(800, 500), new Vector2(30, 80), 800f, new Vector2(-150, -150)));
            saws.Add(new Saw(new Vector2(1100, 800), new Vector2(30, 80), 800f, new Vector2(200, -200)));

            collectibles.Add(new Vector2(150, 150));
            collectibles.Add(new Vector2(300, 300));
            collectibles.Add(new Vector2(450, 450));
            collectibles.Add(new Vector2(600, 600));
            collectibles.Add(new Vector2(750, 750));
            collectibles.Add(new Vector2(900, 900));
            collectibles.Add(new Vector2(1050, 1050));
            collectibles.Add(new Vector2(1200, 1200));

            enemies.Add(new Enemy(new Vector2(100, 100), 25f, 300f));
            enemies.Add(new Enemy(new Vector2(1200, 100), 25f, 300f));
        }
    }

    static void Update()
    {
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

        playerHurt = false;
        foreach (var saw in saws)
        {
            saw.Rotation += saw.RotationSpeed * deltaTime;
            if (saw.Rotation > 360f)
            {
                saw.Rotation -= 360f;
            }

            saw.Position += saw.Velocity * deltaTime;

            if (saw.Position.X - saw.Size.X / 2 < 0 || saw.Position.X + saw.Size.X / 2 > screenWidth)
            {
                saw.Velocity.X = -saw.Velocity.X;
            }
            if (saw.Position.Y - saw.Size.Y / 2 < 0 || saw.Position.Y + saw.Size.Y / 2 > screenHeight)
            {
                saw.Velocity.Y = -saw.Velocity.Y;
            }

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

        foreach (var enemy in enemies)
        {
            enemy.Position += enemy.Velocity * deltaTime;

            if (enemy.Position.X - enemy.Radius < 0 || enemy.Position.X + enemy.Radius > screenWidth)
            {
                enemy.Velocity.X = -enemy.Velocity.X;
            }
            if (enemy.Position.Y - enemy.Radius < 0 || enemy.Position.Y + enemy.Radius > screenHeight)
            {
                enemy.Velocity.Y = -enemy.Velocity.Y;
            }
        }

        timeSinceLastShot += deltaTime;
        if (timeSinceLastShot >= shootInterval)
        {
            foreach (var enemy in enemies)
            {
                ShootAtPlayer(enemy.Position);
            }
            timeSinceLastShot = 0f;
        }

        for (int i = enemyProjectiles.Count - 1; i >= 0; i--)
        {
            enemyProjectiles[i].Position += Vector2.Normalize(PlayerPosition - enemyProjectiles[i].Position) * enemyProjectiles[i].Speed * deltaTime;
            enemyProjectiles[i].TimeSinceFired += deltaTime;

            if (enemyProjectiles[i].TimeSinceFired >= 3f)
            {
                // Explode the projectile
                if (Raylib.CheckCollisionCircles(PlayerPosition, PlayerRadius, enemyProjectiles[i].Position, 50f))
                {
                    playerHealth -= 20; // Decrease health when hit by explosion
                    if (playerHealth <= 0)
                    {
                        gameOver = true;
                        gameState = GameState.GameOver;
                    }
                }
                enemyProjectiles.RemoveAt(i);
            }
            else if (Raylib.CheckCollisionCircles(PlayerPosition, PlayerRadius, enemyProjectiles[i].Position, 5f))
            {
                playerHealth -= 10; // Decrease health when hit by projectile
                enemyProjectiles.RemoveAt(i);
                if (playerHealth <= 0)
                {
                    gameOver = true;
                    gameState = GameState.GameOver;
                }
            }
            else if (enemyProjectiles[i].Position.X < 0 || enemyProjectiles[i].Position.X > screenWidth || enemyProjectiles[i].Position.Y < 0 || enemyProjectiles[i].Position.Y > screenHeight)
            {
                enemyProjectiles.RemoveAt(i); // Remove projectiles that go off-screen
            }
        }

        for (int i = collectibles.Count - 1; i >= 0; i--)
        {
            if (Raylib.CheckCollisionCircles(PlayerPosition, PlayerRadius, collectibles[i], collectibleRadius))
            {
                collectibles.RemoveAt(i); // Remove collectible if collected
                score++; // Increment score

                if (collectibles.Count == 0)
                {
                    if (currentLevel == 1)
                    {
                        gameState = GameState.NextLevel;
                        System.Console.WriteLine("Level 1 Complete!");
                    }
                    else if (currentLevel == 2)
                    {
                        gameState = GameState.GameWon; // Ensure state transition
                        System.Console.WriteLine("Game Won!");
                    }
                }
            }
        }
    }

    static void DrawGame()
    {
        foreach (var saw in saws)
        {
            Raylib.DrawRectanglePro(
               new Rectangle(saw.Position.X, saw.Position.Y, saw.Size.X, saw.Size.Y),
               new Vector2(saw.Size.X / 2, saw.Size.Y / 2),
               saw.Rotation,
               Color.LightGray
           );
        }

        DrawPlayer(PlayerPosition, playerHurt ? Color.Red : Color.RayWhite);

        foreach (var enemy in enemies)
        {
            DrawPlayer(enemy.Position, Color.Blue);
        }

        foreach (var projectile in enemyProjectiles)
        {
            Raylib.DrawCircleV(projectile.Position, 10f, Color.Red);
        }

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

    static void ShootAtPlayer(Vector2 enemyPosition)
    {
        enemyProjectiles.Add(new Projectile(enemyPosition, projectileSpeed));
    }

    static void DrawPlayer(Vector2 position, Color color)
    {
        Color darkGrey = new Color(105, 105, 105, 255); // Dark grey for the body
        Color lightGrey = new Color(169, 169, 169, 255); // Light grey for the dome

        float ufoWidth = PlayerRadius * 2.5f;
        float ufoHeight = PlayerRadius * 1.2f;
        Raylib.DrawEllipse((int)position.X, (int)position.Y, (int)ufoWidth, (int)ufoHeight, darkGrey);

        float domeWidth = PlayerRadius * 1.5f;
        float domeHeight = PlayerRadius * 0.8f;
        Vector2 domePosition = new Vector2(position.X, position.Y - PlayerRadius * 0.5f);
        Raylib.DrawEllipse((int)domePosition.X, (int)domePosition.Y, (int)domeWidth, (int)domeHeight, lightGrey);

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

        Raylib.DrawRectangle(barX, barY, barWidth, barHeight, Color.Red);

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
