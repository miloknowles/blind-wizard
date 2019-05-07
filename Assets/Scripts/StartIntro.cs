using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Attached to the main menu panel, handles the transition to the narrative intro scene.
 */
public class StartIntro : MonoBehaviour
{
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => this.OnClick());

        // Since this is the main menu and we are starting a new game, reset all of the game state.
        GameStateManager.InitializeNewGame(true);
    }

    private void OnClick()
    {
        SceneManager.LoadScene("GameIntro");
    }
}
