using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public bool collected = false;
    public int healAmount;
    public GameObject UIActorStatsPanel;

    private void Update()
    {
        if(collected)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collected = true;
        GameStateManager.MapState.hearts[this.name] = true;
        GameStateManager.PlayerStats.Health = Mathf.Min(GameStateManager.PlayerStats.Health + healAmount, GameStateManager.PlayerStats.MaxHealth);
        UIActorStatsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(GameStateManager.PlayerStats.Health);
    }
}
