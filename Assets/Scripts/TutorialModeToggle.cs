using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialModeToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Toggle>().onValueChanged.AddListener((isSelected) => {
            GameStateManager.MapState.TutorialMode = isSelected;
        });
    }
}
