using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedDoor : Door
{

   override public void UnlockDoor()
    {
        base.UnlockDoor();
        SceneManager.LoadScene(1);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
