using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Primitives;

/*
 * Handles the turn-based fighting between a single player and enemy.
 */
public class BattleManager : MonoBehaviour
{
    //====================== STORE UI STATE ===========================
    private GameObject actionsMenu;
    private GameObject enemyUnitsMenu;
    private Attack currentAttack;
    
    // Eventually, we may want to support multiple enemies and spawn them programmatically.
    // For simplicity, I'm going to use a single enemy game object for now.
    public GameObject enemyObject;
    public GameObject playerObject;

    // Units get placed in this queue with some priority to act. Every turn, we pop the highest
    // priority actor from the list and do it's action.
    private List<ActorManager> actorQueue_;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // Set up a callback for when the scene is loaded.
    }

    /*
     * Gets called BEFORE any of the script Start() methods.
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneLoaded");

        // Initialize the PlayerStats if they haven't been initialized already.
        // For now, the only important thing this does is set player health to
        // 100 at the start of the game. Internally, the GameStateManager will
        // make sure that this happens at most once!
        GameStateManager.PlayerStats.Initialize();
        
        // Retrieve all of the UI elements that the player interacts with during battle.
        GameObject HUD = GameObject.Find("HUDCanvas");
        this.actionsMenu = HUD.transform.Find("ActionMenu").gameObject;
        this.enemyUnitsMenu = HUD.transform.Find("EnemyMenu").gameObject;
    }

    // Start is called before the first frame update (and after OnSceneLoaded).
    void Start()
    {
        actorQueue_ = new List<ActorManager>();
        GameObject[] player_units = GameObject.FindGameObjectsWithTag("PlayerUnit");
        GameObject[] enemy_units = GameObject.FindGameObjectsWithTag("EnemyUnit");

        Debug.Log("Found " + player_units.Length + " player units and " + enemy_units.Length + " enemy units.");

        // Collect all "PlayerUnit" tagged game objects and add them to the action queue.
        foreach (GameObject unit in player_units) {
            ActorManager m = unit.GetComponent<ActorManager>();

            // Set the player unit stats from the global game state!
            m.Health = GameStateManager.PlayerStats.Health;

            m.CalculateNextActTurn(0);
            actorQueue_.Add(m);
        }

        // Collect all "EnemyUnit" tagged game objects and add them to the action queue.
        foreach (GameObject unit in enemy_units) {
            ActorManager m = unit.GetComponent<ActorManager>();

            // Enemy should always start with 100 health for now.
            m.Health = 100;

            m.CalculateNextActTurn(0);
            actorQueue_.Add(m);
        }

        // Sort the queue such that the unit with lowest next turn to act is at position zero.
        actorQueue_.Sort();

        // Disable action menus.
        this.actionsMenu.SetActive(false);
        this.enemyUnitsMenu.SetActive(false);

        // Go the the first turn.
        this.NextTurn();
    }

    public void NextTurn()
    {
        Debug.Log("Actors remaining: " + actorQueue_.Count);

        // Pop the next unit from actorQueue_.
        if (actorQueue_.Count > 0) {
            ActorManager acting_unit_stats = actorQueue_[0];
            actorQueue_.Remove(acting_unit_stats);

            if (!acting_unit_stats.IsDead()) {
                GameObject acting_unit = acting_unit_stats.gameObject;

                // Calculate the next turn the acting_unit_object will act, and add it back to the queue.
                acting_unit_stats.CalculateNextActTurn(acting_unit_stats.NextActTurn);
                actorQueue_.Add(acting_unit_stats);
                actorQueue_.Sort();

                if (acting_unit.tag == "PlayerUnit") {
                    Debug.Log("Player unit acting");
                    // this.NextTurn();
                } else {
                    Debug.Log("Enemy unit acting");
                    // this.NextTurn();
                }
            
            // If the acting unit is dead, skip its turn.
            } else {
                Debug.Log("Actor was dead, skipping.");
                this.NextTurn();
            }
        }
    }

    public void UISelectPlayerAttack(Attack selected)
    {
        Debug.Log("Selected a Player attack from the UI");
        currentAttack = selected;
    }

    public void UIPlayerAttack()
    {
        Debug.Log("UIPlayerAttack()"); 
    }
}
