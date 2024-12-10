using ThanaNita.MonoGameTnt;
using System;
using Microsoft.Xna.Framework;

namespace GameProject;

public class HitboxObj : RectangleActor
{
    private Vector2 position;
    private int groupCode;
    private RectF rect;
    private float spanTime;
    public HitboxObj(Vector2 position, RectF rect, int groupCode, float spanTime): base(Color.LightBlue, new RectF(30, 15, 15, 5))
    {
        this.position = position;
        this.rect = rect;
        this.groupCode = groupCode;
        this.spanTime = spanTime;

        var collistionObj = CollisionObj.CreateWithRect(this, groupCode);
        collistionObj.OnCollide = OnCollide;
        collistionObj.DebugDraw = true;
        Add(collistionObj);
    }

    private float time = 0f;
    public override void Act(float deltaTime)
    {
        base.Act(deltaTime);
        time += deltaTime;
        if(time >= spanTime)
            this.Detach();
    }
    public void OnCollide(CollisionObj objB, CollideData data)
    {
        var direction = data.objA.RelativeDirection(data.OverlapRect);
        if(objB.Actor is Player2)
        {
            objB.Actor.Position += new Vector2(40, 0);
        }
    }
}