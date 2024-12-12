using System.Xml.Schema;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using ThanaNita.MonoGameTnt;

namespace GameProject;

public class Player2 : SpriteActor
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
        Position = new Vector2((screenSize.X / 2 - ((size.X * sprite.Scale.X) / 2)) + 150, screenSize.Y - (100 + (size.Y * sprite.Scale.Y)));

        var texture = TextureCache.Get("B_witch_idle.png");
        var region2d = RegionCutter.Cut(texture, size);
        var selector = RegionSelector.Select(region2d, start:0 , count:6);
        animation = new Animation(sprite, 1.0f, selector);
        sprite.AddAction(animation);

        nRect = new RectF(5, 5, 20, 36);
        var collisionObj = CollisionObj.CreateWithRect(this, nRect, 2);
        collisionObj.DebugDraw = true;
        Add(collisionObj);
    }

    public override void Act(float deltaTime)
    {
        base.Act(deltaTime);
        if(Position.X > screenSize.X || Position.X + RawRect.Width < 0)
        {
            var pos = new Vector2((screenSize.X / 2 - ((size.X * Scale.X) / 2)) + 150, screenSize.Y - (100 + (size.Y * Scale.Y)));
            Position = pos;
        }
            
    }
}