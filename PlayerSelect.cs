using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class PlayerSelect : SpriteActor
    {
        int playerId;
        Texture texture;
        TextureRegion region;
        public PlayerSelect(int id)
        {
            playerId = id;

            //Texture
            Origin = RawSize / 2;
            Scale = new Vector2(0.5f, 0.5f);
            SetTexture();
        }

        public void SetTexture()
        {
            if (playerId == 0)
            {
                region = new TextureRegion(TextureCache.Get("Resources/cursor/mouse.png"), new RectF(0, 0, 23, 21));
            }
            else
            {
                region = new TextureRegion(TextureCache.Get("Resources/cursor/mouse.png"), new RectF(23, 0, 23, 21));
            }
            SetTextureRegion(region);
            Scale = new Vector2(6, 6);
        }


    }
}
