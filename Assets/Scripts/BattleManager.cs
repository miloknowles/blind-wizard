using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Primitives;

/*
 * Handles the turn-based fighting between a single player and enemy.
 */
public class BattleManager : MonoBehaviour
{
    //====================== BATTLE STATES ===========================
    public enum State {
        IN_PROGRESS,
        PLAYER_WON,
        ENEMY_WON
    };

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

    //================ UNITY EDITOR UI ELEMENTS =======================
    public GameObject UIRegionText;
    public GameObject UIEnemyAttributeText;
    public GameObject UIEnemyMaxHealthText;
    public GameObject UIRegionSamplesTitle;
    public GameObject UIRegionSamplesText;
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
        // I don't think we'll ever have more than one PlayerUnit, but you never know...
        foreach (GameObject unit in player_units)
        {
            ActorManager m = unit.GetComponent<ActorManager>();

            // Set the player unit stats from the global game state!
            m.Health = GameStateManager.PlayerStats.Health;
            actorQueue_.Add(m);
        }

        // Collect all "EnemyUnit" tagged game objects and add them to the action queue.
        // This allows us to have multiple enemies later on.
        foreach (GameObject unit in enemy_units) {
            ActorManager m = unit.GetComponent<ActorManager>();
            actorQueue_.Add(m);
        }

        // Retrieve all of the UI buttons so that we can enable/disable them at the right time.
        selectActionButtons = GameObject.FindGameObjectsWithTag("SelectActionButton");
        doActionButtons = GameObject.FindGameObjectsWithTag("DoActionButton");
        selectElementButtons = GameObject.FindGameObjectsWithTag("SelectElementButton");
        foreach(var b in selectActionButtons) { b.GetComponent<Button>().interactable = false; }
        foreach(var b in doActionButtons) { b.GetComponent<Button>().interactable = false; }
        foreach(var b in selectElementButtons) { b.GetComponent<Button>().interactable = true; }

        // IMPORTANT: If we ever switch to multiple players or enemies this will need to be handled differently!!!
        // We would probably have to store a list of references to player / enemy ActorManager scripts
        playerManager = playerObject.GetComponent<ActorManager>();
        enemyManager = enemyObject.GetComponent<ActorManager>();

        // Copy the global persisted player / enemy states into the player and enemy instances.
        playerManager.Health = GameStateManager.PlayerStats.Health;
        playerManager.Element = Element.Water; // This is the default in the UI!
        enemyManager.Element = GameStateManager.UpcomingEnemyStats.Element;
        enemyManager.Health = GameStateManager.UpcomingEnemyStats.Health;
        enemyManager.Attribute = GameStateManager.UpcomingEnemyStats.Attribute;

        // Update the region and attribute displays.
        Region currentRegion = GameStateManager.MapState.CurrentRegion;
        UIEnemyAttributeText.GetComponent<TextMeshProUGUI>().text = enemyManager.Attribute.ToString() + " enemy";
        UIRegionText.GetComponent<TextMeshProUGUI>().text = currentRegion.ToString();
        UIEnemyMaxHealthText.GetComponent<TextMeshProUGUI>().text = enemyManager.Health + "hp";

        // Show the available samples for this region (retrieve them from GameStateManager).
        int num_water_enemies = GameStateManager.PlayerStats.Samples[currentRegion][Element.Water];
        int num_fire_enemies = GameStateManager.PlayerStats.Samples[currentRegion][Element.Fire];
        int num_air_enemies = GameStateManager.PlayerStats.Samples[currentRegion][Element.Air];
        int num_earth_enemies = GameStateManager.PlayerStats.Samples[currentRegion][Element.Earth];

        Debug.Log(num_water_enemies);
        Debug.Log(num_fire_enemies);
        Debug.Log(num_air_enemies);
        Debug.Log(num_earth_enemies);

        int total_samples = num_water_enemies + num_fire_enemies + num_air_enemies + num_earth_enemies;

        UIRegionSamplesTitle.GetComponent<TextMeshProUGUI>().text = "You have observed " + total_samples + " enemies:";

        UIRegionSamplesText.GetComponent<TextMeshProUGUI>().text =
            num_water_enemies.ToString() + " water type creatures\n" +
            num_fire_enemies.ToString() + " fire type creatures\n" +
            num_air_enemies.ToString() + " air type creatures\n" +
            num_earth_enemies.ToString() + " earth type creatures\n";

        // Go the the first turn of the battle.
        this.NextTurn();
    }

    public void NextTurn()
    {
        // Pop the next unit from actorQueue_. It could be either a player or enemy unit.
        if (actorQueue_.Count > 0) {
            ActorManager acting_unit_stats = actorQueue_[0];
            actorQueue_.Remove(acting_unit_stats);

            if (!acting_unit_stats.IsDead()) {
                GameObject acting_unit = acting_unit_stats.gameObject;

                // Add the current actor back to the end of the queue. If we keep doing this,
                // the order of the actors will repeat over and over again.
                actorQueue_.Add(acting_unit_stats);

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

        // Check if the game state is terminal.
        State current_game_state = CheckBattleState();
        if (current_game_state != State.IN_PROGRESS) {
            HandleBattleOver(current_game_state);
        }
    }
    
    /*
     * Check whether the player or enemy has won. This should be called after every action is
     * taken. If there are no player or enemy units left, then this function indicates that
     * the enemy or player has won.
     */
    private State CheckBattleState()
    {
        int player_units_remaining = 0;
        int enemy_units_remaining = 0;

        foreach(var actor_manager in actorQueue_) {
            if (!actor_manager.IsDead()) {
                if (actor_manager.gameObject.tag == "PlayerUnit") {
                    ++player_units_remaining;
                } else {
                    ++enemy_units_remaining;
                }
            }
        }
        return (player_units_remaining == 0) ? State.ENEMY_WON :
               (enemy_units_remaining == 0) ? State.PLAYER_WON : State.IN_PROGRESS;
    }

    /*
     * Called when the battle is over, either because the player won or enemy won.
     * Saves any important information back to the GameStateManager, and returns
     * to the map scene.
     */
    private void HandleBattleOver(State terminal_state)
    {
        // Update the global GameStateManager with the player's health at the end of the battle.
        GameStateManager.PlayerStats.Health = playerManager.Health;

        if (terminal_state == State.PLAYER_WON) {
            Debug.Log("PLAYER WON");
            SceneManager.LoadScene("BattleVictory");
        } else {
            Debug.Log("ENEMY WON");
            SceneManager.LoadScene("GameOver");
        }    
    }

    private void HandlePlayerTurn(GameObject player_unit)
    {
        // First, enable the select element buttons and the do action buttons.
        foreach(var b in selectElementButtons) { b.GetComponent<Button>().interactable = true; }
        foreach(var b in selectActionButtons) { b.GetComponent<Button>().interactable = true; }
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
