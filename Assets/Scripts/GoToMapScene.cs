using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToMapScene : MonoBehaviour
{
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => this.OnClick());

        // If the game hasn't already been initialized, this will do it.
        // This is here for convenience, so that we don't have to start in the main menu scene
        // for every playtest to have things set up properly.
        GameStateManager.InitializeNewGame();
    }

    private void OnClick()
    {
        SceneManager.LoadScene("MapScene");
    }
}
