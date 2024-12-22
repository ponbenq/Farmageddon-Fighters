using System.Diagnostics;
using Game12;
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
        private ImageButton resumeButton, mainButton;
        private ExitNotifier notifier;
        public PauseMenu(Vector2 screenSize, Actor actor, GameManager gameManager, ExitNotifier notifier)
        {
            this.screenSize = screenSize;
            this.actor = actor;
            this.gameManager = gameManager;
            this.notifier = notifier;

            var texture = TextureCache.Get("Resources/pauseMenu/pause_game.png");
            SetTexture(texture);

            var resumeRegion = new TextureRegion(TextureCache.Get("Resources/pauseMenu/resume_button.png"));
            resumeButton = new ImageButton(resumeRegion);
            resumeButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 - 100);
            resumeButton.Origin = resumeRegion.Size / 2;
            // resumeButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 62, Color.Black, "Resume");
            resumeButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            resumeButton.ButtonClicked += Resume;
            Add(resumeButton);

            var mainRegion = new TextureRegion(TextureCache.Get("Resources/pauseMenu/main_button.png"));
            mainButton = new ImageButton(mainRegion);
            mainButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 100);
            mainButton.Origin = mainRegion.Size / 2;
            mainButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            mainButton.ButtonClicked += Main;
            Add(mainButton);
        }

        public override void Act(float deltaTime)
        {
            if(!gameManager.isPaused)
            {
                Detach();
            }
            base.Act(deltaTime);
        }

        public void Resume(GenericButton button)
        {
            gameManager.Resume();
        }
        public void Main(GenericButton button)
        {
            gameManager.TogglePause();
            AddAction(new RunAction(() => notifier(actor, 0)));
        }

    }
}