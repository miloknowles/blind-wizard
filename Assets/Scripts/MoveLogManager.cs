using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Primitives;

public class MoveLogManager : MonoBehaviour
{
    private List<GameObject> entries = new List<GameObject>();
    public GameObject moveLogEntryPrefab; 

    public void AppendPlayerLogEntry(PlayerActionResult result)
    {
        GameObject entry = Instantiate(moveLogEntryPrefab);
        entry.transform.SetParent(this.transform);
        string entryText =
            "Your " + result.element + " " + result.attack.name +
            " " + (result.successful ? "hit!" : "failed");
        entry.GetComponent<Text>().text = entryText;
        entries.Add(entry);
    }

    public void AppendEnemyLogEntry(EnemyActionResult result)
    {
        GameObject entry = Instantiate(moveLogEntryPrefab);
        entry.transform.SetParent(this.transform);
        string entryText = "Enemy attack " + (result.successful ? "hit!" : "failed");
        entry.GetComponent<Text>().text = entryText;
        entries.Add(entry);
    }
}
