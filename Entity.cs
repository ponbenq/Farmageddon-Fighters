
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ThanaNita.MonoGameTnt;

namespace GameProject
{
    public class Entity : PlayerAb
    {
        private AnimationStates animationStates;
        private Vector2 screenSize, size;
        private SoundEffect hitSound;
        private PlayerInputHandler inputHandler;
        private KeyScheme keyScheme;
        private const int speed = 400;
        public bool isFacingRight = false;
        private int collisionGroup;

        public Entity(Vector2 screenSize, Vector2 position, String spritePath, int collisionGroup, KeyScheme keyScheme, int playerNum)
        {
            // size, origin, scale, position
            size = new Vector2(50, 50);
            Origin = RawSize / 2;
            Scale = new Vector2(5, 5);
            Position = position;
            this.screenSize = screenSize;

            // init
            isFacingRight = playerNum == 2? false: true;
            this.collisionGroup = collisionGroup;

            // animation declaration
            var texture = TextureCache.Get(spritePath);
            var region = RegionCutter.Cut(texture, size);

            var idleLeft = RegionSelector.Select(region, start: 0, count:4);
            var walkLeft = RegionSelector.Select(region, start: 8, count: 6);
            var fistLeft = RegionSelector.Select(region, start: 16, count:8);
            var kickLeft = RegionSelector.Select(region, start: 24, count: 8);
            var hurtLeft = RegionSelector.Select(region, start:32, count: 4);

            var idleRight = RegionSelector.Select(region, start: 40, count: 4);
            var walkRight = RegionSelector.Select(region, start: 48, count: 6);
            var fistRight = RegionSelector.Select(region, start: 56, count: 8);
            var kickRight = RegionSelector.Select(region, start: 64, count: 8);
            var hurtRight = RegionSelector.Select(region, start: 72, count:4);

            var block = RegionSelector.Select(region, start: 80, count: 2);

            var idleL = new Animation(this, 1.0f, idleLeft);
            var walkL = new Animation(this, 1.0f, walkLeft);
            var fistL = new Animation(this, 1.0f, fistLeft);
            var kickL = new Animation(this, 1.0f, kickLeft);
            var hurtL = new Animation(this, 1.0f, hurtLeft);
            
            var idleR = new Animation(this, 1.0f, idleRight);
            var walkR = new Animation(this, 1.0f, walkRight);
            var fistR = new Animation(this, 1.0f, fistRight);
            var kickR = new Animation(this, 1.0f, kickRight);
            var hurtR = new Animation(this, 1.0f, hurtRight);

            var b = new Animation(this, 1.0f, block);

            animationStates = new AnimationStates([idleL, walkL, fistL, kickL, hurtL, 
                                                    idleR, walkR, fistR, kickR, hurtR, b]);
            AddAction(animationStates);

            // create collision object
            var collisionObj = CollisionObj.CreateWithRect(this, RawRect.CreateAdjusted(0.4f, 1), collisionGroup);
            collisionObj.DebugDraw = true;
            collisionObj.OnCollide = OnCollide;
            Add(collisionObj);

            // set keyscheme [base class method]
            this.keyScheme = keyScheme;
            inputHandler = new PlayerInputHandler(keyScheme);
            setInputHandler(keyScheme);
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            var keyInfo = GlobalKeyboardInfo.Value;
            var direction = inputHandler.getDirection(keyInfo);

            // apply gravity 
            applyFall(deltaTime);

            // move by speed
            applyDirection(direction, speed);

            // fall from screen
            screenBounding();

            // state checking && animation changes && direction
            if (isFacingRight) // on the left, facing right
            {
                if (state == playerState.idle)
                {
                    if (vX != 0)
                        animationStates.Animate(6);
                    else
                        animationStates.Animate(5);
                }
                if(state == playerState.attacking)
                {
                    var hitbox = new HitboxObj(new Vector2(0, 0), new RectF(size.X - 20, 20, 14, 12),
                                                 collisionGroup, 0.15f, hitCheck, 2f);
                    Add(hitbox);
                    animationStates.Animate(7);
                }
                if(state == playerState.dash)
                {

                }
                if(state == playerState.blocking)
                {
                    animationStates.Animate(10);
                }
            }
            else // on right, facing left
            {
                if (state == playerState.idle)
                {
                    if (vX != 0)
                        animationStates.Animate(1);
                    else
                        animationStates.Animate(0);
                }
                if(state == playerState.attacking)
                {
                    var hitbox = new HitboxObj(new Vector2(0, 0), new RectF(size.X - 20, 20, 14, 12),
                                                 collisionGroup, 0.15f, hitCheck, 2f);
                    Add(hitbox);
                    animationStates.Animate(2);
                }
                if(state == playerState.dash)
                {

                }
                if(state == playerState.blocking)
                {
                    animationStates.Animate(10);
                }
            }



            // default setting
            Position += V * deltaTime;
            onFloor = false;

            // debuging
            Debug.WriteLine(state);
        }

        private void screenBounding()
        {
            var buffer = 90f;
            if(Position.X + RawRect.Width > screenSize.X - buffer)
            {
                Position = new Vector2(screenSize.X - RawRect.Width - buffer, Position.Y);
                vX = 0;
            }
            if(Position.X < 0)
            {
                Position = new Vector2(0, Position.Y);
                vX = 0;
            }
        }

        public void OnCollide(CollisionObj objB, CollideData data)
        {
            var direction = data.objA.RelativeDirection(data.OverlapRect);

            if(objB.Actor is Floor)
            {
                if(direction.Y == 1)
                    onFloor = true;
                if((direction.Y > 0 && vY > 0) || (direction.Y < 0 && vY < 0))
                {
                    vY = 0;
                    Position -= new Vector2(0, data.OverlapRect.Height * direction.Y);
                }
                if((direction.X > 0 && vX > 0) || (direction.X < 0 && vX < 0))
                {
                    vX = 0;
                    Position -= new Vector2(data.OverlapRect.Width * direction.X, 0);
                }
            }
            if(objB.Actor is Entity otherPlayer)
            {
                vX = 0;
                Position -= new Vector2(data.OverlapRect.Width * direction.X, 0);
            }
        }
    }
}