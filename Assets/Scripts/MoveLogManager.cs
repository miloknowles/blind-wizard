using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Primitives;
using TMPro;

/*
 * UI Panel that displays the player and enemy actions and their results.
 */
public class MoveLogManager : MonoBehaviour
{
    private List<GameObject> entries = new List<GameObject>();
    public GameObject moveLogEntryPrefab; 

    public void AppendPlayerLogEntry(PlayerActionResult result)
    {
        GameObject entry = Instantiate(moveLogEntryPrefab);
        string entryText =
            "Your " + result.element + " " + result.attack.name +
            " " + (result.successful ? "hit!" : "failed");
        entry.GetComponent<TextMeshProUGUI>().text = entryText;
        entry.transform.SetParent(this.transform, false);   // Really important to use false here!!!!
        entries.Add(entry);
    }

    public void AppendEnemyLogEntry(EnemyActionResult result)
    {
        GameObject entry = Instantiate(moveLogEntryPrefab);
        string entryText = "Enemy attack " + (result.successful ? "hit!" : "failed");
        entry.GetComponent<TextMeshProUGUI>().text = entryText;
        entry.transform.SetParent(this.transform, false);   // Really important to use false here!!!!
        entries.Add(entry);
    }
}
