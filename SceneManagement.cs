using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadingSceneController.LoadScene("3DQuarterView");
        }
    }
}
