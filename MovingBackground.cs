using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThanaNita.MonoGameTnt;
namespace GameProject
{
    internal class MovingBackground : SpriteActor
    {
        private float scrollSpeed;
        private Texture2D texture;
        private Vector2 startPosition;
        private Vector2 screenSize;
        private Vector2 player1Position, player2Position;
        private float lastCenter;
        private bool isGame;
        public MovingBackground(string file, Vector2 screenSize, Vector2 startPosition, float scrollSpeed, bool isGame)
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
            this.isGame = isGame;
        }
        public override void Act(float deltaTime)
        {
            var velocity = new Vector2(scrollSpeed, 0);
            Position += velocity * deltaTime;

            if (isGame)
            {
                var decay = 0.44f;
                var centerX = (player1Position.X + player2Position.X) / 2;

                var targetX = centerX - (screenSize.X / 2);

                var currentX = Position.X;
                var distanceToTarget = targetX - currentX;
                var scrollAdjustment = distanceToTarget * (scrollSpeed * decay) * deltaTime;

                Position += new Vector2(scrollAdjustment, 0);
            }

            if (Position.X >= GetTextureWidth())
            {
                Position = new Vector2(Position.X - GetTextureWidth() * 2, Position.Y);
            }

            Position += velocity * deltaTime;

            base.Act(deltaTime);
        }
        public float GetTextureWidth()
        {
            return texture.Width * Scale.X;
        }
        public void getPosition(Vector2 player1Position, Vector2 player2Position)
        {
            this.player1Position = player1Position;
            this.player2Position = player2Position;
        }
        public void getLastPosition(Vector2 player1Position, Vector2 player2Position)
        {
            lastCenter = Math.Abs(player1Position.X - player2Position.X) / 2;
        }
    }
}