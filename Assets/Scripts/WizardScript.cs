using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;

    private Vector2 direction;

    void Start()
    {
        // If the position of the wizard was saved, load it in here.
        if (GameStateManager.MapState.BattlesCompleted >= 1) {
            this.gameObject.transform.position = GameStateManager.MapState.WizardPosition;
        }
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
            if (GameStateManager.MapState.CompletedBattleTriggerNodeNames.Contains(collider.gameObject.name)) {
                Debug.Log("Battle trigger already done, skipping.");
                return;
            }
            
            // Otherwise go to battle!
            GameStateManager.MapState.CompletedBattleTriggerNodeNames.Add(collider.gameObject.name);
            EnterBattle();
        }
    }

    void EnterBattle()
    {
        // Save the location of the wizard in the map.
        GameStateManager.MapState.WizardPosition = this.gameObject.transform.position;

        // Right before going to the battle, we need to set up the upcoming enemy.
        GameStateManager.UpcomingEnemyStats.Generate();

        // If elements-for-region samples haven't been generated, do that before battle scene.
        if (!GameStateManager.PlayerStats.samplesInitialized) {
            foreach (Primitives.Region reg in System.Enum.GetValues(typeof(Primitives.Region))) {
                Debug.Log(reg);
                GameStateManager.PlayerStats.AddSamplesForRegion(reg, 15);
            }
        }

        SceneManager.LoadScene("BattleScene");
    }
}
