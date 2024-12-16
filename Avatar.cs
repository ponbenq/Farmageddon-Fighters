using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class Avatar : SpriteActor
    {
        Vector2 screenSize;
        Vector2 size;
        RectF nRect;
        public Avatar(int i)
        {
            size = new Vector2(240, 240);
            var sprite = this;
            this.screenSize = new Vector2(1920, 1080);
            Origin = RawSize / 2;
            Scale = new Vector2(1,1);
            var texture = TextureCache.Get("Characters.png");
            var region = new TextureRegion(texture, new RectF(i * 120, 0, 120, 120));
            SetTextureRegion(region);
        }
    }
}
