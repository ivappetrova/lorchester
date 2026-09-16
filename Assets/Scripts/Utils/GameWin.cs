using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class GameWin : MonoBehaviour
    {
        void Update()
        {

            TriggerGameWon();

        }

        private void TriggerGameWon()
        {
            SceneManager.LoadScene(2);
        }
    }
}
