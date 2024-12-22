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
            var file = "bgmain";
            Add(new ParallaxBackground(file, screenSize, 20f, 50f));

            //Fighters
            var size = new Vector2(250, 50);
            var sprite = new SpriteActor();
            sprite.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 - 280);
            sprite.Origin = size / 2;
            sprite.Scale = new Vector2(4, 4);
            Add(sprite);
            var texture = TextureCache.Get("Resources/sprite/menu_idle.png");
            var regions2D = RegionCutter.Cut(texture, new Vector2(250, 50));
            var regions1D = RegionSelector.Select(regions2D, start: 0, count: 4);
            var animation = new Animation(sprite, 0.6f, regions1D);
            sprite.AddAction(animation);

            //Logo
            var logoRegion = new TextureRegion(TextureCache.Get("Resources/img/logo.png"));
            var logo = new SpriteActor(logoRegion);
            logo.Position = screenSize / 2;
            logo.Origin = logoRegion.Size / 2;
            logo.Scale = new Vector2(0.6f, 0.6f);
            Add(logo);
            
            var btnRegion = new TextureRegion(TextureCache.Get("Resources/img/btn.png"));
            
            //Start button
            var startButton = new ImageButton(btnRegion);
            startButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 250);
            startButton.Origin = btnRegion.Size / 2;
            startButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 65,Color.DimGray, "Start");
            startButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            startButton.ButtonClicked += GameStart;
            this.Add(startButton);

            //Exit button
            var exitButton = new ImageButton(btnRegion);
            exitButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 380);
            exitButton.Origin = btnRegion.Size / 2;
            exitButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 65, Color.DimGray, "Exit");
            exitButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
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
