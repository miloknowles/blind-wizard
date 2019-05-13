using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Primitives;

public class WizardScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;

    private Vector2 direction;

    public GameObject UIActorStatsPanel;   // Should be set from Unity editor.

    void Start()
    {
        // If the game hasn't already been initialized, this will do it.
        // This is here for convenience, so that we don't have to start in the main menu scene
        // for every playtest to have things set up properly.
        GameStateManager.InitializeNewGame();

        // If the position of the wizard was saved, load it in here.
        if (GameStateManager.MapState.BattlesCompleted >= 1) {
            this.gameObject.transform.position = GameStateManager.MapState.WizardPosition;
        }
        
        UIActorStatsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(GameStateManager.PlayerStats.Health);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
    }

    public void Move()
    {
        transform.Translate(direction * this.speed * Time.deltaTime);
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            direction += Vector2.up;
        }
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            direction += Vector2.left;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            direction += Vector2.down;
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            direction += Vector2.right;
		}
    }

    /*
     * The BattleTriggerNode prefab has a Collider2D attached to it,
     * and the tag "BattleTriggerNode". This function handles entering
     * a battle when the player touches a trigger.
     */
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name);
        if (collider.gameObject.tag == "BattleTriggerNode") {

            // Ignore any battle triggers that we've already done.
            string name = collider.gameObject.name;
            if (GameStateManager.MapState.BattleNodes[name].completed) {
                Debug.Log("Battle trigger already done, skipping.");
                return;
            }
            
            // Otherwise go to battle!
            GameStateManager.MapState.BattleNodes[name].completed = true;
            EnterBattle(GameStateManager.MapState.BattleNodes[name].region);
        
        // The glasses trigger the end of the game!!!
        } else if (collider.gameObject.name == "VictoryGlassesSprite") {
            SceneManager.LoadScene("GameWin1");
        }
    }

    void EnterBattle(Region upcoming_region)
    {
        // IMPORTANT: Update the CurrentRegion in the GameStateManager.
        GameStateManager.MapState.CurrentRegion = upcoming_region;

        // Save the location of the wizard in the map.
        GameStateManager.MapState.WizardPosition = this.gameObject.transform.position;

        // Right before going to the battle, we need to set up the upcoming enemy.
        GameStateManager.UpcomingEnemyStats.Generate(upcoming_region);
        
        SceneManager.LoadScene("BattleScene");
    }
}
