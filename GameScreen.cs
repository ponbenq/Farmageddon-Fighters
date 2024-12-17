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
        float countdownTemp, hpTemp1, hpTemp2, hitDelay1, hitDelay2, setupTimeTemp;
        Text countdownText, damage1, damage2;
        bool player1Hit, player2Hit;
        Avatar avatar1, avatar2;

        //Constants
        const float setupTime = 2f;
        const float hpDepleteDelay = 2.5f;
        const float hpDepleteRate = 1f;
        const float hitDelay = 1f;
        Color color50 = new Color(100, 100, 100);
        Color color25 = new Color(75, 75, 75);
        Color color0 = new Color(50, 50, 50);

        //State
        public enum gameStates {Setup, Start, End}
        public gameStates state = gameStates.Setup;
        public GameScreen(Vector2 screenSize, Player player1, Player2 player2)
        {
            //Floor
            Add(new Floor(new RectF(0, screenSize.Y - 150, 1000, 30)));
            this.player1 = player1;
            this.player2 = player2;
            player1.SetHitCheck(HitCheck); //Pass hitcheck to Player
            player2.SetHitCheck(HitCheck);
            //set input handler
            player1.setInputHandler(new KeyScheme(Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.L, Keys.K));
            player2.setInputHandler(new KeyScheme(Keys.W, Keys.S, Keys.D, Keys.A, Keys.Space, Keys.G));
            //Add(this.player1);
            //Add(this.player2);
            var player1Cursor = new Cursor(player1, 1);
            var player2Cursor = new Cursor(player2, 2);

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
            damage1 = new Text("Resources/Fonts/ZFTERMIN__.ttf", 75, Color.Red, "20\nHIT");
            damage1.Position = new Vector2(screenSize.X * 0.075f, screenSize.Y * 0.25f);
            damage1.Origin = damage1.RawSize / 2;
            damage1.Effect = FontStashSharp.FontSystemEffect.Stroked;
            damage1.EffectAmount = 3;
            damage1.CharacterSpacing = 10;
            damage2 = new Text("Resources/Fonts/ZFTERMIN__.ttf", 75, Color.Red, "20\nHIT");
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

            //Game Start apt
            if (state == gameStates.Setup)
            {
                setupTimeTemp += deltaTime;
                if (setupTimeTemp >= setupTime)
                {
                    changeState(gameStates.Start);
                }
            }
                  
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
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                Debug.WriteLine(state);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.End))
            {
                player1Hp = 0f;
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

            
            if (state == gameStates.Start)
            {
                //Countdown
                countdownTemp += deltaTime;
                if (countdownTemp >= 1f)
                {
                    countdown -= 1;
                    countdownTemp = 0;
                    countdownText.Origin = countdownText.RawSize / 2;
                }
                countdownText.Str = countdown.ToString();

                //End State
                if (countdown <= 0 || player1Hp <= 0 || player2Hp <= 0)
                {
                    changeState(gameStates.End);
                }

                //Low HP Avatar
                switch (player1Hp)
                {
                    case <= 50 and > 25:
                        AddAction(new ColorAction(2f, color50, avatar1));
                        break;
                    case <= 25 and > 0:
                        AddAction(new ColorAction(2f, color25, avatar1));
                        break;
                    case <= 0:
                        AddAction(new ColorAction(2f, color0, avatar1));
                        break;
                }
                switch (player2Hp)
                {
                    case <= 50 and > 25:
                        AddAction(new ColorAction(2f, color50, avatar2));
                        break;
                    case <= 25 and > 0:
                        AddAction(new ColorAction(2f, color25, avatar2));
                        break;
                    case <= 0:
                        AddAction(new ColorAction(2f, color0, avatar2));
                        break;
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

        public void changeState(gameStates newState)
        {
            state = newState;
            if (newState == gameStates.Start)
            {
                Start();
            } else if (newState == gameStates.End)
            {
                End();
            }
        }

        public void Start()
        {
            Add(player1);
            Add(player2);
        }

        private void End()
        {
            //Time's up
            if (countdown <= 0)
            {
                if (player1Hp > player2Hp) //Player1 win
                {
                    
                } else if (player2Hp > player1Hp) //Player2 win
                {

                } else //Draw
                {

                }
            }
        }
    }
}
