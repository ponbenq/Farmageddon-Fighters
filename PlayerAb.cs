using Microsoft.Xna.Framework;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class PlayerAb : SpriteActor
    {
        public Vector2 V {get; set;}
        public float vX {get => V.X; set => V = new Vector2(value, V.Y);}
        public float vY {get => V.Y; set => V = new Vector2(V.X, value);}
        public bool onFloor {get; set;} = false;

        protected float rate = 2500f;

        public void applyFall(float deltaTime)
        {
            var keyInfo = GlobalKeyboardInfo.Value;
            if(!onFloor)
            {
                vY += rate * deltaTime;
            }
            else
            {
                if(keyInfo.IsKeyDown(Keys.L) && onFloor)
                    vY = -1050;
                else
                    vY = 0;
            }
        }

        public void applyDirection(Vector2 direction, float speed)
        {
            vX =  direction.X * speed;
        }
        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);

        }
    }
}