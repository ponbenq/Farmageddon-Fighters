using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThanaNita.MonoGameTnt;

public class Background : SpriteActor
{
    private float scrollSpeed = 40f; 
    private float offsetX = 0f;       
    private Texture2D texture;        
    private float textureWidth;        
    private float maxOffset;

    public Background(RectF rect, Vector2 screenSize)
    {
        texture = TextureCache.Get("Resources/Images/skybg.jpg");

        float scaleX = screenSize.X / texture.Width;
        float scaleY = screenSize.Y / texture.Height;
        float scale = Math.Max(scaleX, scaleY); 

        SetTextureRegion(new TextureRegion(texture, new RectF(0, 0, texture.Width, texture.Height)));

        Scale = new Vector2(scale, scale);
        Position = new Vector2(rect.Position.X, rect.Position.Y - 50); 

        textureWidth = texture.Width * scale; 
        maxOffset = textureWidth * 0.1f; 
    }


    public override void Act(float deltaTime)
    {
        base.Act(deltaTime);

        offsetX += scrollSpeed * deltaTime;

        if (offsetX >= maxOffset)
        {
            offsetX = maxOffset;  
            scrollSpeed = -scrollSpeed; 
        }
        else if (offsetX <= 0)
        {
            offsetX = 0;  
            scrollSpeed = -scrollSpeed; 
        }

        Position = new Vector2(-offsetX, Position.Y);
    }
}
