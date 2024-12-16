using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class GameScreen : Actor
    {
        float player1Hp = 100, player2Hp = 100;
        PlayerAb player1, player2;
        ProgressBar player1HpBar1, player1HpBar2, player2HpBar1, player2HpBar2;
        int countdown = 90;
        float countdownTemp, hpTemp1, hpTemp2, hitDelay1, hitDelay2;
        Text countdownText;
        bool player1Hit, player2Hit;

        //Constants
        const float hpDepleteDelay = 2.5f;
        const float hpDepleteRate = 2f;
        const float hitDelay = 0.4f;
        public GameScreen(Vector2 screenSize, Player player1, Player2 player2)
        {
            //Floor
            Add(new Floor(new RectF(0, screenSize.Y - 150, 1000, 30)));
            this.player1 = player1;
            this.player2 = player2;
            player1.SetHitCheck(HitCheck); //Pass hitcheck to Player
            Add(this.player1);
            Add(this.player2);

            var player1Cursor = new Cursor(player1, 1);
            var player2Cursor = new Cursor(player2, 2);

            //HP bar
            player1HpBar1 = new ProgressBar(new Vector2(600, 75), max: 100, Color.Transparent, Color.DarkGreen);
            player1HpBar1.Origin = player1HpBar1.RawSize / 2;
            player1HpBar1.Position = new Vector2(screenSize.X * 0.3f, 120);
            player1HpBar2 = new ProgressBar(new Vector2(600, 75), max: 100, Color.Black, Color.Red) {Value = 100};
            player1HpBar2.Origin = player1HpBar2.RawSize / 2;
            player1HpBar2.Position = new Vector2(screenSize.X * 0.3f, 120);
            Add(player1HpBar2);
            Add(player1HpBar1);

            player2HpBar1 = new ProgressBar(new Vector2(600, 75), max: 100, Color.Transparent, Color.DarkGreen);
            player2HpBar1.Origin = player2HpBar1.RawSize / 2;
            player2HpBar1.Position = new Vector2(screenSize.X * 0.7f, 120);
            player2HpBar2 = new ProgressBar(new Vector2(600, 75), max: 100, Color.Black, Color.Red) {Value = 100};
            player2HpBar2.Origin = player2HpBar2.RawSize / 2;
            player2HpBar2.Position = new Vector2(screenSize.X * 0.7f, 120);
            player2HpBar2.Value = 100;
            Add(player2HpBar2);
            Add(player2HpBar1);

            //Markers
            Add(new CrossHair(player1HpBar1.Position));
            Add(new CrossHair(player2HpBar1.Position));
            Add(new CrossHair(new Vector2(screenSize.X * 0.075f, 120))); //player1 avatar
            Add(new CrossHair(new Vector2(screenSize.X * 0.925f, 120))); //player2 avatar

            //Countdown
            countdownText = new Text("Resources/Fonts/ZFTERMIN__.ttf", 100, Color.White, "00");
            countdownText.Effect = FontStashSharp.FontSystemEffect.Stroked; //stroke font
            countdownText.EffectAmount = 3;            
            countdownText.Position = new Vector2(screenSize.X / 2, 120); //need fix center
            Add(countdownText);
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            countdownText.Origin = countdownText.RawSize / 2;

            //HP Bar
            player1HpBar1.Value = player1Hp;
            if (player1HpBar1.Value < player1HpBar2.Value)
            {
                hpTemp1 += deltaTime;
                if (hpTemp1 >= hpDepleteDelay) //hpbar2 deplete after hpDepleteDelay
                {
                    player1HpBar2.Value -= hpDepleteRate; //hpbar2 slowly deplete
                    if (player1HpBar2.Value == player1HpBar1.Value)
                    {
                        hpTemp1 = 0f;
                    }
                }
            }
            player2HpBar1.Value = player2Hp;
            if (player2HpBar1.Value < player2HpBar2.Value)
            {
                hpTemp2 += deltaTime;
                if (hpTemp2 >= hpDepleteDelay)
                {
                    player2HpBar2.Value -= hpDepleteRate;
                    if (player2HpBar2.Value == player2HpBar1.Value)
                    {
                        hpTemp2 = 0f;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.End))
            {
                //player2Hp = 100;
                Debug.WriteLine(countdownText.Origin);
            }

            //Hit Delay
            if (player1Hit)
            {
                hitDelay1 += deltaTime;
                if (hitDelay1 >= hitDelay)
                {
                    player1Hit = false;
                    hitDelay1 = 0f;
                }
            }
            if (player2Hit)
            {
                hitDelay2 += deltaTime;
                if (hitDelay2 >= hitDelay)
                {
                    player2Hit = false;
                    hitDelay2 = 0f;
                }
            }

            //Countdown
            if (true) //If no one win yet or time not over (Implement gamestate in future)
            {
                countdownTemp += deltaTime;
                if (countdownTemp >= 1f)
                {
                    countdown -= 1;
                    countdownTemp = 0;
                }
                countdownText.Str = countdown.ToString();
            }
            if (countdown <= 0) 
            {
                //set gameover
                if (player1Hp > player2Hp)
                {
                    //player1 win

                } else if (player2Hp > player1Hp)
                {
                    //player2 win

                } else
                {
                    //draw

                }
            }
        }   

        public void HitCheck(Actor target, float damage)
        {
            if (target is Player)
            {
                if (!player1Hit)
                {
                    player1Hp -= damage;
                    player1Hit = true;
                    Debug.WriteLine("Player1 got hit!");
                }
            }
            if (target is Player2)
            {
                if (!player2Hit)
                {
                    player2Hp -= damage;
                    //target.Position += new Vector2(40, 0);
                    player2Hit = true;
                    Debug.WriteLine("Player2 got hit!");
                }
            }
        }
    }
}
