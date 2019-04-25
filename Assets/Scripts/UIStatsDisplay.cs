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
    public void UpdateHealth(int value)
    {
        GameObject health_value_text_object = transform.Find("HealthValue").gameObject;
        health_value_text_object.GetComponent<Text>().text = value.ToString();

        GameObject playerHealthBar;

        if (transform.name == "PlayerStatsPanel") {
            playerHealthBar = GameObject.Find("PlayerBarSprite").gameObject;
        } else {
            playerHealthBar = GameObject.Find("EnemyBarSprite").gameObject;
        }

        var rectTransform = playerHealthBar.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(150f * value / 100f, 20f);
    }
}
