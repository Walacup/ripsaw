using Raylib_cs;
using System.Numerics;


public static class CustomColors
{ 
    public static Color GameBackgroundColor = new Color(0, 10, 11, 255);
}
public class Program
{
    // If you need variables in the Program class (outside functions), you must mark them as static
    static string title = "Ripsaw"; // Window title
    static int screenWidth = 1300; // Screen width
    static int screenHeight = 1000; // Screen height
    static int targetFps = 60; // Target frames-per-second

    // Define the rectangle's properties
    static Vector2 PlayerPosition = new Vector2(screenWidth / 2, screenHeight / 2);
    static Vector2 PlayerSize = new Vector2(50, 50);
    static float speed = 600f;

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
            Raylib.ClearBackground(CustomColors.GameBackgroundColor);
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
        if (PlayerPosition.X < 0)
        {
            PlayerPosition.X = 0;
        }
        if (PlayerPosition.Y < 0)
        {
            PlayerPosition.Y = 0;
        }
        if (PlayerPosition.X + PlayerSize.X > screenWidth)
        {
            PlayerPosition.X = screenWidth - PlayerSize.X;
        }
        if (PlayerPosition.Y + PlayerSize.Y > screenHeight)
        {
            PlayerPosition.Y = screenHeight - PlayerSize.Y;

        }

            Raylib.DrawRectangleV(PlayerPosition, PlayerSize, Color.RayWhite);
        
    }
}