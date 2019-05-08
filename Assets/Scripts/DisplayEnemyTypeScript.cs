using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayEnemyTypeScript : MonoBehaviour
{
    public TMP_Text infoText;
    public GameObject revealObjects;
    public GameObject upgradeObjects;
    private string preposition;

    // Start is called before the first frame update
    void Start()
    {
        preposition = " a ";
        upgradeObjects.SetActive(false);
        revealObjects.SetActive(true);
        if (GameStateManager.UpcomingEnemyStats.Element.ToString() == "Earth" || GameStateManager.UpcomingEnemyStats.Element.ToString() == "Air")
        {
            preposition = " an ";
        }
        infoText.text = "Congratulations, you defeated "+ preposition + GameStateManager.UpcomingEnemyStats.Element.ToString() + " type enemy!";
    }

    public void SwitchToUpgrade()
    {
        revealObjects.SetActive(false);
        upgradeObjects.SetActive(true);
    }

}
