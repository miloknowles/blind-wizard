﻿using System;
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
public class ActorManager : MonoBehaviour, IComparable {
    public GameObject statsPanel;   // Should be set from Unity editor.
    public GameObject narratorText; // Should be set from Unity editor.

    private int nextActTurn_ = 0;
    public int NextActTurn {
        get { return nextActTurn_; }
        private set {
            nextActTurn_ = value;
        }
    }

    // Both the player and enemy have health and an element.
    // The player can switch its Element with each attack.
    private int health_;
    public int Health {
        get { return health_; }
        set {
            health_ = value;
            statsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(health_);
        }
    }
    public Element Element { get; set; }
    public Primitives.Attribute Attribute { get; set; }

    public void Start()
    {
        // NextActTurn = 0;

        // Since health will be loaded in from GameStateManager, we want to update here.
        statsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(Health);
    }

    /*
     * Perform a specified attack on another ActorManager.
     * This function will simulate whether the attack hits,
     * and apply the damage to the target if so.
     */
    public bool DoAttack(ActorManager target, Attack attack)
    {
        // Debug.Log(">>> Attacking with " + Element + " + " + attack);

        System.Random r = new System.Random();
        double random_val = r.NextDouble();

        int matchup_multiplier = ElementOrdering.Compare(this.Element, target.Element);
        double accuracy = attack.accuracy + matchup_multiplier * Constants.SUPER_EFFECTIVE_ACCURACY_BONUS;

        // Debug.Log(">>> Target Element: " + target.Element);
        // Debug.Log(">>> Accuracy: " + accuracy);
        // Debug.Log(">>> Damage: " + attack.damage);

        // Simulate whether the attack should hit.
        if (random_val <= accuracy) {
            narratorText.GetComponent<Text>().text = this.name + " attack hit!";
            // Debug.Log(">>> Attack hit!");
            target.ReceiveDamage(attack.damage);
            return true;
        }

        // Debug.Log(">>> Attack missed!");
        narratorText.GetComponent<Text>().text = this.name + " attack missed!";

        return false;
    }

    public void ReceiveDamage(int amount)
    {
        this.Health -= amount;
        statsPanel.GetComponent<UIStatsDisplay>().UpdateHealth(Health);

        if (this.IsDead()) { Destroy(this); }
    }

    // For now, return to the MapScene whenever the enemy or player dies.
    public void OnDestroy()
    {
        SceneManager.LoadScene("MapScene");
    }

    /*
     * The player is currently dead if health is <= 0.
     */
    public bool IsDead() { return this.Health <= 0; }

    /*
     * Used by the BattleManager to decide which actor should go next.
     * Currently, the player and enemy alternate, so the next turn that they should
     * act is incremented by two. If we move to multiple players / enemies this will
     * break!!!
     */
    public void CalculateNextActTurn(int currentTurn)
    {
        this.NextActTurn = (currentTurn + 2);
    }

    // Allows actors to be sorted based on their next act turn.
    public int CompareTo(object other)
    {
        return NextActTurn.CompareTo(((ActorManager)other).NextActTurn);
    }
};
