using System.Xml.Schema;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace GameProject;

public class Player2 : PlayerAb 
{
    Animation animation;
    Vector2 screenSize;
    Vector2 size;
    RectF nRect;

    public Player2(Vector2 screenSize)
    {
        size = new Vector2(32, 48);
        var sprite = this;
        this.screenSize = screenSize;
        sprite.Origin = RawSize / 2;
        sprite.Scale = new Vector2(6, 6);
        // Position = new Vector2((screenSize.X / 2 - ((size.X * sprite.Scale.X) / 2)) + 150, screenSize.Y - (100 + (size.Y * sprite.Scale.Y)));
        Position = new Vector2(600, 100);

        var texture = TextureCache.Get("B_witch_idle.png");
        var region2d = RegionCutter.Cut(texture, size);
        var selector = RegionSelector.Select(region2d, start:0 , count:6);
        animation = new Animation(sprite, 1.0f, selector);
        sprite.AddAction(animation);

        nRect = new RectF(5, 5, 20, 36);
        var collisionObj = CollisionObj.CreateWithRect(this, nRect, 2);
        collisionObj.DebugDraw = true;
        collisionObj.OnCollide = OnCollide;
        Add(collisionObj);

        registerJump(Keys.Space, Keys.G);
        OnAttack = IsAttack;
    }

    public override void Act(float deltaTime)
    {
        base.Act(deltaTime);
        //base class perform
        applyFall(deltaTime);
        applyDirection(DirectionWASD.Direction, 500);
        
        if(Position.X > screenSize.X || Position.X + RawRect.Width < 0)
        {
            var pos = new Vector2((screenSize.X / 2 - ((size.X * Scale.X) / 2)) + 150, screenSize.Y - (100 + (size.Y * Scale.Y)));
            Position = pos;
        }
        if(state == playerState.attacking)
        {
            var hitbox = new HitboxObj(new Vector2(0, 0), new RectF(35, 15, 20, 10), 2, 0.15f, hitCheck, 0f);
            Add(hitbox);
        }
        if(state == playerState.dash)
        {
            var dash = new Dash(this, DirectionWASD.Direction);
            Add(dash);
        }

        float buffer = 90f;
        if (Position.X + RawRect.Width > screenSize.X - buffer)
        {
            Position = new Vector2(screenSize.X - RawRect.Width - buffer, Position.Y);
            V = new Vector2(0, V.Y);
        }

        if (Position.X < 0)
        {
            Position = new Vector2(0, Position.Y);
            V = new Vector2(0, V.Y);
        }
        Position += V * deltaTime;
        onFloor = false;
        Debug.WriteLine(this.GetMatrix());
    }

    public void OnCollide(CollisionObj objB, CollideData data)
    {
        var direction = data.objA.RelativeDirection(data.OverlapRect);

        if(objB.Actor is Floor)
        {
            if (direction.Y == 1)
                onFloor = true;
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
    public void IsAttack(RectF rect)
    {
        Debug.WriteLine("hello from IsAttack!");
        Debug.WriteLine($"the rect is : {rect}");
    }
}