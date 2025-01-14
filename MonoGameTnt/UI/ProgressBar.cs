
using Microsoft.Xna.Framework;

namespace ThanaNita.MonoGameTnt
{
    public class ProgressBar : Actor
    {
        float max;
        float value;
        Actor outer;
        Actor inner;
        Color outerColor;
        Color innerColor;
        Vector2 size;
        public Vector2 RawSize { get => size; }
        public float Max {  get => max; set { max = value; Update(); } }
        public float Value { get => value; set { this.value = value; Update(); } }
        public Color OuterColor { get => outerColor; set { outerColor = value; Update(); } }
        public Color InnerColor { get => innerColor; set { innerColor = value; Update(); } }
        public ProgressBar(Vector2 size, float max, Color outerColor, Color innerColor)
        {
            this.max = max;
            value = max;
            this.outerColor = outerColor;
            this.innerColor = innerColor;
            this.size = size;
            Update();
        }

        private void Update()
        {
            float v = value;
            if (v > max) v = max;
            if (v < 0) v = 0;

            var innerSize = new Vector2((v / max) * size.X, size.Y);

            outer = new RectangleActor(outerColor, size);
            inner = new RectangleActor(innerColor, innerSize);
        }

        protected override void DrawSelf(DrawTarget target, DrawState state)
        {
            base.DrawSelf(target, state);
            var combine = CombineState(state);
            outer.Draw(target, combine);
            inner.Draw(target, combine);
        }
    }
}
