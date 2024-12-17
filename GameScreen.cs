using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ThanaNita.MonoGameTnt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class GameScreen : Actor
    {
        float player1Hp = 100, player2Hp = 100;
        PlayerAb player1, player2;
        ProgressBar player1HpBar1, player1HpBar2, player2HpBar1, player2HpBar2;
        int countdown = 90;
        float countdownTemp, hpTemp1, hpTemp2, hitDelay1, hitDelay2, startingTimeTemp;
        Text countdownText, damage1, damage2;
        bool player1Hit, player2Hit;
        Avatar avatar1, avatar2;

        //Constants
        const float startingTime = 2f;
        const float hpDepleteDelay = 2.5f;
        const float hpDepleteRate = 1f;
        const float hitDelay = 1f;

        //State
        public enum gameStates { start, action, end}
        public gameStates state = gameStates.start;
        public GameScreen(Vector2 screenSize, Player player1, Player2 player2)
        {
            //Floor
            Add(new Floor(new RectF(0, screenSize.Y - 150, 1000, 30)));
            this.player1 = player1;
            this.player2 = player2;
            player1.SetHitCheck(HitCheck); //Pass hitcheck to Player
            player2.SetHitCheck(HitCheck);
            Add(this.player1);
            Add(this.player2);

            //HP bar
            player1HpBar1 = new ProgressBar(new Vector2(575, 75), max: 100, Color.Transparent, Color.DarkGreen);
            player1HpBar1.Origin = player1HpBar1.RawSize / 2;
            player1HpBar1.Scale = new Vector2(player1HpBar1.Scale.X * -1, player1HpBar1.Scale.Y); //invert
            player1HpBar1.Position = new Vector2(screenSize.X * 0.3f, screenSize.Y * 0.111f);
            player1HpBar2 = new ProgressBar(new Vector2(575, 75), max: 100, Color.Black, Color.Red) {Value = 100};
            player1HpBar2.Origin = player1HpBar2.RawSize / 2;
            player1HpBar2.Scale = new Vector2(player1HpBar2.Scale.X * -1, player1HpBar2.Scale.Y); //invert
            player1HpBar2.Position = new Vector2(screenSize.X * 0.3f, screenSize.Y * 0.111f);
            Add(player1HpBar2);
            Add(player1HpBar1);

            player2HpBar1 = new ProgressBar(new Vector2(575, 75), max: 100, Color.Transparent, Color.DarkGreen);
            player2HpBar1.Origin = player2HpBar1.RawSize / 2;            
            player2HpBar1.Position = new Vector2(screenSize.X * 0.7f, screenSize.Y * 0.111f);
            player2HpBar2 = new ProgressBar(new Vector2(575, 75), max: 100, Color.Black, Color.Red) {Value = 100};
            player2HpBar2.Origin = player2HpBar2.RawSize / 2;            
            player2HpBar2.Position = new Vector2(screenSize.X * 0.7f, screenSize.Y * 0.111f);
            player2HpBar2.Value = 100;
            Add(player2HpBar2);
            Add(player2HpBar1);           

            //Countdown
            countdownText = new Text("Resources/Fonts/ZFTERMIN__.ttf", 140, Color.White, "90");
            countdownText.Effect = FontStashSharp.FontSystemEffect.Stroked; //stroke font
            countdownText.EffectAmount = 3;
            countdownText.Origin = countdownText.RawSize / 2;
            countdownText.Position = new Vector2(screenSize.X / 2, screenSize.Y * 0.099f);
            Add(countdownText);

            //Avatar
            avatar1 = new Avatar(0);
            avatar1.Origin = new Vector2(60, 60);
            avatar1.Position = new Vector2(screenSize.X * 0.075f, screenSize.Y * 0.111f);
            Add(avatar1);
            avatar2 = new Avatar(1);
            avatar2.Origin = new Vector2(60, 60);
            avatar2.Position = new Vector2(screenSize.X * 0.925f, screenSize.Y * 0.111f);
            Add(avatar2);

            //Damage Text
            damage1 = new Text("ZFTERMIN__.ttf", 75, Color.Red, "20\nHIT");
            damage1.Position = new Vector2(screenSize.X * 0.075f, screenSize.Y * 0.25f);
            damage1.Origin = damage1.RawSize / 2;
            damage1.Effect = FontStashSharp.FontSystemEffect.Stroked;
            damage1.EffectAmount = 3;
            damage1.CharacterSpacing = 10;
            damage2 = new Text("ZFTERMIN__.ttf", 75, Color.Red, "20\nHIT");
            damage2.Position = new Vector2(screenSize.X * 0.925f, screenSize.Y * 0.25f);
            damage2.Origin = damage2.RawSize / 2;
            damage2.Effect = FontStashSharp.FontSystemEffect.Stroked;
            damage2.EffectAmount = 3;
            damage2.CharacterSpacing = 10;

            //Markers
            Add(new CrossHair(player1HpBar1.Position));
            Add(new CrossHair(player2HpBar1.Position));
            Add(new CrossHair(new Vector2(screenSize.X * 0.075f, screenSize.Y * 0.111f))); //player1 avatar
            Add(new CrossHair(new Vector2(screenSize.X * 0.925f, screenSize.Y * 0.111f))); //player2 avatar
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);

            //Game State
            UpdateGameState(deltaTime);            

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
                Debug.WriteLine(state);
            }

            //Hit Delay
            if (player1Hit)
            {
                hitDelay1 += deltaTime;
                if (hitDelay1 >= hitDelay)
                {
                    player1Hit = false;
                    hitDelay1 = 0f;
                    damage2.Detach();
                }
            }
            if (player2Hit)
            {
                hitDelay2 += deltaTime;
                if (hitDelay2 >= hitDelay)
                {
                    player2Hit = false;
                    hitDelay2 = 0f;
                    damage1.Detach();
                }
            }

            //Countdown
            if (state == gameStates.action) //If no one win yet or time not over (Implement gamestate in future)
            {
                countdownTemp += deltaTime;
                if (countdownTemp >= 1f)
                {
                    countdown -= 1;
                    countdownTemp = 0;
                    countdownText.Origin = countdownText.RawSize / 2;
                }
                countdownText.Str = countdown.ToString();
            }
            if (countdown <= 0)  //if time's up
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

            if (player2Hp <= 50)
            {
                AddAction(new ColorAction(3f, Color.DarkRed, avatar2));
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
                    damage2.Str = damage.ToString("0") + "\nHIT";
                    damage2.Origin = damage2.RawSize / 2;
                    Add(damage2);
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
                    damage1.Str = damage.ToString("0") + "\nHIT";
                    damage1.Origin = damage1.RawSize / 2;
                    Add(damage1);
                }
            }
        }

        public void UpdateGameState(float deltaTime)
        {
            switch (state)
            {
                case gameStates.start:
                    startingTimeTemp += deltaTime;
                    if (startingTimeTemp >= 2f)
                    {
                        state = gameStates.action;
                    }
                    break;
            }
        }
    }
}
