
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ThanaNita.MonoGameTnt
{
    public class InputBox : Actor
    {
        Panel panel;
        Text text;
        Caret caret;
        public InputBox(string fontName, float fontSize, Color color
                        , string str, float width
                        , Vector2 padding1 = new Vector2(), Vector2 padding2 = new Vector2())
        {
            text = new Text(fontName, fontSize, color, str);
            text.Position = padding1;

            var size = new Vector2(width + padding1.X + padding2.X,
                                    text.RawSize.Y + padding1.Y + padding2.Y);
            panel = new Panel(size, Color.White, Color.Black, 1);

            caret = new Caret(Color.Pink, new Vector2(2, size.Y - 2));
            
            caret.Position = new Vector2(text.MeasureString("Hello").X + padding1.X, 1);

            Add(panel);
            Add(text);
            Add(caret);
        }
    }
}
