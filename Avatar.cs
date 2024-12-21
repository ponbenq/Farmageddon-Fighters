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
        Vector2 size;
        RectF nRect;
        public Avatar(int i)
        {
            size = new Vector2(50, 50);
            var sprite = this;            
            Origin = RawSize / 2;
            var texture = TextureCache.Get("Resources/sprite/characterTiles.png");
            var region = new TextureRegion(texture, new RectF(i * 50, 0, 50, 50));
            SetTextureRegion(region);
        }
    }
}
