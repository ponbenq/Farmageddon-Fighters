using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class GameScreen : Actor
    {
        Actor player1, player2;
        public GameScreen(Vector2 screenSize, Actor player1, Actor player2)
        {
            Add(new Floor(new RectF(0, screenSize.Y - 150, 1000, 30)));
            this.player1 = player1;
            this.player2 = player2;
            this.player1.Position = new Vector2(500, 0);
            this.player2.Position = new Vector2(600, 0);
            Add(this.player1);
            Add(this.player2);
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
        }   
    }
}
