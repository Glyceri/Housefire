using System;                                   // System contains a lot of default C# libraries 
using System.Diagnostics;
using System.Drawing;                           // System.Drawing contains a library used for canvas drawing below
using System.Threading;
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Objects;
using GXPEngine.Objects.Handlers;
using GXPEngine.Objects.Scenes;
using GXPEngine.OpenGL;
using GXPEngine.Core.Audio;

public class MyGame : Game
{
	public static MyGame Instance;
    public static bool drawCollision = false;       //Debug draw collision?

    public BeatmapButtonHandler beatmapButtonHandler;

    MainMenuScreen menuScreen;

    public MenuMusicHandler musicHandler;
    public InsertCoinMenu coinMenu;

    public MyGame() : base(1920, 1080, false, false, 1920, 1080, false)		// Create a window that's 800x600 and NOT fullscreen
	{
		Instance = this;
		targetFps = 1000;
        GL.ClearColor(0f, 0f, 0f, 1);

        musicHandler = new MenuMusicHandler();

        menuScreen = new MainMenuScreen();
        AddChild(menuScreen);


        menuScreen.canBeInteractedWith = false;

        coinMenu = new InsertCoinMenu();
        coinMenu.onAnimationEnd += OnCoindAnimationEnd;
        AddChild(coinMenu);

        AddChild(musicHandler);
    }

    void OnStartup()
    {
        coinMenu.visible = true;
        menuScreen.canBeInteractedWith = false;
        coinMenu.canBeInteractedWith = true;
    }

    void OnCoindAnimationEnd()
    {
        musicHandler.canBeInteractedWith = true;
        menuScreen.canBeInteractedWith = true;
        coinMenu.canBeInteractedWith = false;
        coinMenu.visible = false;
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

        if (Input.GetKeyDown(Key.BACKSPACE))
        {
            OnStartup();
        }

        if (Input.GetKeyDown(Key.G))
        {
            musicHandler.canBeInteractedWith = !musicHandler.canBeInteractedWith;
        }

        if (Input.GetKeyDown(Key.H))
        {
            menuScreen.menuScreen.canBeInteractedWith = !menuScreen.menuScreen.canBeInteractedWith;
        }

        if (Input.GetKeyDown(Key.F))
        {
            coinMenu.visible = !coinMenu.visible;
        }

        if (Input.GetKeyDown(Key.J))
        {
            musicHandler.visible = !musicHandler.visible;
        }

        if (Input.GetKeyDown(Key.K))
        {
            menuScreen.menuScreen.visible = !menuScreen.menuScreen.visible;
        }

        if (counter >= 1)
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