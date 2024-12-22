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
using Microsoft.Xna.Framework.Audio;
using Game12;

namespace GameProject
{
    public class GameScreen : Actor
    {
        float player1Hp = 100, player2Hp = 100;
        PlayerAb player1, player2;
        ProgressBar player1HpBar1, player1HpBar2, player2HpBar1, player2HpBar2;
        int countdown = 90, start;
        float countdownTemp, hpTemp1, hpTemp2, hitDelay1, hitDelay2, setupTimeTemp, startCountdownTemp, endTime;
        Text countdownText, damage1, damage2, centerText;
        bool player1Hit, player2Hit, fightSfxPlayed;
        Avatar avatar1, avatar2;
        private SoundEffect hurtsound;
        ExitNotifier exitNotifier;

        GameManager manager;
        //Constants
        const float setupTime = 4f;
        const float hpDepleteDelay = 2.5f;
        const float hpDepleteRate = 1f;
        const float hitDelay = 1f;
        Color color50 = new Color(100, 100, 100);
        Color color25 = new Color(75, 75, 75);
        Color color0 = new Color(50, 50, 50);

        //State
        public enum gameStates {Setup, Start, End}
        public gameStates state = gameStates.Setup;
        public GameScreen(Vector2 screenSize, Entity player1, Entity player2, ExitNotifier exitNotifier, string stage)
        {
            this.exitNotifier = exitNotifier;

            //Background
            Add(new ParallaxBackground(stage, screenSize, 50f, 100f));

            //Floor
            Add(new Floor(new RectF(0, screenSize.Y - 150, 1000, 50)));
            this.player1 = player1;
            this.player2 = player2;
            player1.SetHitCheck(HitCheck); //Pass hitcheck to Player
            player2.SetHitCheck(HitCheck);
            //set input handler
            // player1.setInputHandler(new KeyScheme(Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.L, Keys.K));
            // player2.setInputHandler(new KeyScheme(Keys.W, Keys.S, Keys.D, Keys.A, Keys.Space, Keys.G));
            //Add(this.player1);
            //Add(this.player2);
            var player1Cursor = new Cursor(player1, 1);
            var player2Cursor = new Cursor(player2, 2);

            //HP bar
            player1HpBar1 = new ProgressBar(new Vector2(550, 60), max: 100, Color.Transparent, Color.Teal);
            player1HpBar1.Origin = player1HpBar1.RawSize / 2;
            player1HpBar1.Scale = new Vector2(player1HpBar1.Scale.X * -1, player1HpBar1.Scale.Y); //invert
            player1HpBar1.Position = new Vector2(screenSize.X * 0.3f, screenSize.Y * 0.111f);
            player1HpBar2 = new ProgressBar(new Vector2(550, 60), max: 100, Color.Black, Color.DarkRed) {Value = 100};
            player1HpBar2.Origin = player1HpBar2.RawSize / 2;
            player1HpBar2.Scale = new Vector2(player1HpBar2.Scale.X * -1, player1HpBar2.Scale.Y); //invert
            player1HpBar2.Position = new Vector2(screenSize.X * 0.3f, screenSize.Y * 0.111f);
            Add(player1HpBar2);
            Add(player1HpBar1);

            player2HpBar1 = new ProgressBar(new Vector2(550, 60), max: 100, Color.Transparent, Color.Teal);
            player2HpBar1.Origin = player2HpBar1.RawSize / 2;            
            player2HpBar1.Position = new Vector2(screenSize.X * 0.7f, screenSize.Y * 0.111f);
            player2HpBar2 = new ProgressBar(new Vector2(550, 60), max: 100, Color.Black, Color.DarkRed) {Value = 100};
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
            avatar1 = new Avatar(player1.spritePath);
            avatar1.Origin = avatar1.RawSize / 2;
            avatar1.Scale = new Vector2(-3,3);
            avatar1.Position = new Vector2(screenSize.X * 0.075f, screenSize.Y * 0.111f);
            Add(avatar1);
            avatar2 = new Avatar(player2.spritePath);
            avatar2.Origin = avatar2.RawSize / 2;
            avatar2.Scale = new Vector2(3, 3);
            avatar2.Position = new Vector2(screenSize.X * 0.925f, screenSize.Y * 0.111f);
            Add(avatar2);

            //Damage Text
            damage1 = new Text("Resources/Fonts/ZFTERMIN__.ttf", 75, Color.White, "20\nHIT");
            damage1.Position = new Vector2(screenSize.X * 0.09f, screenSize.Y * 0.25f);
            damage1.Origin = damage1.RawSize / 2;
            damage1.Effect = FontStashSharp.FontSystemEffect.Stroked;
            damage1.EffectAmount = 3;
            damage1.CharacterSpacing = 10;
            damage2 = new Text("Resources/Fonts/ZFTERMIN__.ttf", 75, Color.White, "20\nHIT");
            damage2.Position = new Vector2(screenSize.X * 0.91f, screenSize.Y * 0.25f);
            damage2.Origin = damage2.RawSize / 2;
            damage2.Effect = FontStashSharp.FontSystemEffect.Stroked;
            damage2.EffectAmount = 3;
            damage2.CharacterSpacing = 10;

            //Center Text
            centerText = new Text("Resources/Fonts/ZFTERMIN__.ttf", 300, Color.White, "3");
            centerText.Origin = centerText.RawSize / 2;
            centerText.Position = screenSize/2;
            centerText.Effect = FontStashSharp.FontSystemEffect.Stroked;
            centerText.EffectAmount = 3;
            Add(centerText);
            start = ((int)setupTime) - 1;

            //Markers
            Add(new CrossHair(player1HpBar1.Position));
            Add(new CrossHair(player2HpBar1.Position));
            Add(new CrossHair(new Vector2(screenSize.X * 0.075f, screenSize.Y * 0.111f))); //player1 avatar
            Add(new CrossHair(new Vector2(screenSize.X * 0.925f, screenSize.Y * 0.111f))); //player2 avatar

            manager = new GameManager();
        }

