using Game12;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

            //Background
            var file = "bg2";
            Add(new MoveBackground(file, screenSize));

            var text = new Text("ZFTERMIN__.ttf", 200, Color.White, "Veggie Warriors");
            text.Origin = text.RawSize / 2;
            text.Effect = FontStashSharp.FontSystemEffect.Stroked;
            text.EffectAmount = 3;
            text.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2-50);
            this.Add(text);

            var btnSize = new Vector2(500, 100);

            //Start button
            var startButton = new Button("ZFTERMIN__.ttf", 50, Color.SteelBlue, "Start", btnSize);
            startButton.Position = new Vector2(screenSize.X/2 , screenSize.Y/2+150);
            startButton.Origin = btnSize / 2;
            startButton.ButtonClicked += GameStart;
            this.Add(startButton);

            //Exit button
            var exitButton = new Button("ZFTERMIN__.ttf", 50, Color.SteelBlue, "Exit", btnSize);
            exitButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2+270);
            exitButton.Origin = btnSize / 2;
            exitButton.ButtonClicked += GameExit;
            this.Add(exitButton);
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
