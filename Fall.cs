using Microsoft.Xna.Framework;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework.Input;

namespace GameProject;
public class Fall : Actor
{
    Vector2 rate;
    Player2 actor;
    public Fall(Player2 actor , Vector2 rate) 
    {
        this.actor = actor;
        this.rate = rate;
    }


    public override void Act(float deltaTime)
    {
        var keyInfo = GlobalKeyboardInfo.Value;

        
        // if(!actor.onFloor)
        // {
        //     actor.V.Y += rate.Y * deltaTime;
        // }
        // else
        // {
        //     if(keyInfo.IsKeyPressed(Keys.L) && actor.onFloor)
        //     {
        //         actor.V.Y = -1000;
        //     }
        // }

        base.Act(deltaTime);
    }

}