using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class Game1 : Game2D
    {
        Actor menuScreen;
        Actor characterSelectScreen;
        protected override void LoadContent()
        {
            menuScreen = new MenuScreen(ScreenSize, ExitNotifier);
            All.Add(menuScreen);

            
            // TODO: use this.Content to load your game content here
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
        }
        private void GameStart(Actor player1Char, Actor player2Char)
        {
            characterSelectScreen.Detach();
            //gameScreen = new GameScreen(player1Char, player2Char);

            //All.Add(gameScreen);
            CollisionDetectionUnit.AddDetector(1, 2);
        }
    }
}
