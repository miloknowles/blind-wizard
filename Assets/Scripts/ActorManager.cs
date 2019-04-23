using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private int nextActTurn;

    // Both the player and enemy have health and an element.
    // The player can switch its Element with each attack.
    public int Health { get; private set; }
    public Element Element { get; private set; }
    public bool Dead { get; private set; }

    /*
     * Perform a specified attack on another ActorManager.
     * This function will simulate whether the attack hits,
     * and apply the damage to the target if so.
     */
    bool DoAttack(ActorManager target, Attack attack)
    {
        System.Random r = new System.Random();
        double random_val = r.NextDouble();

        int matchup_multiplier = ElementOrdering.Compare(this.Element, target.Element);
        double accuracy = attack.accuracy + matchup_multiplier * Constants.SUPER_EFFECTIVE_ACCURACY_BONUS;

        // Simulate whether the attack should hit.
        if (random_val <= accuracy) {
            Debug.Log("Attack hit!");
            target.ReceiveDamage(attack.damage);
            return true;
        }

        Debug.Log("Attack missed!");
        return false;
    }

    public void ReceiveDamage(int amount)
    {
        this.Health -= amount;
    }

    // For now, return to the MapScene whenever the enemy or player dies.
    public void OnDestroy()
    {
        SceneManager.LoadSceneAsync("MapScene");
    }

    /*
     * The player is currently dead if health is <= 0.
     */
    public bool IsDead() { return this.Health <= 0; }

    /*
     * Used by the BattleManager to decide which actor should go next.
     */
    public void CalculateNextActTurn(int currentTurn)
    {
        //this.nextActTurn = currentTurn + (int)Math.Ceiling(100.0f / this.speed);
        this.nextActTurn = currentTurn + 100;
    }

    // Allows actors to be sorted based on their next act turn.
    public int CompareTo(ActorManager other)
    {
        return nextActTurn.CompareTo(other.nextActTurn);
    }
};
