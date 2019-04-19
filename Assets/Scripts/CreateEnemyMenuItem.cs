using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateEnemyMenuItem : MonoBehaviour
{
    [SerializeField]
    private GameObject targetEnemyUnitPrefab;

    [SerializeField]
    private Sprite menuItemSprite;

    [SerializeField]
    private KillEnemy killEnemyScript;

    private void Awake()
    {
        //GameObject enemyUnitsMenu = GameObject.Find("EnemyMenu");
        GameObject HUD = GameObject.Find("HUD");
        GameObject enemyUnitsMenu = HUD.transform.Find("EnemyMenu").gameObject;
        GameObject targetEnemyUnit = Instantiate(this.targetEnemyUnitPrefab, enemyUnitsMenu.transform) as GameObject;
        targetEnemyUnit.name = "Target " + this.gameObject.name;
        targetEnemyUnit.GetComponentInChildren < Text >().text = gameObject.name;
        targetEnemyUnit.GetComponent<Button>().onClick.AddListener(() => selectEnemyTarget());

        killEnemyScript.menuItem = targetEnemyUnit;
    }

    public void selectEnemyTarget()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<SelectUnit>().attackEnemyTarget(this.gameObject);
    }
}
