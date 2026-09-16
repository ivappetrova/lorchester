using UnityEngine.SceneManagement;

namespace Movables
{
    public class FinalDoor : Door
    {
        public override void UnlockDoor()
        {
            base.UnlockDoor();
            SceneManager.LoadScene(2);

        }
    }
}