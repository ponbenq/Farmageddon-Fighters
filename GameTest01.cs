using ThanaNita.MonoGameTnt;
using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject;

public class GameTest01 : Game2D
{
    private SoundEffect bgmsong;
    private SoundEffectInstance bgmInstance;
    public GameTest01() : base()
    {
        BackgroundColor = Color.DarkGray;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        //BGM Music

        bgmsong = SoundEffect.FromFile("Resources/soundeffect/bgm.wav");
        bgmInstance = bgmsong.CreateInstance();
        bgmInstance.IsLooped = true;
        bgmInstance.Volume = 0.5f;
        bgmInstance.Play();

        CollisionDetectionUnit.AddDetector(1, 2);
        CollisionDetectionUnit.AddDetector(1,3);
        CollisionDetectionUnit.AddDetector(2,3);
        CollisionDetectionUnit.AddDetector(2,4);
        var player1 = new Entity(ScreenSize, new Vector2(100, 100), "Resources/sprite/pumpkin/pumpkin.png",
                                1, new KeyScheme(Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.L, Keys.K), 1);
        var player2 = new Entity(ScreenSize, new Vector2(ScreenSize.X - 300, 100), "Resources/sprite/broccoli/broccoli_test.png",
                                2, new KeyScheme(Keys.W, Keys.S, Keys.D, Keys.A, Keys. Space, Keys.F), 2);
        All.Add(new GameScreen(ScreenSize, player1, player2));
        //All.Add(new Floor(new RectF(0, ScreenSize.Y - 150, 1000, 30)));
        //All.Add(new PlayerHolder1(ScreenSize));
        //All.Add(new Player(ScreenSize));
        //All.Add(new Player2(ScreenSize));
        //CollisionDetectionUnit.AddDetector(1, 3);
        //CollisionDetectionUnit.AddDetector(2, 3);
        //All.Add(new Floor(new RectF(0, ScreenSize.Y - 150, 1000, 30)));
        //var player2 = new Player2(ScreenSize);
        //var player1 = new Player(ScreenSize);
        //All.Add(player1);
        //All.Add(player2);
    }
}