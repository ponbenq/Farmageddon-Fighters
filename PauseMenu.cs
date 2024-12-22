using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class PauseMenu : SpriteActor
    {
        private Vector2 screenSize;
        private GameManager gameManager;
        private Actor actor;
        private ImageButton resumeButton;
        public PauseMenu(Vector2 screenSize, Actor actor)
        {
            this.screenSize = screenSize;
            this.actor = actor;
            gameManager = new GameManager();
             
            var texture = TextureCache.Get("Resources/pauseMenu/pause_game.png");
            SetTexture(texture);

            var btnRegion = new TextureRegion(TextureCache.Get("Resources/pauseMenu/resume_button.png"));
            resumeButton = new ImageButton(btnRegion);
            resumeButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 - 100);
            resumeButton.Origin = btnRegion.Size / 2;
            // resumeButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 62, Color.Black, "Resume");
            resumeButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            resumeButton.ButtonClicked += Resume;
            Add(resumeButton);
        }

        public override void Act(float deltaTime)
        {
            if(!gameManager.isPaused)
                Detach();
            base.Act(deltaTime);
        }

        public void Resume(GenericButton button)
        {
            gameManager.Resume();
            Debug.WriteLine("Clicked@");
        }

    }
}