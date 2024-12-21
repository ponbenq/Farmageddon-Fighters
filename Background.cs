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
    internal class Background : SpriteActor
    {
        private float scrollSpeed;
        private Texture2D texture;
        private Vector2 startPosition;
        private Vector2 screenSize;

        public Background(string file, Vector2 screenSize, Vector2 startPosition, float scrollSpeed)
        {
            this.scrollSpeed = scrollSpeed;
            this.screenSize = screenSize;
            this.startPosition = startPosition;

            texture = TextureCache.Get(file);

            float scaleX = screenSize.X / texture.Width;
            float scaleY = screenSize.Y / texture.Height;
            float scale = Math.Max(scaleX, scaleY);

            Position = startPosition;
            SetTextureRegion(new TextureRegion(texture, new RectF(0, 0, texture.Width, texture.Height)));
            Scale = new Vector2(scale, scale);
        }

        public override void Act(float deltaTime)
        {
            var velocity = new Vector2(scrollSpeed, 0); 
            Position += velocity * deltaTime;

            if (Position.X >= GetTextureWidth())
            {
                Position = new Vector2(Position.X - GetTextureWidth() * 2, Position.Y);
            }

            base.Act(deltaTime);
        }

        public float GetTextureWidth()
        {
            return texture.Width * Scale.X;
        }
    }



}