using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class Game1 : Game2D
    {
        Actor menuScreen;    

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
                        return;
                }
            }

            
        }        
    }
}
