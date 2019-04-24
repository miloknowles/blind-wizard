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
    public GameObject playerObject;
    public Element thisButtonElement;
    private ActorManager playerManager;
    public Color defaultColor;
    public Color selectedColor;

    // Start is called before the first frame update
    void Start()
    {
        // Set up click event handler.
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => this.OnClick());
        // Get player object in order to monitor its element.
        playerManager = playerObject.GetComponent<ActorManager>();
    }

    private void OnClick()
    {
        // Tell the BattleManager that an element was selected in the UI.
        GameObject hudCanvas = GameObject.Find("HUDCanvas");
        hudCanvas.GetComponent<BattleManager>().UISelectPlayerElement(this.thisButtonElement);
    }

    private void Update()
    {
        ColorBlock allColors = this.gameObject.GetComponent<Button>().colors;
        if(this.thisButtonElement == playerManager.Element)
        {
            allColors.normalColor = selectedColor;
        } else
        {
            allColors.normalColor = defaultColor;
        }
        this.gameObject.GetComponent<Button>().colors = allColors;
    }
}
