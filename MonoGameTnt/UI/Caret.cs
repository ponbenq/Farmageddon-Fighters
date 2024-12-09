
using Microsoft.Xna.Framework;

namespace ThanaNita.MonoGameTnt
{
    public class Caret : RectangleActor
    {
        float time = 0;
        bool show = true;
        Color color;
        public float HalfDuration { get; set; } = 0.5f;
        public Caret(Color color, Vector2 size) : base(color, size)
        {
            this.color = color;
        }

        public override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            time += deltaTime;
            if (time < HalfDuration)
                return;

            time -= HalfDuration;
            show = !show;
            if (show)
                Color = color;
            else
                Color = Color.Transparent;
        }
    }
}
