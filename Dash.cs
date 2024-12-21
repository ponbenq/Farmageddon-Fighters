using Microsoft.Xna.Framework;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class Dash : SpriteActor
    {
        private AnimationStates dashAnimationState;
        private Vector2 size;
        private Vector2 direction;
        private float timer;
        private Actor player;
        public Dash(Actor player, Vector2 direction)
        {
            this.player = player;
            size = new Vector2(32, 32);
            Scale = new Vector2(2, 2);
            var texture = TextureCache.Get("Resources/smoke/dash.png");
            var region = RegionCutter.Cut(texture, size);
            var selector = RegionSelector.Select(region, start: 0, count: 7);
            var left = new Animation(this, 1.0f, selector);

            var selector2 = RegionSelector.Select(region, start: 7, count: 7);
            var right = new Animation(this, 1.0f, selector2);
            
            dashAnimationState = new AnimationStates([left, right]);
            AddAction(dashAnimationState);
            this.direction = direction;

            timer = 0;
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            timer += deltaTime;
            if(timer >= 1f)
                this.Detach();
            if(direction.X == -1)
            {
                dashAnimationState.Animate(0);
                Position = new Vector2(player.RawRect.X - player.RawSize.X, player.RawRect.Y - (player.RawSize.Y / 3));
            }
            else if(direction.X == 1)
            {
                dashAnimationState.Animate(1);
                Position = new Vector2(player.RawRect.X + player.RawSize.X, player.RawRect.Y - (player.RawSize.Y / 3));
            }
        }
    }
}