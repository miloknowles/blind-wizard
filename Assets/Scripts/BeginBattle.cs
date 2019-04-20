using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginBattle : MonoBehaviour
{
    private Button b;

    // Start is called before the first frame update
    void Start()
    {
        b = GetComponent<Button>();
        b.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        SceneManager.LoadSceneAsync("BattleScene");
    }
}
