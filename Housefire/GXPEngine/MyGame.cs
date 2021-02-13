using System;                                   // System contains a lot of default C# libraries 
using System.Diagnostics;
using System.Drawing;                           // System.Drawing contains a library used for canvas drawing below
using System.Threading;
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Objects;
using GXPEngine.Objects.Handlers;
using GXPEngine.Objects.Scenes;
using GXPEngine.OpenGL;

public class MyGame : Game
{
	public static MyGame Instance;
    public static bool drawCollision = false;       //Debug draw collision?

    public BeatmapButtonHandler beatmapButtonHandler;
    public MyGame() : base(1920, 1080, false, false, 1920, 1080, false)		// Create a window that's 800x600 and NOT fullscreen
	{
		Instance = this;
		targetFps = 1000;

        //EasyDraw easyDraw = new EasyDraw(new Bitmap(@"E:\School\Periode 3\Project 3\HouseFire\Housefire\GXPEngine\bin\Debug\Songs\What the cat\BG.jpg"), false);
        //AddChild(easyDraw);

        GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
        MenuScreen menuScreen = new MenuScreen();
        AddChild(menuScreen);

        //beatmapButtonHandler = new BeatmapButtonHandler();
        //AddChild(beatmapButtonHandler);

       
    }

    float counter = 0;
    BeatmapHandler beatmapHandler;
    public Sound oldSong = null;

    public void StartBeatMap(string name)
    {
        oldSong?.Stop();
        oldSong = null;
        beatmapHandler?.Stop();
        beatmapHandler = new BeatmapHandler(name);
        beatmapHandler.activeBeatmap?.WriteDebug();
        beatmapHandler?.Play();
    }

    public void StartBeatMap(Beatmap beatmap)
    {
        oldSong?.Stop();
        oldSong = null;
        beatmapHandler?.Stop();
        beatmapHandler = new BeatmapHandler(beatmap);
        beatmapHandler.activeBeatmap?.WriteDebug();
        beatmapHandler?.Play();
    }

    void Update()
	{
        counter += Time.deltaTime;

        if (Input.GetKeyDown(Key.ENTER))
        {
            beatmapHandler?.Play();
        }

        if (Input.GetKeyDown(Key.P)){
            OsuToBeatConverter.ConvertFile(4);
        }

        if(counter >= 1)
        {
            counter = 0;
            Debug.WriteLine(currentFps);
        }

        if (Input.GetKeyDown(Key.Y))
        {
            oldSong = beatmapHandler?.Stop(true);

        }

        if (Input.GetKeyDown(Key.U))
        {
            beatmapHandler?.Stop();
        }

        if (Input.GetKeyDown(Key.C))
        {
            drawCollision = !drawCollision;
        }

        beatmapHandler?.Update();

       
    }

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}
}