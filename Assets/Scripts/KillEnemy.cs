using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * When an enemy is destroyed, transition back to the MapScene.
 */
public class KillEnemy : MonoBehaviour
{
    public GameObject menuItem;

    private void OnDestroy()
    {
        Destroy(this.menuItem);
        SceneManager.LoadSceneAsync("MapScene");
    }
}
