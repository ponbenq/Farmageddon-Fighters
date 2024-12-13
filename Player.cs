using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework.Input;
using System.Formats.Tar;
namespace GameProject;
using System;
using System.Diagnostics;
using ThanaNita.MonoGameTnt;

public class Player : SpriteActor
{
    private KeyboardMover mover;
    private HitboxObj hitbox;
    {
        var sprite = this;
        sprite.Origin = RawSize / 2;



        collisionObj.DebugDraw = true;
        collisionObj.OnCollide = OnCollide;
        Add(collisionObj);
    }

    private float cooltime = 0;
    private float coolFix = 0.5f;

    }
    public override void Act(float deltaTime)
    {
        base.Act(deltaTime);
        var keyInfo = GlobalKeyboardInfo.Value;

        {
        }
    }
    {
    }

    public void OnCollide(CollisionObj objB, CollideData data)
    {
        var direction = data.objA.RelativeDirection(data.OverlapRect);

        if (direction.Y == 1)
            onFloor = true;

        if (objB.Actor is Player2)
        {
        }

    }
}