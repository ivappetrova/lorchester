using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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