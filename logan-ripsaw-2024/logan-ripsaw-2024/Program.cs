using Raylib_cs;
using System.Numerics;

public class Program
{
    // If you need variables in the Program class (outside functions), you must mark them as static
    static string title = "Ripsaw"; // Window title
    static int screenWidth = 800; // Screen width
    static int screenHeight = 600; // Screen height
    static int targetFps = 60; // Target frames-per-second

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
            Raylib.ClearBackground(Color.Beige);
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
    }
}