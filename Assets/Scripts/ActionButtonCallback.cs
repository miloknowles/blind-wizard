using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Primitives;
using TMPro;

/*
 * Attach this script to every action-choosing button.
 */
public class ActionButtonCallback : MonoBehaviour
{
    // Unfortunately, we need to create another enum here so that button attack
    // types can be set within Unity. Later on, if we create these buttons
    // in code, we might be able to avoid this issue.
    public enum AttackButtonType { Punch, Kick, Tackle };
    public AttackButtonType thisButtonAttack;
    private Attack attack;
    public TMP_Text damageText;

    void Start()
    {

        // Set up click event handler.
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => this.OnClick());
        
        if (thisButtonAttack == AttackButtonType.Punch) {
            attack = new Punch();
            damageText.text = GameStateManager.GameConstants.PUNCH_DAMAGE + "hp";
        } else if (thisButtonAttack == AttackButtonType.Kick) {
            attack = new Kick();
            damageText.text = GameStateManager.GameConstants.KICK_DAMAGE + "hp";
        } else {
            attack = new Tackle();
            damageText.text = GameStateManager.GameConstants.TACKLE_DAMAGE + "hp";
        }


        // this.gameObject.GetComponent<Button>().interactable = false;
    }

    private void OnClick()
    {
        // Tell the BattleManager that an attack was selected in the UI.
        GameObject hudCanvas = GameObject.Find("HUDCanvas");
        hudCanvas.GetComponent<BattleManager>().UISelectPlayerAttack(this.attack);
    }
}
