using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private GameObject actionsMenu;
    private GameObject enemyUnitsMenu;

    public GameObject playerObject;
    
    // Eventually, we may want to support multiple enemies and spawn them programmatically.
    // For simplicity, I'm going to use a single enemy game object for now.
    public GameObject enemyObject;

    // Units get placed in this queue with some priority to act. Every turn, we pop the highest
    // priority actor from the list and do it's action.
    private List<UnitStats> unitActionQueue_;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // Set up a callback for when the scene is loaded.
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Retrieve all of the UI elements that the player interacts with during battle.
        GameObject HUD = GameObject.Find("HUD");
        this.actionsMenu = HUD.transform.Find("ActionMenu").gameObject;
        this.enemyUnitsMenu = HUD.transform.Find("EnemyMenu").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        unitActionQueue_ = new List<UnitStats>();
        GameObject[] player_units = GameObject.FindGameObjectsWithTag("PlayerUnit");
        GameObject[] enemy_units = GameObject.FindGameObjectsWithTag("EnemyUnit");

        // Collect all "PlayerUnit" tagged game objects and add them to the action queue.
        foreach (GameObject unit in player_units) {
            UnitStats stats = unit.GetComponent<UnitStats>();
            stats.calculateNextActTurn(0);
            unitActionQueue_.Add(stats);
        }

        // Collect all "EnemyUnit" tagged game objects and add them to the action queue.
        foreach (GameObject unit in enemy_units) {
            UnitStats stats = unit.GetComponent<UnitStats>();
            stats.calculateNextActTurn(0);
            unitActionQueue_.Add(stats);
        }

        // Sort the queue such that the unit with lowest next turn to act is at position zero.
        unitActionQueue_.Sort();

        // Disable action menus.
        this.actionsMenu.SetActive(false);
        this.enemyUnitsMenu.SetActive(false);

        // Go the the first turn.
        this.NextTurn();
    }

    public void NextTurn()
    {
        // Pop the next unit from unitActionQueue_.
        UnitStats acting_unit_stats = unitActionQueue_[0];
        unitActionQueue_.Remove(acting_unit_stats);

        if (!acting_unit_stats.isDead()) {
            GameObject acting_unit = acting_unit_stats.gameObject;

            // Calculate the next turn the acting_unit_object will act, and add it back to the queue.
            acting_unit_stats.calculateNextActTurn(acting_unit_stats.nextActTurn);
            unitActionQueue_.Add(acting_unit_stats);
            unitActionQueue_.Sort();

            if (acting_unit.tag == "PlayerUnit") {
                Debug.Log("Player unit acting");
                this.NextTurn();
            } else {
                Debug.Log("Enemy unit acting");
                this.NextTurn();
            }
        
        // If the acting unit is dead, skip its turn.
        } else {
            this.NextTurn();
        }
    }
}
