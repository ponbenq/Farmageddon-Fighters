
using Microsoft.Xna.Framework;

namespace ThanaNita.MonoGameTnt
{
    public class Panel : Actor
    {
        RectangleActor background;
        HollowRectActor frame;

        RectF rawRect;
        public override RectF RawRect => rawRect;
        public Panel(Vector2 size, Color backgroundColor, 
                     Color outlineColor, float outlineWidth = 2)
        {
            rawRect = new RectF(Vector2.Zero, size);
            background = new RectangleActor(backgroundColor, rawRect);
            frame = new HollowRectActor(outlineColor, outlineWidth, 
                                        rawRect.CreateExpand(-outlineWidth/2));
        }

        public Panel(Vector2 parentSize, Vector2 offset, Color backgroundColor,
                     Color outlineColor, float outlineWidth = 2)
            : this(parentSize - 2*offset, backgroundColor, outlineColor, outlineWidth)
        {
            Position = offset;
        }

        protected override void DrawSelf(DrawTarget target, DrawState state)
        {
            base.DrawSelf(target, state);
            var combine = CombineState(state);
            background.Draw(target, combine);
            frame.Draw(target, combine);
        }
    }
}
