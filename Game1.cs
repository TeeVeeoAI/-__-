using ____.GameStates;
using ____.Systems;
using ____.Systems.LoadData.LoadSettings;
using ____.Systems.SaveData.SaveSettings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ____;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState currentGameState;
    private Color bgColor = Color.CornflowerBlue;
    private Settings currentSettings;
    public static Point screenSize = new(1920, 1080);
    private FpsCounter fpsCounter;

    public Game1()
    {
        currentSettings = LoadSettings.Load();

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        screenSize = currentSettings.ScreenSize.ToPoint();
        _graphics.PreferredBackBufferWidth = screenSize.X;
        _graphics.PreferredBackBufferHeight = screenSize.Y;
        _graphics.SynchronizeWithVerticalRetrace = currentSettings.VSync; //VSync
        IsFixedTimeStep = false; //Uncapped FPS
        _graphics.IsFullScreen = currentSettings.Fullscreen;
        _graphics.ApplyChanges();
        fpsCounter = new();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        instance = this;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        currentGameState = new MenuState(this, GraphicsDevice, Content);
        currentGameState.LoadContent();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        currentGameState.Update(gameTime);

        // TODO: Add your update logic here
        fpsCounter.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(bgColor);

        // TODO: Add your drawing code here

        currentGameState.Draw(gameTime, _spriteBatch);
        _spriteBatch.Begin();
        fpsCounter.Draw(_spriteBatch, Content.Load<SpriteFont>("Fonts/DefaultFont"), new Vector2(10, 10), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void SetBGColor(Color color)
    {
        bgColor = color;
    }

    public void ChangeGameState(GameState newState)
    {
        currentGameState = newState;
        currentGameState.LoadContent();
    }

    public bool IsFullScreen => currentSettings?.Fullscreen ?? _graphics.IsFullScreen;
    public bool IsVSync => currentSettings?.VSync ?? _graphics.SynchronizeWithVerticalRetrace;

    public void ToggleFullscreen()
    {
        if (currentSettings == null)
            return;

        currentSettings.Fullscreen = !_graphics.IsFullScreen;
        _graphics.IsFullScreen = currentSettings.Fullscreen;
        _graphics.ApplyChanges();
        SaveSettings.Save(currentSettings);
    }

    public void ToggleVSync()
    {
        if (currentSettings == null)
            return;

        currentSettings.VSync = !currentSettings.VSync;
        _graphics.SynchronizeWithVerticalRetrace = currentSettings.VSync;
        _graphics.ApplyChanges();
        SaveSettings.Save(currentSettings);
    }

    public void ReloadSettings()
    {
        var loaded = LoadSettings.Load();
        if (loaded == null)
            return;

        currentSettings = loaded;
        screenSize = currentSettings.ScreenSize.ToPoint();
        _graphics.PreferredBackBufferWidth = screenSize.X;
        _graphics.PreferredBackBufferHeight = screenSize.Y;
        _graphics.SynchronizeWithVerticalRetrace = currentSettings.VSync;
        _graphics.IsFullScreen = currentSettings.Fullscreen;
        _graphics.ApplyChanges();
    }

    static Game1 instance;
    public static Game1 Instance
    {
        get
        {   
            return instance;
        }
    }
}
