using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    public GameObject enemyUnitPrefab;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        // For testing purposes, remove when we have proper enemy spawning
        Instantiate(enemyUnitPrefab);

        this.gameObject.SetActive(true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.gameObject.SetActive(scene.name == "BattleScene");     
    }
}
