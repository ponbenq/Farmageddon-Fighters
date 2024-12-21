using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class Game1 : Game2D
    {
        Actor menuScreen, characterSelectScreen, gameScreen;
        private SoundEffect bgmsong;
        private SoundEffectInstance bgmInstance;

        public Game1()
        {
            BackgroundColor = Color.DarkGray;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //BGM Music
            //bgmsong = SoundEffect.FromFile("Resources/soundeffect/bgm.wav");
            //bgmInstance = bgmsong.CreateInstance();
            //bgmInstance.IsLooped = true;
            //bgmInstance.Volume = 0.5f;
            //bgmInstance.Play();

            menuScreen = new MenuScreen(ScreenSize, ExitNotifier);
            All.Add(menuScreen);
        }

        private void ExitNotifier(Actor actor, int code) 
        {
            if (actor == null)
                return;
            if (actor == menuScreen)
            {
                switch (code)
                {
                    case 0: //Exit case
                        Exit();
                        break;
                    case 1: //Start case
                        menuScreen.Detach();
                        characterSelectScreen = new CharacterSelectScreen(ScreenSize, GameStart);
                        All.Add(characterSelectScreen);
                        return;
                }
            }
            if (actor == gameScreen)
            {
                gameScreen.Detach();
                All.Add(menuScreen);
            }
        }
        private void GameStart(string player1Sprite, string player2Sprite)
        {
            CollisionDetectionUnit.AddDetector(1, 2);
            CollisionDetectionUnit.AddDetector(1, 3);
            CollisionDetectionUnit.AddDetector(2, 3);
            CollisionDetectionUnit.AddDetector(2, 4);
            characterSelectScreen.Detach();
            var player1 = new Entity(ScreenSize, new Vector2(100, 100), player1Sprite,
                                1, new KeyScheme(Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.L, Keys.K), 1);
            var player2 = new Entity(ScreenSize, new Vector2(ScreenSize.X - 300, 100), player2Sprite,
                                    2, new KeyScheme(Keys.W, Keys.S, Keys.D, Keys.A, Keys.Space, Keys.F), 2);
            gameScreen = new GameScreen(ScreenSize, player1, player2, ExitNotifier);
            All.Add(gameScreen);
        }
    }
}
