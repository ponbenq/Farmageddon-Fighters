using ThanaNita.MonoGameTnt;
using System;
using Microsoft.Xna.Framework;

namespace GameProject;

public class GameTest01 : Game2D
{
    public GameTest01() : base()
    {
        BackgroundColor = Color.DarkGray;
    }

    protected override void LoadContent()
    {
        CollisionDetectionUnit.AddDetector(1, 2);
        var player2 = new Player2(ScreenSize);
        var player1 = new Player(ScreenSize, player2);
        All.Add(player1);
        All.Add(player2);
    }
}
