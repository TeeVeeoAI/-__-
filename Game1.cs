using ____.GameStates;
using ____.Systems;
using ____.Systems.LoadData.LoadSettings;
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
    public static Point screenSize = new(1920, 1080);
    private FpsCounter fpsCounter;

    public Game1()
    {
        Settings settings = LoadSettings.Load();
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        screenSize = settings.ScreenSize.ToPoint();
        _graphics.PreferredBackBufferWidth = screenSize.X;
        _graphics.PreferredBackBufferHeight = screenSize.Y;
        _graphics.SynchronizeWithVerticalRetrace = settings.VSync; //VSync
        IsFixedTimeStep = false; //Uncapped FPS
        _graphics.IsFullScreen = settings.Fullscreen;
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

    static Game1 instance;
    public static Game1 Instance
    {
        get
        {   
            return instance;
        }
    }
}
