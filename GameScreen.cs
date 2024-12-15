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
        float player1Hp = 100, player2Hp = 100;
        PlayerAb player1, player2;
        ProgressBar player1HpBar1, player1HpBar2, player2HpBar1, player2HpBar2;
        public GameScreen(Vector2 screenSize, Player player1, Player2 player2)
        {
            Add(new Floor(new RectF(0, screenSize.Y - 150, 1000, 30)));
            this.player1 = player1;
            this.player2 = player2;
            player1.SetHitCheck(HitCheck); //Pass hitcheck to Player
            Add(this.player1);
            Add(this.player2);

            //HP bar
            player1HpBar1 = new ProgressBar(new Vector2(200, 50), max: 100, Color.Transparent, Color.DarkGreen);
            player1HpBar1.Position = new Vector2(50, 50);
            player1HpBar2 = new ProgressBar(new Vector2(200, 50), max: 100, Color.Black, Color.Red);
            player1HpBar2.Position = new Vector2(50, 50);
            player1HpBar2.Value = 100;
            Add(player1HpBar2);
            Add(player1HpBar1);

            player2HpBar1 = new ProgressBar(new Vector2(200, 50), max: 100, Color.Transparent, Color.DarkGreen);
            player2HpBar1.Position = new Vector2(1000, 50);
            player2HpBar2 = new ProgressBar(new Vector2(200, 50), max: 100, Color.Black, Color.Red);
            player2HpBar2.Position = new Vector2(1000, 50);
            player2HpBar2.Value = 100;
            Add(player2HpBar2);
            Add(player2HpBar1);

        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);

            //delay hp
            player1HpBar1.Value = player1Hp;
            if (player1HpBar1.Value < player1HpBar2.Value)
            {
                player1HpBar2.Value -= 1f;
            }
            player2HpBar1.Value = player2Hp;
            if (player2HpBar1.Value < player2HpBar2.Value)
            {
                player2HpBar2.Value -= 1f;
            }
        }   

        public void HitCheck(Actor target, float damage)
        {
            if (target is Player2)
            {
                player2Hp -= damage;
                target.Position += new Vector2(40, 0);
            }
        }
    }
}
