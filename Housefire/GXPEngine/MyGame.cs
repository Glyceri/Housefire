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

    public static int roundsRemaining = 3;

    public MainMenuScreen menuScreen;

    public MenuMusicHandler musicHandler;
    public InsertCoinMenu coinMenu;
    public PlayerMenu playerMenu;
    public LivesMenu livesMenu;
    public MenuScores menuScores;

    public Player player1;
    public Player player2;

    public HighscoreHandler highscorehandler;

    public MyGame() : base(1920, 1080, false, false, 1920, 1080, false)		// Create a window that's 800x600 and NOT fullscreen
	{
		Instance = this;
		targetFps = 1000;
        GL.ClearColor(0f, 0f, 0f, 1);

        highscorehandler = new HighscoreHandler();
        highscorehandler.ReadHighscores();

        musicHandler = new MenuMusicHandler();
        //menuOverlay = new MenuOverlay();
        menuScreen = new MainMenuScreen();
        playerMenu = new PlayerMenu();
        menuScores = new MenuScores();
        livesMenu = new LivesMenu();

        AddChild(menuScreen);
        AddChild(livesMenu);
        menuScreen.canBeInteractedWith = false;

        coinMenu = new InsertCoinMenu();
        coinMenu.onAnimationEnd += OnCoinAnimationEnd;

        playerMenu.onAnimationEnd += OnPlayerSelectEnd;

        //AddChild(menuOverlay);
        AddChild(playerMenu);
        AddChild(coinMenu);

        AddChild(menuScores);
        AddChild(musicHandler);

        

        OnStartup();
        livesMenu.SetLives();
        livesMenu.SetRoundsRemaining();
    }

    public void OnPlayerSelectEnd()
    {
        playerMenu.inEditMode = false;
        coinMenu.visible = false;
        menuScreen.canBeInteractedWith = true;
        playerMenu.canBeInteractedWith = false;
        playerMenu.visible = false;
        coinMenu.canBeInteractedWith = false;
        menuScores.visible = false;
        menuScores.canBeInteractedWith = false;
        livesMenu.SetLives();
        livesMenu.SetRoundsRemaining();
    }

    void OnStartup()
    {
        roundsRemaining = 3;
        menuScores.visible = false;
        menuScores.canBeInteractedWith = false;
        playerMenu.visible = true;
        playerMenu.inEditMode = false;
        coinMenu.visible = true;
        menuScreen.canBeInteractedWith = false;
        playerMenu.canBeInteractedWith = false;
        coinMenu.canBeInteractedWith = true;
        player1 = new Player(1);
        player2 = new Player(2);
    }

    void OnCoinAnimationEnd()
    {
        
        playerMenu.inEditMode = false;
        musicHandler.canBeInteractedWith = true;
        menuScreen.canBeInteractedWith = false;
        playerMenu.canBeInteractedWith = true;
        coinMenu.canBeInteractedWith = false;
        coinMenu.visible = false;
    }

    float counter = 0;
    public BeatmapHandler beatmapHandler;

    public void StartBeatMap(string name)
    {
        beatmapHandler?.Stop();
        beatmapHandler = new BeatmapHandler(name);
        //beatmapHandler.activeBeatmap?.WriteDebug();
        //beatmapHandler?.Play();
    }

    public void StartBeatMap(BeatmapSmall beatmap)
    {
      
        beatmapHandler?.Stop();
        beatmapHandler = new BeatmapHandler(beatmap);
        //beatmapHandler.activeBeatmap?.WriteDebug();
        //beatmapHandler?.Play();
    }

    void Update()
	{
        counter += Time.deltaTime;

        if (!playerMenu.inEditMode && (!beatmapHandler?.isPlaying ?? false))
        {
            if (Input.GetKeyDown(Key.ENTER))
            {
                beatmapHandler?.Play();
            }

            if (Input.GetKeyDown(Key.P))
            {
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
            if (Input.GetKeyDown(Key.Y))
            {
                //oldSong = beatmapHandler?.Stop(true);

            }
         
        }
        if (Input.GetKeyDown(Key.U))
        {
            if(beatmapHandler != null)
                if(beatmapHandler.beatmapPlayer != null)
            beatmapHandler.beatmapPlayer.forceStop = true;
        }
        if (counter >= 1)
        {
            counter = 0;
            Debug.WriteLine(currentFps);
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