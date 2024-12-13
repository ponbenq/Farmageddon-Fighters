using Microsoft.Xna.Framework;

namespace ThanaNita.MonoGameTnt
{
    public struct DrawState
    {
        public Matrix3 WorldMatrix {  get; }
        //public Color Color { get => ColorF.ToColor(); } // may loss some precision
        public ColorF ColorF {  get; }
        public bool FlipHorizontal {get; set;}
        public bool FlipVerical {get; set;}

        public DrawState()
        {
            WorldMatrix = Matrix3.Identity;
            ColorF = (ColorF)Color.White;
            FlipHorizontal = false;
            FlipVerical = false;
        }

/*        public DrawState(Matrix3 worldMatrix, Color color)
        {
            WorldMatrix = worldMatrix;
            ColorF = (ColorF)color;
        }*/

        public DrawState(Matrix3 worldMatrix, ColorF colorF)
        {
            WorldMatrix = worldMatrix;
            ColorF = colorF;
            FlipHorizontal = false;
            FlipVerical = false;
        }

        public DrawState Combine(Matrix3 innerMatrix)
        {
            return new DrawState(WorldMatrix * innerMatrix, ColorF);
        }

        public DrawState Combine(Matrix3 innerMatrix, ColorF colorF)
        {
            return new DrawState(WorldMatrix * innerMatrix, ColorF * colorF);
        }

        public static DrawState Identity = new DrawState();
    }
}
