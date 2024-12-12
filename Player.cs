using GameProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using ThanaNita.MonoGameTnt;

public class Player : SpriteActor
{
    Animation animation;
    public Vector2 V { get => mover.Velocity; }
    private KeyboardMover mover;
    private HitboxObj hitbox;
    private Player2 opponent;

    public Player(Vector2 screenSize, Player2 opponent)
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
        cooltime += deltaTime;
        var keyInfo = GlobalKeyboardInfo.Value;
        Origin = RawSize / 2;

        if (distance > 0)
        {
            Scale = new Vector2(Math.Abs(Scale.X), Scale.Y);
        }
        else if (distance < 0)
        {
            Scale = new Vector2(-Math.Abs(Scale.X), Scale.Y);
        }
        if (keyInfo.IsKeyDown(Keys.K) && cooltime >= 0 && keyInfo.IsKeyPressed(Keys.K))
        {
            hitbox = new HitboxObj(new Vector2(15, 32), new RectF(30, 15, 15, 5), 1, 0.15f);
            Add(hitbox);
            cooltime -= coolFix;
        }
        Debug.WriteLine(deltaTime);
    }

    public void OnCollide(CollisionObj objB, CollideData data)
    {
        var direction = data.objA.RelativeDirection(data.OverlapRect);
        if (objB.Actor is Player2)
        {
            if (direction.X > 0 && direction.Y <= 0)
                objB.Actor.Position += new Vector2(20,0);
            if (direction.X < 0 && direction.Y <= 0)
                objB.Actor.Position += new Vector2(-20,0);
        }
    }
}
