using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Primitives;

/*
 * Handles attacks between different actors and determining the order in
 * which they should attack.
 *
 * Both the Player and Enemy game objects have an ActorManager attached.
 * Eventually, this manager could also handle animations, and other updates
 * that need to access the owning game object.
 */
public class ActorManager : MonoBehaviour {
    public GameObject UIActorStatsPanel;   // Should be set from Unity editor.
    public GameObject narratorText; // Should be set from Unity editor.

    // The setter for health automatically updates the UI component that's
    // responsible for displaying it.
    private int health_;
    public int Health {
        get { return health_; }
        set {
            health_ = value;
            UIActorStatsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(health_);
        }
    }

    // Both the player and enemy have health and an element.
    // The player can switch its Element with each attack.
    public Element Element { get; set; }
    public Primitives.Attribute Attribute { get; set; }

    // Only initialize the rng once, then make calls to rng.NextDouble()
    private System.Random rng = new System.Random();

    public void Start()
    {
        // Since health will be loaded in from GameStateManager, we want to update here.
        UIActorStatsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(Health);
    }

    /*
     * Perform a specified attack on another ActorManager.
     * This function will simulate whether the attack hits,
     * and apply the damage to the target if so.
     */
    public bool DoAttack(ActorManager target, Attack attack)
    {
        double random_val = rng.NextDouble();

        // Although the player doesn't know the enemy's element type, the attack simulator
        // here needs to know it.
        int matchup_multiplier = ElementOrdering.Compare(this.Element, target.Element);
        double accuracy = attack.accuracy + matchup_multiplier * Constants.SUPER_EFFECTIVE_ACCURACY_BONUS;

        // Simulate whether the attack should hit.
        if (random_val <= accuracy) {
            narratorText.GetComponent<Text>().text = this.name + " attack hit!";
            target.ReceiveDamage(attack.damage);
            return true;
        }

        narratorText.GetComponent<Text>().text = this.name + " attack missed!";

        return false;
    }

    public void ReceiveDamage(int amount)
    {
        this.Health -= amount;
        this.Health = Mathf.Max(this.Health, 0); // Limit health from going below zero.
        UIActorStatsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(Health);

        if (this.IsDead()) { Destroy(this); }
    }

    /*
     * The player is currently dead if health is <= 0.
     */
    public bool IsDead() { return this.Health <= 0; }
};
