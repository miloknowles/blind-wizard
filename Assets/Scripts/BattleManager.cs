using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Primitives;

/*
 * Handles the turn-based fighting between a single player and enemy.
 */
public class BattleManager : MonoBehaviour
{
    //====================== STORE UI STATE ==========================
    private Attack currentAttack;
    private GameObject[] selectActionButtons;   // UI buttons to choose Punch, Kick, Tackle
    private GameObject[] selectElementButtons;  // UI buttons to select element for an attack.
    private GameObject[] doActionButtons;       // UI buttons to hit an enemy with an attack.
    
    
    // Eventually, we may want to support multiple enemies and spawn them programmatically.
    // For simplicity, I'm going to use a single enemy game object for now.

    public GameObject playerObject;
    public GameObject enemyObject;
    private ActorManager enemyManager;
    private ActorManager playerManager;

    public GameObject UIRegionText;
    public GameObject UIAttributeText;
    public GameObject UIMoveLogMenu;

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
        // Initialize the PlayerStats if they haven't been initialized already.
        // For now, the only important thing this does is set player health to
        // 100 at the start of the game. Internally, the GameStateManager will
        // make sure that this happens at most once!
        GameStateManager.PlayerStats.Initialize();
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

            m.CalculateNextActTurn(1);
            actorQueue_.Add(m);
        }

        // Sort the queue such that the unit with lowest next turn to act is at position zero.
        actorQueue_.Sort();

        // Retrieve all of the UI buttons so that we can enable/disable them at the right time.
        selectActionButtons = GameObject.FindGameObjectsWithTag("SelectActionButton");
        doActionButtons = GameObject.FindGameObjectsWithTag("DoActionButton");
        selectElementButtons = GameObject.FindGameObjectsWithTag("SelectElementButton");
        foreach(var b in selectActionButtons) { b.GetComponent<Button>().interactable = false; }
        foreach(var b in doActionButtons) { b.GetComponent<Button>().interactable = false; }
        foreach(var b in selectElementButtons) { b.GetComponent<Button>().interactable = false; }

        playerManager = playerObject.GetComponent<ActorManager>();
        enemyManager = enemyObject.GetComponent<ActorManager>();

        // Copy the global persisted player / enemy states into the player and enemy instances.
        // TODO: copy them back at the end of the battle!
        playerManager.Health = GameStateManager.PlayerStats.Health;
        enemyManager.Element = GameStateManager.UpcomingEnemyStats.Element;
        enemyManager.Health = GameStateManager.UpcomingEnemyStats.Health;
        enemyManager.Attribute = GameStateManager.UpcomingEnemyStats.Attribute;

        // Update the region and attribute displays.
        UIAttributeText.GetComponent<Text>().text = "Attribute: " + enemyManager.Attribute;
        UIRegionText.GetComponent<Text>().text = "Region: " + GameStateManager.MapState.CurrentRegion;

        // Go the the first turn.
        this.NextTurn();
    }

    public void NextTurn()
    {
        // Pop the next unit from actorQueue_.
        if (actorQueue_.Count > 0) {
            ActorManager acting_unit_stats = actorQueue_[0];
            actorQueue_.Remove(acting_unit_stats);

            if (!acting_unit_stats.IsDead()) {
                GameObject acting_unit = acting_unit_stats.gameObject;

                // Calculate the next turn the acting_unit_object will act, and add it back to the queue.
                int current_turn = acting_unit_stats.NextActTurn;
                acting_unit_stats.CalculateNextActTurn(current_turn);
                actorQueue_.Add(acting_unit_stats);
                actorQueue_.Sort();

                if (acting_unit.tag == "PlayerUnit") {
                    Debug.Log("======= Player unit acting =======");
                    HandlePlayerTurn(acting_unit);
                } else {
                    Debug.Log("======== Enemy unit acting =======");
                    IEnumerator coroutine = HandleEnemyTurn(acting_unit);
                    StartCoroutine(coroutine);
                }
            
            // If the acting unit is dead, skip its turn.
            } else {
                Debug.Log("Actor was dead, skipping.");
                this.NextTurn();
            }
        }
    }

    private void HandlePlayerTurn(GameObject player_unit)
    {
        // First, enable the select element buttons.
        foreach(var b in selectElementButtons) { b.GetComponent<Button>().interactable = true; }
    }

    /*
     * Waits asynchronously for a few seconds before taking the enemy's turn.
     */
    private IEnumerator HandleEnemyTurn(GameObject enemy_unit)
    {
        yield return new WaitForSeconds(2.0f);

        bool successful = enemyManager.DoAttack(playerManager, new GenericEnemyAttack());

        // It's critical that the playerManager.Element doesn't change between when the enemy
        // does its attack and when this action result is added to the move log.
        EnemyActionResult enemy_result = new EnemyActionResult(successful, playerManager.Element);
        UIMoveLogMenu.GetComponent<MoveLogManager>().AppendEnemyLogEntry(enemy_result);

        this.NextTurn();
    }

    //===================== CALLBACKS TO ALLOW UI TO INTERACT WITH BATTLEMANAGER ==================
    public void UISelectPlayerElement(Element selected)
    {
        // After choosing an element, the player is allowed to select an attack.
        // The choose element buttons will still be enabled in case they want to switch.
        foreach(var b in selectActionButtons) { b.GetComponent<Button>().interactable = true; }

        // The ActorManager for the player is responsible for knowing it's current element,
        // so we need to update that.
        playerManager.Element = selected;
    }

    public void UISelectPlayerAttack(Attack selected)
    {
        currentAttack = selected;

        // Disable all of the attack buttons (until enemy does its attack).
        foreach(var b in selectActionButtons) { b.GetComponent<Button>().interactable = false; }
        foreach(var b in selectElementButtons) { b.GetComponent<Button>().interactable = false; }
        foreach(var b in doActionButtons) { b.GetComponent<Button>().interactable = false; }

        // Clicking any attack button triggers the attack. We then add the attack (and its result)
        // to the UIMoveLogMenu.
        bool successful = playerManager.DoAttack(enemyManager, currentAttack);
        PlayerActionResult result = new PlayerActionResult(playerManager.Element, currentAttack, successful);
        UIMoveLogMenu.GetComponent<MoveLogManager>().AppendPlayerLogEntry(result);

        this.NextTurn();
    }
}
