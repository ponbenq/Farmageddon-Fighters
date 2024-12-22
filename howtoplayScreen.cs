using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GameProject
{
    public class howtoplayScreen : Actor
    {
        ExitNotifier exitNotifier;
        SoundEffect clicksound;
        Actor actor;
        public howtoplayScreen(Vector2 ScreenSize,Actor actor)
        {
            this.actor = actor;
            var texture = TextureCache.Get("Resources/img/HowToPlay.png");
            var imageRegion = new TextureRegion(texture);
            var image = new SpriteActor(imageRegion);
            var file = "bgmain";
            Add(new ParallaxBackground(file, ScreenSize, 20f, 50f, false));
            image.Position = ScreenSize / 2; 
            image.Origin = imageRegion.Size / 2; 
            image.Scale = new Vector2(1.0f, 1.0f);
            Add(image);

            var btnRegion = new TextureRegion(TextureCache.Get("Resources/img/btn.png"));
            var backButton = new ImageButton(btnRegion);
            backButton.Position = new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2 + 450);
            backButton.Origin = btnRegion.Size / 2;
            backButton.SetButtonText("Resources/Fonts/ZFTERMIN__.ttf", 65, Color.DimGray, "Back");
            backButton.SetOutlines(0, Color.Transparent, Color.Transparent, Color.Transparent);
            backButton.ButtonClicked += Playsoundclick;
            backButton.ButtonClicked += GameBack;
            this.Add(backButton);
        }

        private void Playsoundclick(GenericButton button)
        {
            clicksound = SoundEffect.FromFile("Resources/soundeffect/click.wav");
            clicksound.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
        }
        public void GameBack(GenericButton button)
        {
            AddAction(new RunAction(() => exitNotifier(actor, 0)));
        }
    }
}
