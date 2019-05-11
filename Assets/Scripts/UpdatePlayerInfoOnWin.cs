using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerInfoOnWin : MonoBehaviour
{
    public GameObject UIActorStatsPanel;   // Should be set from Unity editor.
    // Start is called before the first frame update
    void Start()
    {
        UIActorStatsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(GameStateManager.PlayerStats.Health);
    }


}
