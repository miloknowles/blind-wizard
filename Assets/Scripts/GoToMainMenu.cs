using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/*
 * Attached to the gameover / win scenes, handles going back to the main menu when a game is finished.
 */
public class GoToMainMenu : MonoBehaviour
{
    public TMP_Text infoText;
    private string preposition;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => this.OnClick());
        preposition = " a ";
        if (GameStateManager.UpcomingEnemyStats.Element.ToString() == "Earth" || GameStateManager.UpcomingEnemyStats.Element.ToString() == "Air")
        {
            preposition = " an ";
        }
        infoText.text = "Oh no! You were defeated by" + preposition + GameStateManager.UpcomingEnemyStats.Element.ToString() + " type enemy! Try paying more attention to the provided information next time, to more effectively identify and defeat your enemies.";

    }
    private void OnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
