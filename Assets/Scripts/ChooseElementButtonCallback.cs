using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Primitives;

/*
 * This script should be attached to each of the buttons that the
 * player can press to choose an element for their attack.
 */
public class ChooseElementButtonCallback : MonoBehaviour
{
    public Element thisButtonElement;

    // Start is called before the first frame update
    void Start()
    {
        // Set up click event handler.
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => this.OnClick());
    }

    private void OnClick()
    {
        // Tell the BattleManager that an element was selected in the UI.
        GameObject hudCanvas = GameObject.Find("HUDCanvas");
        hudCanvas.GetComponent<BattleManager>().UISelectPlayerElement(this.thisButtonElement);
    }
}
