using Microsoft.Xna.Framework;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework.Input;
using System.Data;
using System.Diagnostics;

namespace GameProject
{
    public class PlayerAb : SpriteActor
    {
        public Vector2 V { get; set; }
        public float vX { get => V.X; set => V = new Vector2(value, V.Y); }
        public float vY { get => V.Y; set => V = new Vector2(V.X, value); }
        public bool onFloor { get; set; } = false;

        private SoundEffect jumpsound;
        private SoundEffect dashsound;
        private bool hasPlayedDashSound = false;

        protected float rate = 2500f;
        public enum playerState {idle, jumping, attacking, blocking, dash, hurt, death};
        public playerState state = playerState.idle;

        public Keys jumpKey, attKey;

        public delegate void onAttackDelegate(RectF rect);
        public onAttackDelegate OnAttack = null;
        public HitCheck hitCheck;
        public float stateTimer = 0f;
        public Vector2 playerDirection;
        private PlayerInputHandler inputHandler;
        private KeyScheme keyScheme;
        private Vector2 dashDirection = Vector2.Zero;

        public void applyFall(float deltaTime)
        {
            var keyInfo = GlobalKeyboardInfo.Value;
            jumpsound = SoundEffect.FromFile("Resources/soundeffect/jump.wav");
            if (!onFloor)
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
            if(state == playerState.dash)
                return;
            this.playerDirection = direction;
            vX =  direction.X * speed;
            vY +=  direction.Y;
        }

        public void SetHitCheck(HitCheck hitCheck)
        {
            this.hitCheck = hitCheck;
        }

        public void setInputHandler(KeyScheme scheme)
        {
            this.keyScheme = scheme;
            inputHandler = new PlayerInputHandler(scheme);
        }
        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            stateTimer += deltaTime;
            var keyInfo = GlobalKeyboardInfo.Value;
            var pressedTime = stateTimer;
            switch(state)
            {
                case playerState.idle:
                    if(onFloor)
                    {
                        if(inputHandler.isJumpPressed(keyInfo, playerDirection) && stateTimer > 0.2f)
                        {
                            vY -= 1050;
                            changeState(playerState.jumping);
                        }
                    }
                    if(inputHandler.isAttackPressed(keyInfo))
                        changeState(playerState.attacking);
                    if(inputHandler.isDoublePressed(keyScheme.right, pressedTime) && inputHandler.getDirection(keyInfo).X == 1 && stateTimer > 0.2f)
                    {
                        vX = 0;
                        dashDirection = inputHandler.getDirection(keyInfo);
                        changeState(playerState.dash);
                    }
                    if (inputHandler.isDoublePressed(keyScheme.left, pressedTime) && inputHandler.getDirection(keyInfo).X == -1 && stateTimer > 0.2f)
                    {
                        vX = 0;
                        dashDirection = inputHandler.getDirection(keyInfo);
                        changeState(playerState.dash);
                    }
                    if(inputHandler.isBlockingPressed(keyInfo))
                    {
                        vX = 0;
                        changeState(playerState.blocking);
                    }
                    break;
                case playerState.jumping:
                    if(inputHandler.isJumpPressed(keyInfo, playerDirection) && stateTimer > 0.3f)
                    {
                        vY -= 550;
                        changeState(playerState.idle);
                    }
                    if (onFloor)
                        changeState(playerState.idle);
                    break;
                case playerState.attacking:
                    if (stateTimer > 0.4f)
                    {
                        // OnAttack?.Invoke(new RectF(0, 0, 40, 40));
                        changeState(playerState.idle);
                    }
                    break;
                case playerState.dash:
                    dashsound = SoundEffect.FromFile("Resources/soundeffect/dash2.wav");
                    var speed = 200f;
                    var acc = 1.2f;
                    var decay = 0.75f;

                    if (!hasPlayedDashSound) 
                    {
                        dashsound.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                        hasPlayedDashSound = true; 
                    }

                    if (stateTimer <= 0.18f)
                    {
                        var smoothDash = speed * acc;
                        vX += dashDirection.X > 0 ? smoothDash : -smoothDash;
                        acc *= decay;
                    }
                    else
                    {
                        changeState(playerState.idle);
                        hasPlayedDashSound = false;
                    }
                    break;
                case playerState.blocking:
                    if(!inputHandler.isBlockingPressed(keyInfo) && stateTimer > 0.2f)
                    {
                        changeState(playerState.idle);
                    }
                    break;
                case playerState.hurt:
                    if(stateTimer > 0.2f)
                    {
                        changeState(playerState.idle);
                    }
                    break;
                default:
                    changeState(playerState.idle);
                    break;
            }
        }

        public void changeState(playerState newState)
        {
            state = newState;
            stateTimer = 0f;
        }
    }
}