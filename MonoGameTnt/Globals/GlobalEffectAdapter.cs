using Microsoft.Xna.Framework.Graphics;


namespace ThanaNita.MonoGameTnt
{
    public class GlobalEffectAdapter
    {
        private static EffectAdapter storedValue = null;
        public static EffectAdapter Value 
        {
            get => storedValue;
            set
            {
                GlobalUtil.AssertOneTimeSetter(storedValue, value, "EffectAdapter");
                storedValue = value;
            }
        }
    }
}
