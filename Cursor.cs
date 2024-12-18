using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Animations;

namespace GameProject
{
    public class Cursor : SpriteActor
    {
        private Vector2 size;
        private AnimationStates cursorAnimation;
        public Cursor(Actor player, int number)
        {
            size = new Vector2(11, 12);
            switch(number)
            {
                case 1:
                    preparePlayerOne();
                    setPlayerCursor(player);
                    break;
                case 2:
                    preparePlayerTwo();
                    setPlayerCursor(player);
                    break;
                default:
                    break;
            }
        }

        public void preparePlayerOne()
        {
            var texture = TextureCache.Get("Resources/cursor/cursor.png");
            var region2d = RegionCutter.Cut(texture, size);
            var selector = RegionSelector.Select(region2d, start: 0, count:8);
            var animation = new Animation(this, 1.0f, selector);
            cursorAnimation = new AnimationStates([animation]);
            AddAction(cursorAnimation);
        }
        public void preparePlayerTwo()
        {
            var texture = TextureCache.Get("Resources/cursor/cursor.png");
            var region2d = RegionCutter.Cut(texture, size);
            var selector = RegionSelector.Select(region2d, start: 8, count:8);
            var animation = new Animation(this, 1.0f, selector);
            cursorAnimation = new AnimationStates([animation]);
            AddAction(cursorAnimation);
        }

        public void setPlayerCursor(Actor player)
        {
            Position = new Vector2(player.RawRect.CenterPoint.X, player.RawRect.CenterPoint.Y - 40);
            player.Add(this);
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            cursorAnimation.Animate(0);
        }
    }
}