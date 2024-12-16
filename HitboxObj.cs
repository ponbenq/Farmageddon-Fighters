using ThanaNita.MonoGameTnt;
using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace GameProject;

public class HitboxObj : RectangleActor
{
    private Vector2 position;
    private int groupCode;
    private RectF rect;
    private float spanTime;
    HitCheck hitCheck;
    private float damage;
    public HitboxObj(Vector2 position, RectF rect, int groupCode, float spanTime, HitCheck hitCheck, float damage): base(Color.Transparent, rect)// new RectF(30, 15, 15, 5))
    {
        this.position = position;
        this.rect = rect;
        this.groupCode = groupCode;
        this.spanTime = spanTime;
        this.hitCheck = hitCheck;
        this.damage = damage;

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
            this.Detach();
    }
    public void OnCollide(CollisionObj objB, CollideData data)
    {
        var direction = data.objA.RelativeDirection(data.OverlapRect);
        if(objB.Actor is Player2) //if target is player2
        {
            //objB.Actor.Position += new Vector2(40, 0);
            if (data.objA.Actor is HitboxObj) //if hit by hitboxobj
            {
                Debug.WriteLine("Player1 hit player2 for " + damage.ToString());
                AddAction(new RunAction(() => hitCheck(objB.Actor, damage)));               
            }
        }        
    }
}