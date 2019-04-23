using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectUnit : MonoBehaviour
{
    private GameObject currentUnit;

    private GameObject actionsMenu, enemyUnitsMenu;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene") {
            GameObject HUD = GameObject.Find("HUD");
            this.actionsMenu = HUD.transform.Find("ActionMenu").gameObject;
            this.enemyUnitsMenu = HUD.transform.Find("EnemyMenu").gameObject;
            //this.actionsMenu = GameObject.Find("ActionsMenu");
            //this.enemyUnitsMenu = GameObject.Find("EnemyUnitsMenu");
        }
    }

    public void selectCurrentUnit(GameObject unit)
    {
        this.currentUnit = unit;

        this.actionsMenu.SetActive(true);
    }

    public void selectAttack(int type)
    {
        this.currentUnit.GetComponent<PlayerAction>().SelectAttack(type);

        this.actionsMenu.SetActive(false);
        this.enemyUnitsMenu.SetActive(true);
    }

    public void attackEnemyTarget(GameObject target)
    {
        this.actionsMenu.SetActive(false);
        this.enemyUnitsMenu.SetActive(false);

        this.currentUnit.GetComponent<PlayerAction>().Act(target);
    }
}
