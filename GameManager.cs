namespace GameProject
{
    public class GameManager
    {
        public  bool isPaused {get; private set;} = false;

        public void TogglePause()
        {
            isPaused = !isPaused;
        }
        public void Pause()
        {
            isPaused = true;
        }
        public void Resume()
        {
            isPaused = false;
        }
    }
}