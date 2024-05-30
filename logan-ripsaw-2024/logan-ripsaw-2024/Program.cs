﻿using Raylib_cs;
using System.Numerics;


public static class CustomColors
{ 
    public static Color GameBackgroundColor = new Color(0, 10, 11, 255);
    public static Color CustomGreenColor = new Color(78, 133, 110, 255); // Color #4e856e
    public static Color CustomDarkPurpleColor = new Color(38, 11, 38, 255); // Color #260B26
    public static Color CustomLightGreenColor = new Color(187, 242, 172, 255); // Color #BBF2AC
}
public class Program
{
    // If you need variables in the Program class (outside functions), you must mark them as static
    static string title = "Ripsaw"; // Window title
    static int screenWidth = 1300; // Screen width
    static int screenHeight = 1000; // Screen height
    static int targetFps = 60; // Target frames-per-second

    // Player properties
    static Vector2 PlayerPosition = new Vector2(screenWidth / 2, screenHeight / 2);
    static float PlayerRadius = 25f;
    static float speed = 600f;

    // Saw properties
    static Vector2 sawPosition = new Vector2(400, 300);
    static Vector2 sawSize = new Vector2(20, 60);
    static float sawRotation = 0f; // Saw rotation angle
    static float sawRotationSpeed = 160f; // Degrees per second

    static bool playerHurt = false;


    static void Main()
    {
        // Create a window to draw to. The arguments define width and height
        Raylib.InitWindow(screenWidth, screenHeight, title);
        // Set the target frames-per-second (FPS)
        Raylib.SetTargetFPS(targetFps);
        // Setup your game. This is a function YOU define.
        Setup();
        // Loop so long as window should not close
        while (!Raylib.WindowShouldClose())
        {
            // Enable drawing to the canvas (window)
            Raylib.BeginDrawing();
            // Clear the canvas with one color
            Raylib.ClearBackground(CustomColors.CustomDarkPurpleColor);
            // Your game code here. This is a function YOU define.

            Update();
            // Stop drawing to the canvas, begin displaying the frame
            Raylib.EndDrawing();
        }
        // Close the window
        Raylib.CloseWindow();
    }

    static void Setup()
    {
        // Your one-time setup code here
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

        // Saw rotation
        sawRotation += sawRotationSpeed * deltaTime;
        if (sawRotation > 360f)
        {
            sawRotation -= 360f;
        }

        // Check for collision with the saw
        if (Raylib.CheckCollisionCircleRec(PlayerPosition, PlayerRadius, new Rectangle(sawPosition.X - sawSize.X / 2, sawPosition.Y - sawSize.Y / 2, sawSize.X, sawSize.Y)))
        {
            playerHurt = true;
        }
        else
        {
            playerHurt = false;
        }

        // Draw Saws
        Raylib.DrawRectanglePro(
           new Rectangle(sawPosition.X, sawPosition.Y, sawSize.X, sawSize.Y),
           new Vector2(sawSize.X / 2, sawSize.Y / 2),
           sawRotation,
           Color.LightGray
       );

        // Draw player
        // Draw player (bunny shape)
        DrawPlayer(PlayerPosition, playerHurt ? Color.Red : Color.RayWhite);
    }

    static void DrawPlayer(Vector2 position, Color color)
    {
       
        Raylib.DrawCircleV(position, PlayerRadius, color);
    }
}