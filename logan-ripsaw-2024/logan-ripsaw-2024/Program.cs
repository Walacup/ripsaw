using Raylib_cs;
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

        // Initialize saw positions
        saws.Add(new Saw(new Vector2(200, 300), new Vector2(20, 60), 160f));
        saws.Add(new Saw(new Vector2(700, 175), new Vector2(20, 60), 120f));
        saws.Add(new Saw(new Vector2(500, 600), new Vector2(20, 60), 180f));

        // Initialize collectible positions
        collectibles.Add(new Vector2(200, 200));
        collectibles.Add(new Vector2(500, 500));
        collectibles.Add(new Vector2(800, 300));
        collectibles.Add(new Vector2(700, 100));
        collectibles.Add(new Vector2(400, 400));
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
            }
        }

        // Check for collision with collectibles
        for (int i = collectibles.Count - 1; i >= 0; i--)
        {
            if (Raylib.CheckCollisionCircles(PlayerPosition, PlayerRadius, collectibles[i], collectibleRadius))
            {
                collectibles.RemoveAt(i); // Remove collectible if collected
            }
        }

        // Draw Saws
        // Draw saws
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
        foreach (var collectible in collectibles)
        {
            Raylib.DrawCircleV(collectible, collectibleRadius, Color.Gold);
        }
    }

   static void DrawPlayer(Vector2 position, Color color)
    {

        Raylib.DrawCircleV(position, PlayerRadius, color);
    }

}