using ____.GameStates;
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

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        currentGameState.Update(gameTime);

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(bgColor);

        // TODO: Add your drawing code here
        
        currentGameState.Draw(gameTime, _spriteBatch);

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
}
