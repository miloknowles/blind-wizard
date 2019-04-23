using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackEnemyButtonCallback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set up click event handler.
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => this.OnClick());
    }

    private void OnClick()
    {
        // Tell the BattleManager that the player triggered an attack.
        GameObject hudCanvas = GameObject.Find("HUDCanvas");
        hudCanvas.GetComponent<BattleManager>().UIDoPlayerAttack();
    }
}
