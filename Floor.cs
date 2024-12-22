using Microsoft.Xna.Framework;
using ThanaNita.MonoGameTnt;

namespace GameProject;

public class Floor : SpriteActor
{
    public Floor(RectF rect, string stage)
    {
        var texture = TextureCache.Get("Resources/ground/"+stage+"_floor.png");
        
        SetTextureRegion(new TextureRegion(texture, new RectF(Vector2.Zero, rect.Size)));
        Scale = new Vector2(7,7);
        Position = rect.Position;
        var collisionObj = CollisionObj.CreateWithRect(this, 3);
        collisionObj.DebugDraw = true;
        Add(collisionObj);
    }
}