        public void checkPlayerCross(Entity player1, Entity player2)
        {
            if(player1.Position.X > player2.Position.X)
            {
                player1.isFacingRight = false;
                player2.isFacingRight = true;
            }
            else
            {
                player1.isFacingRight = true;
                player2.isFacingRight = false;
            }
                
        }
        public override void Act(float deltaTime)
        {
            var keyInfo = GlobalKeyboardInfo.Value;
            if(keyInfo.IsKeyPressed(Keys.V))
                manager.TogglePause();
            if(manager.isPaused)
                return;
            base.Act(deltaTime);
            StartCountdown(deltaTime);

            // cross checking
            checkPlayerCross((Entity)player1, (Entity)player2);
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
            if (GlobalKeyboardInfo.Value.IsKeyPressed(Keys.PageUp))
            {
                player1Hp = 1f;
            }
            if (GlobalKeyboardInfo.Value.IsKeyPressed(Keys.PageDown))
            {
                player2Hp = 1f;
            }
            if (GlobalKeyboardInfo.Value.IsKeyPressed(Keys.End))
            {
                countdown = 2;
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
            
            if (state == gameStates.End)
            {
                endTime += deltaTime;
                if (endTime >= 4f)
                {
                    AddAction(new RunAction(() => exitNotifier(this, 0)));
                }
            }
        }   

        public void HitCheck(Actor target, float damage)
        {
            hurtsound = SoundEffect.FromFile("Resources/soundeffect/hurt.wav");
            var blockSfx = SoundEffect.FromFile("Resources/soundeffect/block.wav");
            var blocked = false;
            if (damage == 0f) { blocked = true; }
            if (target is Entity player)
            {
                if (!player1Hit && player.playerNum == 1)
                {
                    if (!blocked)
                    {
                        player.changeState(PlayerAb.playerState.hurt);
                        player1Hp -= damage;
                        player1Hit = true;                        
                        damage2.Str = damage.ToString("0") + "\nHIT";
                        damage2.Color = Color.White;
                        damage2.Origin = damage2.RawSize / 2;
                        hurtsound.Play();
                        Add(damage2);                       
                    } else
                    {
                        player1Hit = true;
                        blockSfx.Play();
                        damage2.Str = "BLOCKED";
                        damage2.Color = Color.Red;
                        damage2.Origin = damage2.RawSize / 2;
                        Add(damage2);                        
                    }
                    
                }
                if(!player2Hit && player.playerNum == 2)
                {
                    if (!blocked)
                    {
                        player.changeState(PlayerAb.playerState.hurt);
                        player2Hp -= damage;
                        //target.Position += new Vector2(40, 0);
                        player2Hit = true;                        
                        damage1.Str = damage.ToString("0") + "\nHIT";
                        damage1.Color = Color.White;
                        damage1.Origin = damage1.RawSize / 2;
                        hurtsound.Play();
                        Add(damage1);
                    } else
                    {
                        player2Hit = true;
                        blockSfx.Play();
                        damage1.Str = "BLOCKED";
                        damage1.Color = Color.Red;
                        damage1.Origin = damage2.RawSize / 2;
                        Add(damage1);
                    }                    
                }
            }
        }

        public void changeState(gameStates newState)
        {
            state = newState;
            if (newState == gameStates.Start)
            {
                Start();
                centerText.Detach();
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
            var player1Win = SoundEffect.FromFile("Resources/soundeffect/announcer/Player1Win.wav");
            var player2Win = SoundEffect.FromFile("Resources/soundeffect/announcer/Player2Win.wav");
            var draw = SoundEffect.FromFile("Resources/soundeffect/announcer/Draw.wav");

            //Any player hp <= 0
            if (player2Hp <= 0)
            {
                centerText.Str = "Player1 Win";
                centerText.Origin = centerText.RawSize / 2;
                Add(centerText);
                player2.changeState(PlayerAb.playerState.dying);
                player1Win.Play();
                
            } else if (player1Hp <= 0)
            {
                centerText.Str = "Player2 Win";
                centerText.Origin = centerText.RawSize / 2;
                Add(centerText);
                player1.changeState(PlayerAb.playerState.dying);
                player2Win.Play();
            }

            //Time's up
            if (countdown <= 0)
            {
                if (player1Hp > player2Hp) //Player1 win
                {
                    centerText.Str = "Player1 Win";
                    centerText.Origin = centerText.RawSize / 2;
                    Add(centerText);
                    player1Win.Play();
                    player2.changeState(PlayerAb.playerState.dying);
                } else if (player2Hp > player1Hp) //Player2 win
                {
                    centerText.Str = "Player2 Win";
                    centerText.Origin = centerText.RawSize / 2;
                    Add(centerText);
                    player2Win.Play();
                    player1.changeState(PlayerAb.playerState.dying);
                } else //Draw
                {
                    centerText.Str = "Draw";
                    centerText.Origin = centerText.RawSize / 2;
                    Add(centerText);
                    draw.Play();
                }
            }
        }

        public void StartCountdown (float deltaTime)
        {
            if (state == gameStates.Setup)
            {
                startCountdownTemp += deltaTime;
                if (startCountdownTemp >= 1f && start != 0)
                {
                    start -= 1;
                    centerText.Str = start.ToString();
                    centerText.Origin = centerText.RawSize / 2;
                    startCountdownTemp = 0f;
                }
                if (start == 0)
                {
                    centerText.Str = "Fight!";
                    centerText.Origin = centerText.RawSize / 2;
                    if (!fightSfxPlayed)
                    {
                        var fightSfx = SoundEffect.FromFile("Resources/soundeffect/announcer/fight.wav");
                        fightSfx.Play();
                        fightSfxPlayed = true;
                    }                    
                }
            }                      
        }
    }
}
