using System;                                   // System contains a lot of default C# libraries 
using System.Diagnostics;
using System.Drawing;                           // System.Drawing contains a library used for canvas drawing below
using System.Threading;
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Objects;
using GXPEngine.Objects.Handlers;
using GXPEngine.OpenGL;

public class MyGame : Game
{
	public static MyGame Instance;
    public static bool drawCollision = false;       //Debug draw collision?

    BeatmapHandler beatmapHandler;

    public MyGame() : base(1920, 1080, false, false, 1920, 1080, false)		// Create a window that's 800x600 and NOT fullscreen
	{
		Instance = this;
		targetFps = 500;
        GL.ClearColor(0.5f, 0.5f, 0.5f, 1);

        beatmapHandler = new BeatmapHandler();
        AddChild(beatmapHandler);
    }


   

    float counter = 0;

    void StartBeatMap(string name)
    {
        beatmapHandler?.Stop();
        beatmapHandler?.SpawnBeatmap(name);
        Thread.Sleep(10);
        beatmapHandler?.Play();
    }

    void Update()
	{
        counter += Time.deltaTime;

        

        if (Input.GetKeyDown(Key.P)){
            OsuToBeatConverter.ConvertFile(4);
        }

        if(counter >= 1)
        {
            counter = 0;
            Debug.WriteLine(currentFps);
        }
       
        if (Input.GetKeyDown(Key.ONE))
        {
            StartBeatMap("Songs/Song1/beatmap.txt");
        }

        if (Input.GetKeyDown(Key.TWO))
        {
            StartBeatMap("Songs/Song2/beatmap.txt");
        }

        if (Input.GetKeyDown(Key.THREE))
        {
            StartBeatMap("Songs/Song3/beatmap.txt");
        }

        if (Input.GetKeyDown(Key.FOUR))
        {
            StartBeatMap("Songs/Song4/beatmap.txt");
        }

        if (Input.GetKeyDown(Key.Y))
        {
            beatmapHandler?.Stop();
            
        }

        if (Input.GetKeyDown(Key.C))
        {
            drawCollision = !drawCollision;
        }

       
    }

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}
}