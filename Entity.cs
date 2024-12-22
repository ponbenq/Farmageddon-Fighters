
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
        private PlayerInputHandler inputHandler;
        private KeyScheme keyScheme;
        private const int speed = 400;
        public bool isFacingRight = false;
        private int collisionGroup;
        public int playerNum;
        public string spritePath;

        public Entity(Vector2 screenSize, Vector2 position, String spritePath, int collisionGroup, KeyScheme keyScheme, int playerNum)
        {
            // size, origin, scale, position
            size = new Vector2(50, 50);
            Origin = RawSize / 2;
            Scale = new Vector2(5, 5);
            Position = position;
            this.screenSize = screenSize;
            this.spritePath = spritePath;

            // init
            isFacingRight = playerNum == 2? false: true;
            this.playerNum = playerNum;
            this.collisionGroup = collisionGroup;

            // animation declaration
            var texture = TextureCache.Get(spritePath);
            var region = RegionCutter.Cut(texture, size);

            var idleLeft = RegionSelector.Select(region, start: 0, count:4);
            var walkLeft = RegionSelector.Select(region, start: 8, count: 6);
            var fistLeft = RegionSelector.Select(region, start: 16, count:8);
            var kickLeft = RegionSelector.Select(region, start: 24, count: 8);
            var hurtLeft = RegionSelector.Select(region, start:32, count: 3);

            var idleRight = RegionSelector.Select(region, start: 40, count: 4);
            var walkRight = RegionSelector.Select(region, start: 48, count: 6);
            var fistRight = RegionSelector.Select(region, start: 56, count: 8);
            var kickRight = RegionSelector.Select(region, start: 64, count: 8);
            var hurtRight = RegionSelector.Select(region, start: 72, count: 3);

            var block = RegionSelector.Select(region, start: 80, count: 2);
            var deadLeft = RegionSelector.Select(region, start: 35, count:1);
            var deadRight = RegionSelector.Select(region, start: 75, count: 1);

            var dyingLeft = RegionSelector.Select(region, start:32, count: 4);
            var dyingRight = RegionSelector.Select(region, start: 72, count: 4);

            var idleL = new Animation(this, 0.5f, idleLeft);
            var walkL = new Animation(this, 0.8f, walkLeft);
            var fistL = new Animation(this, 0.4f, fistLeft);
            var kickL = new Animation(this, 0.4f, kickLeft);
            var hurtL = new Animation(this, 0.3f, hurtLeft);
            
            var idleR = new Animation(this, 0.5f, idleRight);
            var walkR = new Animation(this, 0.8f, walkRight);
            var fistR = new Animation(this, 0.4f, fistRight);
            var kickR = new Animation(this, 0.4f, kickRight);
            var hurtR = new Animation(this, 0.3f, hurtRight);

            var b = new Animation(this, 0.5f, block);
            var deadL = new Animation(this, 0.5f, deadLeft);
            var deadR = new Animation(this, 0.5f, deadRight);
            var dyingL = new Animation(this, 0.5f, dyingLeft);
            var dyingR = new Animation(this, 0.5f, dyingRight);

            animationStates = new AnimationStates([idleL, walkL, fistL, kickL, hurtL, 
                                                    idleR, walkR, fistR, kickR, hurtR, b, 
                                                    deadL, deadR, dyingL, dyingR]);
            AddAction(animationStates);

            // create collision object
            var collisionObj = CollisionObj.CreateWithRect(this, RawRect.CreateAdjusted(0.6f, 1), collisionGroup);
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
                    var hitbox = new HitboxObj(new Vector2(0, 0), new RectF(30, 24, 15, 12),
                                                 collisionGroup, 0.15f, hitCheck, 2f);
                    Add(hitbox);
                    animationStates.Animate(7);
                }
                if(state == playerState.dash && stateTimer < 0.01f)
                {
                    var dash = new Dash(this, direction);
                    Add(dash);
                }
                if(state == playerState.blocking)
                {
                    animationStates.Animate(10);
                }
                if(state == playerState.hurt)
                {
                    animationStates.Animate(9);
                }
                if(state == playerState.death)
                {
                    animationStates.Animate(12);
                }
                if (state == playerState.dying)
                {
                    animationStates.Animate(14);
                }
                if(state == playerState.kicking)
                {
                    var hitbox = new HitboxObj(Vector2.Zero, new RectF(33, 26, 15, 12),
                                                collisionGroup, 0.15f, hitCheck, 15f);
                    Add(hitbox);
                    animationStates.Animate(8);
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
                    var hitbox = new HitboxObj(new Vector2(0, 0), new RectF(0, 24, 15, 12),
                                                 collisionGroup, 0.15f, hitCheck, 2f);
                    Add(hitbox);
                    animationStates.Animate(2);
                }
                if(state == playerState.dash && stateTimer < 0.01f)
                {
                    var dash = new Dash(this, direction);
                    Add(dash);
                }
                if(state == playerState.blocking)
                {
                    animationStates.Animate(10);
                }
                if(state == playerState.hurt)
                {
                    animationStates.Animate(4);
                }
                if(state == playerState.death)
                {
                    animationStates.Animate(11);
                }
                if (state == playerState.dying)
                {
                    animationStates.Animate(13);
                }
                if(state == playerState.kicking)
                {
                    var hitbox = new HitboxObj(Vector2.Zero, new RectF(3, 26, 15, 12),
                                                collisionGroup, 0.15f, hitCheck, 15f);
                    Add(hitbox);
                    animationStates.Animate(3);
                }
            }



            // default setting
            Position += V * deltaTime;
            onFloor = false;

            // debuging
            //Debug.WriteLine(state);
        }

        private void screenBounding()
        {
            var buffer = RawRect.CreateAdjusted(0.6f, 1f); // X: 10, Y: 0, Width: 30, Height: 50
            // right
            if(Position.X +((size.X * 3) + (buffer.X * 2) + buffer.Width) > screenSize.X)
            {
                Debug.WriteLine(Position);
                Position = new Vector2(screenSize.X - ((size.X * 3) + (buffer.X * 2) + buffer.Width), Position.Y);
                vX = 0;
            }
            // left
            if(Position.X + buffer.Width < 0)
            {
                Debug.WriteLine(Position);
                Position = new Vector2(-(buffer.Width), Position.Y);
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