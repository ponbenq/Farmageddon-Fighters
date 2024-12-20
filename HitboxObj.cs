using ThanaNita.MonoGameTnt;
using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace GameProject;

public class HitboxObj : RectangleActor
{
    private Vector2 position;
    private int groupCode;
    private RectF rect;
    private float spanTime;
    HitCheck hitCheck;
    private float damage;
    private SoundEffect hurtsound;
    public HitboxObj(Vector2 position, RectF rect, int groupCode, float spanTime, HitCheck hitCheck, float damage) : base(Color.Transparent, rect)// new RectF(30, 15, 15, 5))
    {
        this.position = position;
        this.rect = rect;
        this.groupCode = groupCode;
        this.spanTime = spanTime;
        this.hitCheck = hitCheck;
        this.damage = damage;
        hurtsound = SoundEffect.FromFile("Resources/soundeffect/hurt.wav");

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
        if (time >= spanTime)
            this.Detach();
    }
    public void OnCollide(CollisionObj objB, CollideData data)
    {
        var direction = data.objA.RelativeDirection(data.OverlapRect);
        if (objB.Actor is PlayerAb otherPlayer) //if target is player2
        {
            // TODO: [ ] check blocking
            //       [ ] create player object for handle response
            //       [ ] attack delay
            //objB.Actor.Position += new Vector2(40, 0);
            if(!(otherPlayer.state == PlayerAb.playerState.blocking))
            {
                otherPlayer.changeState(PlayerAb.playerState.hurt);
                if (data.objA.Actor is HitboxObj) //if hit by hitboxobj
                {
                    Debug.WriteLine("Player1 hit player2 for " + damage.ToString());
                    // hurtsound.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
                    AddAction(new RunAction(() => hitCheck(objB.Actor, damage)));
                }
            }
        }
    }
}