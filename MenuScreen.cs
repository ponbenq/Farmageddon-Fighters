using Game12;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class MenuScreen : Actor
    {
        ExitNotifier exitNotifier;

        public MenuScreen(Vector2 screenSize, ExitNotifier exitNotifier)
        {
            this.exitNotifier = exitNotifier;

            //Start button
            //var startButton = new Button("Simvoni.ttf", 50, Color.Brown, "Start", new Vector2(300, 100));
            //startButton.Position = new Vector2(screenSize.X/2 , screenSize.Y/2 - 200);
            //startButton.ButtonClicked += GameStart;
            //this.Add(startButton);

            //Exit button
            var exitButton = new Button("Simvoni.ttf", 50, Color.Brown, "Exit", new Vector2(300, 100));
            exitButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2);
            exitButton.ButtonClicked += GameExit;
            this.Add(exitButton);

            //image button 1
            var region = new TextureRegion(TextureCache.Get("play_button1.png"), new RectF(0, 0, 518, 232));
            var imagePlayButton = new ImageButton(region);
            imagePlayButton.Position = new Vector2(screenSize.X / 2 - 518 / 2, screenSize.Y / 2 - 232 / 2 - 100);
            imagePlayButton.ButtonClicked += GameStart;
            this.Add(imagePlayButton);

            //image button 2

            //image button 3
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
        }

        public void GameStart(GenericButton button)
        {
            AddAction(new RunAction(() => exitNotifier(this, 1)));
        }

        public void GameExit(GenericButton button)
        {
            AddAction(new RunAction(() => exitNotifier(this, 0)));
        }
    }
}
