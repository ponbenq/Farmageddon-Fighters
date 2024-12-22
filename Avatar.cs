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
        public Avatar(string spritePath)
        {
            var sprite = this;            
            Origin = RawSize / 2;
            int index = Character.GetCharacterIndex(spritePath);            
            var texture = TextureCache.Get("Resources/sprite/char_avatars.png");
            var region = new TextureRegion(texture, new RectF(index * 41, 0, 41, 41));
            SetTextureRegion(region);
        }
    }
}
