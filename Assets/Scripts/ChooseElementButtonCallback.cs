using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Primitives;
using TMPro;

/*
 * This script should be attached to each of the buttons that the
 * player can press to choose an element for their attack.
 */
public class ChooseElementButtonCallback : MonoBehaviour
{
    // Set these from the Unity editor.
    public GameObject UIHUDCanvas;
    public GameObject UIAccuracyModifierDescriptionText;
    public GameObject UICurrentElementText;
    public Element thisButtonElement;

    // Start is called before the first frame update
    void Start()
    {
        // Set up click event handler.
        GetComponent<Button>().onClick.AddListener(() => this.OnClick());
    }

    private void OnClick()
    {
        // Tell the BattleManager that an element was selected in the UI.
        UIHUDCanvas.GetComponent<BattleManager>().UISelectPlayerElement(this.thisButtonElement);

        // Indicate that this button is selected.
        UICurrentElementText.GetComponent<TextMeshProUGUI>().text = this.thisButtonElement.ToString();

        Element effectiveElement = ElementOrdering.GetEffective(thisButtonElement);
        Element ineffectiveElement = ElementOrdering.GetIneffective(thisButtonElement);

        UIAccuracyModifierDescriptionText.GetComponent<TextMeshProUGUI>().text =
            "+30% accuracy against " + effectiveElement +
            "\n" + "-30% accuracy against " + ineffectiveElement;

        this.transform.parent.gameObject.SetActive(false);
    }
}
