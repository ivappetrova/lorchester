using UnityEngine.SceneManagement;

namespace Movables
{
    public class NextLevelDoor : Door
    {
        public override void UnlockDoor()
        {
            base.UnlockDoor();

            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}