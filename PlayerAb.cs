using Microsoft.Xna.Framework;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework.Input;
using System.Data;
using System.Diagnostics;

namespace GameProject
{
    public class PlayerAb : SpriteActor
    {
        public Vector2 V {get; set;}
        public float vX {get => V.X; set => V = new Vector2(value, V.Y);}
        public float vY {get => V.Y; set => V = new Vector2(V.X, value);}
        public bool onFloor {get; set;} = false;

        protected float rate = 2500f;
        public enum playerState {idle, jumping, attacking, blocking};
        public playerState state = playerState.idle;

        public Keys jumpKey, attKey;

        public delegate void onAttackDelegate(RectF rect);
        public onAttackDelegate OnAttack = null;
        public HitCheck hitCheck;
        private float stateTimer = 0f;
        public Vector2 playerDirection;

        public void applyFall(float deltaTime, Keys input, Vector2 direction)
        {
            var keyInfo = GlobalKeyboardInfo.Value;
            if(!onFloor)
            {
                vY += rate * deltaTime;
            }
        }

        public void registerJump(Keys jump, Keys att)
        {
            jumpKey = jump;
            attKey = att;
        }
        public void applyDirection(Vector2 direction, float speed)
        {
            this.playerDirection = direction;
            vX =  direction.X * speed;
            vY +=  direction.Y;
        }

        public void SetHitCheck(HitCheck hitCheck)
        {
            this.hitCheck = hitCheck;
        }
        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            stateTimer += deltaTime;
            var keyInfo = GlobalKeyboardInfo.Value;
            switch(state)
            {
                case playerState.idle:
                    if(onFloor)
                    {
                        if((keyInfo.IsKeyDown(jumpKey) || playerDirection.Y == -1) && stateTimer > 0.2f)
                        {
                            vY -= 1050;
                            changeState(playerState.jumping);
                        }
                    }
                    if(keyInfo.IsKeyDown(attKey))
                        changeState(playerState.attacking);
                    break;
                case playerState.jumping:
                    if((keyInfo.IsKeyDown(jumpKey) || playerDirection.Y == -1) && stateTimer > 0.3f)
                    {
                        vY -= 550;
                        changeState(playerState.idle);
                    }
                    if (onFloor)
                        changeState(playerState.idle);
                    break;
                case playerState.attacking:
                    if(stateTimer > 0.2f)
                    {
                        OnAttack?.Invoke(new RectF(0, 0, 40, 40));
                        changeState(playerState.idle);
                    }
                    break;
                default:
                    changeState(playerState.idle);
                    break;
            }
        }

        protected void changeState(playerState newState)
        {
            state = newState;
            stateTimer = 0f;
        }
    }
}