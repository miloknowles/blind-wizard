using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
