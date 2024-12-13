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

        public void applyFall(float deltaTime, Keys input, Vector2 direction)
        {
            var keyInfo = GlobalKeyboardInfo.Value;
            if(!onFloor)
            {
                vY += rate * deltaTime;
            }
            else
            {
                // if(keyInfo.IsKeyDown(input) && onFloor)
                if((direction.Y == -1 || keyInfo.IsKeyDown(input)) && onFloor )
                    vY = -1050;
                else
                    vY = 0;
            }
        }

        public void applyDirection(Vector2 direction, float speed)
        {
            vX =  direction.X * speed;
            vY +=  direction.Y;
        }
        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);

        }
    }
}