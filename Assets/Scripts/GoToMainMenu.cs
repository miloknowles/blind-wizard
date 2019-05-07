using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Attached to the gameover / win scenes, handles going back to the main menu when a game is finished.
 */
public class GoToMainMenu : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => this.OnClick());
    }
    private void OnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
