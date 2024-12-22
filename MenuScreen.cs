using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        SoundEffect clicksound;
        Vector2 screenSize;
        SpriteActor howToPlay;
        ImageButton startButton, howToPlayButton, exitButton, backButton;
        
        public MenuScreen(Vector2 screenSize, ExitNotifier exitNotifier)
        {
            this.exitNotifier = exitNotifier;
            this.screenSize = screenSize;

            //Background
            var file = "bgmain";
            Add(new ParallaxBackground(file, screenSize, 20f, 50f, false));

            //Fighters
            var size = new Vector2(250, 50);
            var sprite = new SpriteActor();
            sprite.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 - 350);
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
            logo.Position = new Vector2(screenSize.X / 2,screenSize.Y/2 - 100);
            logo.Origin = logoRegion.Size / 2;
            logo.Scale = new Vector2(0.6f, 0.6f);
            Add(logo);
            
            var btnRegion = new TextureRegion(TextureCache.Get("Resources/img/btn.png"));
            
            //Start button
            startButton = new ImageButton(btnRegion);
            startButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 130);
            startButton.Origin = btnRegion.Size / 2;
            startButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 65,Color.DimGray, "Start");
            startButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            startButton.ButtonClicked += Playsoundclick;
            startButton.ButtonClicked += GameStart;
            Add(startButton);

            // How to play
            howToPlayButton = new ImageButton(btnRegion);
            howToPlayButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 250);
            howToPlayButton.Origin = btnRegion.Size / 2;
            howToPlayButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 65, Color.DimGray, "How to play");
            howToPlayButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            howToPlayButton.ButtonClicked += Playsoundclick;
            howToPlayButton.ButtonClicked += Howtoplay;
            Add(howToPlayButton);

            //Exit button
            exitButton = new ImageButton(btnRegion);
            exitButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 370);
            exitButton.Origin = btnRegion.Size / 2;
            exitButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 65, Color.DimGray, "Exit");
            exitButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            exitButton.ButtonClicked += GameExit;
            Add(exitButton);

            var gb_collectors = new TextureRegion(TextureCache.Get("Resources/img/logo_gb.png"));
            Add(new SpriteActor(gb_collectors) { Position = new Vector2(screenSize.X - 300, screenSize.Y - 130) , Scale = new Vector2(0.7f, 0.7f)});
        }

        private void Playsoundclick(GenericButton button)
        {
            clicksound = SoundEffect.FromFile("Resources/soundeffect/click.wav");
            clicksound.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
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
        public void Howtoplay(GenericButton button)
        {
            howToPlay = new HowToPlay(screenSize);
            howToPlay.Origin = howToPlay.RawSize / 2;
            howToPlay.Position = screenSize / 2;
            Add(howToPlay);

            var btnRegion = new TextureRegion(TextureCache.Get("Resources/img/btn.png"));
            backButton = new ImageButton(btnRegion);
            backButton.Position = new Vector2(screenSize.X / 2, screenSize.Y / 2 + 450);
            backButton.Origin = btnRegion.Size / 2;
            backButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 65, Color.DimGray, "Close");
            backButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            backButton.ButtonClicked += BackClicked;
            Add(backButton);

            startButton.Detach();
            howToPlayButton.Detach();
            exitButton.Detach();
        }

        public void BackClicked(GenericButton button)
        {
            howToPlay.Detach();
            backButton.Detach();
            Add(startButton);
            Add(howToPlayButton);
            Add(exitButton);
        }
    }
}
