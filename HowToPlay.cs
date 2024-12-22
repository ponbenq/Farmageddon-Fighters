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
    public class HowToPlay : SpriteActor
    {
        public HowToPlay(Vector2 ScreenSize)
        {
            var texture = TextureCache.Get("Resources/img/how_to_play.png");
            Origin = RawSize / 2;
            Position = ScreenSize/2;
            SetTexture(texture);                    
        }       
    }
}
