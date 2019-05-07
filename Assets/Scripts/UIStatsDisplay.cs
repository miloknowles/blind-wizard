using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Displays the health and other stats for a player.
 * This script belongs to the UIPanel that shows all of the stats.
 * An actor in the game will own a reference to their stats UIPanel
 * and can call the methods defined here to update the UI.
 */
public class UIStatsDisplay : MonoBehaviour
{
    public GameObject UIActorHealthBarSprite;

    // Note: need to set these in the Unity editor!
    // They were hardcoded before, which doesn't work now that the enemy and player have different
    // health bar sizes.
    public float healthBarInitialHeight;
    public float healthBarInitialWidth;
    public bool determineEnemyMaxHealth = false;

    public void UpdateHealth(int value)
    {
        var rectTransform = UIActorHealthBarSprite.GetComponent<RectTransform>();
        float max_value = determineEnemyMaxHealth ? (float)GameStateManager.UpcomingEnemyStats.DetermineEnemyMaxHealth() : 100.0f;
        rectTransform.sizeDelta = new Vector2(this.healthBarInitialWidth * (float)value / max_value, this.healthBarInitialHeight);
    }
}
