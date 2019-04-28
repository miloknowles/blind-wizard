using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Handles the transition from the MapScene to the BattleScene.
 */
public class BeginBattle : MonoBehaviour
{
    private Button b;

    // Start is called before the first frame update
    void Start()
    {
        b = GetComponent<Button>();
        b.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        // Right before going to the battle, we need to set up the upcoming enemy.
        GameStateManager.UpcomingEnemyStats.Generate();

        // If elements-for-region samples haven't been generated, do that before battle scene.
        if (!GameStateManager.PlayerStats.samplesInitialized) {
            foreach (Primitives.Region reg in System.Enum.GetValues(typeof(Primitives.Region))) {
                Debug.Log(reg);
                GameStateManager.PlayerStats.AddSamples(reg, 15);
                Debug.Log(GameStateManager.PlayerStats.samples[reg][0] + " " + GameStateManager.PlayerStats.samples[reg][1] + " " + GameStateManager.PlayerStats.samples[reg][2] + " " + GameStateManager.PlayerStats.samples[reg][3]);
            }
        }

        SceneManager.LoadScene("BattleScene");
    }
}
