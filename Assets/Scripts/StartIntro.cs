using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartIntro : MonoBehaviour
{
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => this.OnClick());
    }

    private void OnClick()
    {
        SceneManager.LoadScene("GameIntro");
    }
}
