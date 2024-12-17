using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class KeyScheme
    {
        public Keys up {get; set;}
        public Keys down {get; set;}
        public Keys right {get; set;}
        public Keys left {get; set;}

        public Keys jump {get; set;}
        public Keys attack {get; set;}

        public KeyScheme(Keys up, Keys down, Keys right, Keys left, Keys jump, Keys attack)
        {
            this.up = up;
            this.down = down;
            this.right = right;
            this.left = left;
            this.jump = jump;
            this.attack = attack;
        }
    }
}