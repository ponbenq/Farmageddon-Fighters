using ThanaNita.MonoGameTnt;
using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace GameProject;

public class GameTest01 : Game2D
{
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
        CollisionDetectionUnit.AddDetector(1, 2);
        CollisionDetectionUnit.AddDetector(1,3);
        CollisionDetectionUnit.AddDetector(2,3);
        All.Add(new Floor(new RectF(0, ScreenSize.Y - 150, 1000, 30)));
        All.Add(new Player(ScreenSize));
        All.Add(new Player2(ScreenSize));
        var player2 = new Player2(ScreenSize);
        var player1 = new Player(ScreenSize, player2);
        All.Add(player1);
        All.Add(player2);
    }
}