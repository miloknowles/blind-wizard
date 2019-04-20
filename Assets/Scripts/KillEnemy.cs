using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillEnemy : MonoBehaviour
{
    public GameObject menuItem;

    private void OnDestroy()
    {
        Destroy(this.menuItem);
        SceneManager.LoadSceneAsync("MapScene");
    }
}
