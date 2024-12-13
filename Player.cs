using GameProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using ThanaNita.MonoGameTnt;

public class Player : PlayerAb 
{
    Animation idleAnimation, runAnimation, attAnimation_1 ;
    // public Vector2 V { get => mover.Velocity; set => mover.Velocity = value;}
    private HitboxObj hitbox;
    private AnimationStates animationState;
    private Vector2 size;
    public bool onFloor{get; set;}
    private Fall fall;
    public Player(Vector2 screenSize)
    {
        this.opponent = opponent;
        var size = new Vector2(32, 48);
        var sprite = this;
        sprite.Origin = RawSize / 2;
        sprite.Scale = new Vector2(6, 6);
        Position = new Vector2((screenSize.X / 2 - ((size.X * sprite.Scale.X) / 2)) - 150, screenSize.Y - (100 + (size.Y * sprite.Scale.Y)));

        var texture = TextureCache.Get("B_witch_idle.png");
        var region2d = RegionCutter.Cut(texture, size);
        var selector = RegionSelector.Select(region2d, start: 0, count: 6);
        animation = new Animation(sprite, 1.0f, selector);
        sprite.AddAction(animation);

        AddAction(mover = new KeyboardMover(this, 500));

        // Collision
        var nRect = new RectF(5, 5, 20, 36);
        var collisionObj = CollisionObj.CreateWithRect(this, nRect, 1);
        collisionObj.DebugDraw = true;
        collisionObj.OnCollide = OnCollide;
        Add(collisionObj);
    }

    private float cooltime = 0;
    private float coolFix = 0.5f;

    public override void Act(float deltaTime)
    {
        float distance = opponent.Position.X - Position.X;
        base.Act(deltaTime);
        changeVy(deltaTime);
        // fall.Act(deltaTime);

        var keyInfo = GlobalKeyboardInfo.Value;
        Origin = RawSize / 2;

        V.X = direction.X * 700;

        if(keyInfo.IsKeyPressed(Keys.K) )
        {
            Scale = new Vector2(Math.Abs(Scale.X), Scale.Y);
            Position = new Vector2(Position.X, Position.Y);
        }
        else if (distance < 0)
        {
            Scale = new Vector2(-Math.Abs(Scale.X), Scale.Y);

        
        Position += V * deltaTime;
        onFloor = false;

    }
    private void changeVy(float deltaTime)
    {
        Vector2 g = new Vector2(0, 2500);
        V.Y += g.Y * deltaTime;

        var keyInfo = GlobalKeyboardInfo.Value;

        if(keyInfo.IsKeyPressed(Keys.Space) && onFloor)
            V.Y = -750;
    }

    public void OnCollide(CollisionObj objB, CollideData data)
    {
        var direction = data.objA.RelativeDirection(data.OverlapRect);
        if (objB.Actor is Player2)
        {
            if (direction.X > 0 && direction.Y <= 0)
                objB.Actor.Position += new Vector2(20, 0);
            if (direction.X < 0 && direction.Y <= 0)
                objB.Actor.Position += new Vector2(-20, 0);
        }
    }
}