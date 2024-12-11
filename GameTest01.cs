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
    protected override void LoadContent()
    {
        CollisionDetectionUnit.AddDetector(1, 2);
        All.Add(new Player(ScreenSize));
        All.Add(new Player2(ScreenSize));
    }
}