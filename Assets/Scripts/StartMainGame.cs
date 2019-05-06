using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMainGame : MonoBehaviour
{

    public void LoadGame()
    {
        SceneManager.LoadScene("MapScene");
    }

}
