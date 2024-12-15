using System.Diagnostics;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework.Input;
using System.Formats.Tar;
namespace GameProject;

public class Player : PlayerAb 
{
    Animation idleAnimation, runAnimation, attAnimation_1 ;
    // public Vector2 V { get => mover.Velocity; set => mover.Velocity = value;}
    private HitboxObj hitbox;
    private AnimationStates animationState;
    private Vector2 size;
    public Player(Vector2 screenSize)
    {
        // var size = new Vector2(32, 48);
        size = new Vector2(128, 40);
        var sprite = this;
        sprite.Origin = RawSize / 2;
        sprite.Scale = new Vector2(3, 3);
        // Position = new Vector2((screenSize.X / 2 - ((size.X * sprite.Scale.X) / 2)) - 150, screenSize.Y - (100 + (size.Y * sprite.Scale.Y)));
        Position = new Vector2( 100, 100);

        // var texture = TextureCache.Get("B_witch_idle.png");
        var idleTexture = TextureCache.Get("Resources/Pic/slime/blue/Idle.png");
        var idleRegion2d = RegionCutter.Cut(idleTexture, size);
        var idleSelector = RegionSelector.Select(idleRegion2d, start:0 , count:6);
        idleAnimation = new Animation(sprite, 1.0f, idleSelector);

        var runTexture = TextureCache.Get("Resources/Pic/slime/blue/Run.png");
        var runRegion2d = RegionCutter.Cut(runTexture, size);
        var runSelector = RegionSelector.Select(runRegion2d, start:0 , count: 7);
        runAnimation = new Animation(sprite, 1.0f, runSelector);

        var attTexture = TextureCache.Get("Resources/Pic/slime/blue/Attack_1.png");
        var attRegion2d = RegionCutter.Cut(attTexture, size);
        var attSelector = RegionSelector.Select(attRegion2d, start:0, count:4);
        attAnimation_1 = new Animation(sprite, 1.0f, attSelector);

        animationState = new AnimationStates([idleAnimation, runAnimation, attAnimation_1]);
        sprite.AddAction(animationState);

        // AddAction(mover = new KeyboardMover(this, 500));
        // fall = new Fall(this, new Vector2(0, 2500));

        //collision
        var nRect = new RectF(40, 0, 48, 48);
        var collisionObj = CollisionObj.CreateWithRect(this, RawRect.CreateAdjusted(0.4f, 1), 1);
        collisionObj.DebugDraw = true;
        collisionObj.OnCollide = OnCollide;
        Add(collisionObj);

        registerJump(Keys.L, Keys.K);

    }
    public override void Act(float deltaTime)
    {

        base.Act(deltaTime);
        applyFall(deltaTime, Keys.L, DirectionKey.Direction);
        applyDirection(DirectionKey.Direction, 700);
        var keyInfo = GlobalKeyboardInfo.Value;
        var direction = DirectionKey.Direction;


        if(state == playerState.attacking)
        {
            hitbox = new HitboxObj(new Vector2(15, 32), new RectF(size.X - 36, 15, 15, 5), 1, 0.15f, hitCheck, 2f);
            Add(hitbox);
            animationState.Animate(2);
        }
        else if(direction.X > 0 || direction.X < 0)
            animationState.Animate(1);
        else
            animationState.Animate(0);

        
        Position += V * deltaTime;
        onFloor = false;
    }

    public void OnCollide(CollisionObj objB, CollideData data)
    {
        var direction = data.objA.RelativeDirection(data.OverlapRect);

        if (direction.Y == 1)
            onFloor = true;

        if (objB.Actor is Player2)
        {
            if(direction.X > 0 && direction.Y <= 0)
                objB.Actor.Position += new Vector2(20, 0);
            if(direction.X < 0 && direction.Y <= 0)
                objB.Actor.Position += new Vector2(-20, 0);
        }
        if ((direction.Y > 0 && V.Y > 0) || (direction.Y < 0 && V.Y < 0))
        {
            V = new Vector2(V.X, 0);
            Position -= new Vector2(0, data.OverlapRect.Height * direction.Y);
        }
        if ((direction.X > 0 && V.X > 0) || (direction.X < 0 && V.X < 0))
        {
            V = new Vector2(0, V.Y);
            Position -= new Vector2(data.OverlapRect.Width * direction.X, 0);
        }

    }
}