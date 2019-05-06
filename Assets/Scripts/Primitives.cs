using System.Collections;
using System.Collections.Generic;

// All of the primitive concepts in our game such as 'Region' and 'Element'
// are placed into their own namespace to avoid colliding with other Unity
// or C# things with the same name.
namespace Primitives {

// /*
//  * Store all of the importants constants here so we only have to change them
//  * in one place. This should also make it easier to adjust game difficulty.
//  */
// static class Constants
// {
//     public const double SUPER_EFFECTIVE_ACCURACY_BONUS = 0.3;
//     public const int GENERIC_ENEMY_ATTACK_DAMAGE = 30;
//     public const double GENERIC_ENEMY_ATTACK_ACCURACY = 0.6;
//     public const int PUNCH_DAMAGE = 20;
//     public const double PUNCH_ACCURACY = 70.0;
//     public const int KICK_DAMAGE = 40;
//     public const double KICK_ACCURACY = 0.5;
//     public const int TACKLE_DAMAGE = 60;
//     public const double TACKLE_ACCURACY = 0.3;
// };

/*
 * Defines all of the regions accessible in the game.
 */
public enum Region {
    Ocean,
    Volcano,
    Underground,
    Cloud,
    Plains,
    MMTTG
};

/*
 * Defines the four element types that the player and enemy can take on.
 * We explicitly assign integer values to each of the enum types so that
 * they have a "super effective" ordering Air > Earth > Water > Fire > Air.
 *
 * Note: Reordering these will break ElementOrder.Compare()!
 */
public enum Element {
    Fire,
    Water,
    Earth,
    Air
};

/*
 * Defines the three visual attributes that types can have.
 */
public enum Attribute {
    Scaly,
    Furry,
    Smooth
};

/*
 * Parent class for all attack types. Each attack type should derive from this
 * and set its own accuracy and damage.
 */
public class Attack {
    public Attack(int d, double a, string name) {
        damage = d;
        accuracy = a;
        this.name = name;
    }
    public int damage;         // Value between 0 and 100 (%).
    public double accuracy;    // Percentage between 0 and 1.
    public string name = "Attack";
};

public class GenericEnemyAttack : Attack {
    public GenericEnemyAttack() :
        base(GameStateManager.GameConstants.GENERIC_ENEMY_ATTACK_DAMAGE,
             GameStateManager.GameConstants.GENERIC_ENEMY_ATTACK_ACCURACY,
             "GenericEnemyAttack") {}
};

public class Punch : Attack {
    public Punch() : base(GameStateManager.GameConstants.PUNCH_DAMAGE,
                          GameStateManager.GameConstants.PUNCH_ACCURACY, "Punch") {}
};

public class Kick : Attack {
    public Kick() : base(GameStateManager.GameConstants.KICK_DAMAGE,
                         GameStateManager.GameConstants.KICK_ACCURACY, "Kick") {}
};

public class Tackle : Attack {
    public Tackle() : base(GameStateManager.GameConstants.TACKLE_DAMAGE,
                           GameStateManager.GameConstants.TACKLE_ACCURACY, "Tackle") {}
};

/*
 * Stores the action that a player took (an element type and attack),
 * and the result of the action.
 */
public struct PlayerActionResult {
    public PlayerActionResult(Element element, Attack attack, bool successful) {
        this.element = element;
        this.attack = attack;
        this.successful = successful;
    }
    public Element element;    // Element that the player chose.
    public Attack attack;      // The attack chosen by the player.
    public bool successful;    // Did the attack hit?
};

/*
 * Enemy does not have differentiated attacks (at least for now), and
 * simply has a base accuracy. We also keep track of the player's element
 * when the enemy attacked them, because this affects the enemy's
 * accuracy.
 */
public struct EnemyActionResult {
    public EnemyActionResult(bool successful, Element player_element_during) {
        this.successful = successful;
        this.player_element_during = player_element_during;
        this.accuracy = GameStateManager.GameConstants.GENERIC_ENEMY_ATTACK_ACCURACY;
    }

    public Element player_element_during;  // The PLAYER's element when the enemy attacked them.
    public double accuracy;                // Accuracy BEFORE any effectiveness changes are made.
    public bool successful;                // Did enemy attack hit you?
};

/*
 * Provides a helper method for comparing two elements.
 */
public static class ElementOrdering {
    /*
     * Returns +1 if el1 is effective against el2
     * Returns 0 if el1 is neutral against el2
     * Returns -1 if el1 is ineffective against el2
     */
    public static int Compare(Element el1, Element el2)
    {
        int enum_val1 = (int)el1;
        int enum_val2 = (int)el2;

        // In case the number of elements changes later, calculate it here.
        int num_elements = System.Enum.GetNames(typeof(Element)).Length;
        
        // Case 1: if el2 is AFTER el1, then el1 is ineffective.
        if (enum_val2 == (enum_val1 + 1) % num_elements) {
            return -1;
        
        // Case 2: if el1 is AFTER el2, then el1 is effective.
        } else if (enum_val1 == (enum_val2 + 1) % num_elements) {
            return 1;

        // Case 3: everything else is neutral.
        } else {
            return 0;
        }
    }

    public static Element GetEffective(Element el)
    {
        int enum_val = (int)el;
        int num_elements = System.Enum.GetNames(typeof(Element)).Length;
        int effective_idx = (enum_val == 0) ? (num_elements - 1) : (enum_val - 1);
        return (Element)effective_idx;
    }

    public static Element GetIneffective(Element el)
    {
        int enum_val = (int)el;
        int num_elements = System.Enum.GetNames(typeof(Element)).Length;
        return (Element)((enum_val + 1) % num_elements);
    }
};


} // namespace Primitives