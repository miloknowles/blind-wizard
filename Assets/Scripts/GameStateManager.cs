using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primitives;

using ElementSamples = System.Collections.Generic.Dictionary<Primitives.Element, int>;

public class GameStateManager : MonoBehaviour
{
    public static bool game_is_initialized_ = false;

    /*
     * This resets all states in the game to their initial values.
     */
    public static void InitializeNewGame(bool force_reset = false)
    {
        if (!game_is_initialized_ || force_reset) {
            MapState.Reset();           // Need to unmark any finished battle nodes on the map.
            GameConstants.Reset();      // Game constants can be upgraded, so we need to reset.
            PlayerStats.Reset();        // Reset samples to 15 for each region, health to 100.
            game_is_initialized_ = true;
        }
    }

    /*
     * Stores the location of the wizard in the map scene so that there's continuity
     * after battles. Also stores the battle node names that bave been completed and
     * the number of battles that have been done.
     */
    public static class MapState
    {
        public static Region CurrentRegion { get; set; }

        // The world coordinates of the wizard in the MapScene. This is saved right before battle
        // and restored after leaving the battle.
        public static Vector3 WizardPosition = new Vector3(0, 0, 0);
        public static int BattlesCompleted = 0;

        // Each BattleTriggerNode has some corresponding data that we store about whether it
        // has been completed and what its Region is. This is persisted here so that its consistent
        // every time the Map Scene is reloaded.
        public static Dictionary<string, BattleNodeInfo> BattleNodes = new Dictionary<string, BattleNodeInfo>();

        public static void Reset()
        {
            WizardPosition = new Vector3(0, 0, 0);
            BattlesCompleted = 0;

            // WARNING: Resetting the battle nodes below could erase all of the work that UpdateZoneSprites does
            // so I don't think it's a good idea. Currently, the BattleNodes will stay the same for multiple
            // playthroughs, until the game is restarted. This is fine, especially if the player wants to
            // play through again with a different path in mind.
            // BattleNodes = new Dictionary<string, BattleNodeInfo>();
        }
    };

    /*
     * Store all of the importants constants here so we only have to change them
     * in one place. This is where difficulty can be changed and/or accuracies and
     * damages can be upgraded.
     */
    public static class GameConstants
    {
        public static double SUPER_EFFECTIVE_ACCURACY_BONUS = 0.3;
        public static int GENERIC_ENEMY_ATTACK_DAMAGE = 15;
        public static double GENERIC_ENEMY_ATTACK_ACCURACY = 0.6;
        public static int PUNCH_DAMAGE = 15;
        public static double PUNCH_ACCURACY = 0.70;
        public static int KICK_DAMAGE = 30;
        public static double KICK_ACCURACY = 0.5;
        public static int TACKLE_DAMAGE = 45;
        public static double TACKLE_ACCURACY = 0.3;

        /*
         * Add hp_upgrade_amount of damage to the attack_to_upgrade.
         */
        public static void UpgradeDamage(Attack attack_to_upgrade, int hp_upgrade_amount)
        {
            if (attack_to_upgrade.GetType() == typeof(Punch)) {
                PUNCH_DAMAGE += hp_upgrade_amount;
            } else if (attack_to_upgrade.GetType() == typeof(Kick)) {
                KICK_DAMAGE += hp_upgrade_amount;
            } else if (attack_to_upgrade.GetType() == typeof(Tackle)) {
                TACKLE_DAMAGE += hp_upgrade_amount;
            }
        }

        public static void Reset()
        {
            SUPER_EFFECTIVE_ACCURACY_BONUS = 0.3;
            GENERIC_ENEMY_ATTACK_DAMAGE = 15;
            GENERIC_ENEMY_ATTACK_ACCURACY = 0.6;
            PUNCH_DAMAGE = 15;
            PUNCH_ACCURACY = 0.7;
            KICK_DAMAGE = 30;
            KICK_ACCURACY = 0.5;
            TACKLE_DAMAGE = 45;
            TACKLE_ACCURACY = 0.3;
        }
    };

    /*
     * The player state is stored in this static class. This is a way to persist
     * the player state globally across multiple scenes.
     *
     * Note: player stats should be retrieved from here when entering the battle scene.
     */
    public static class PlayerStats {
        public static int Health { get; set; }

        // For each region, store a dictionary that maps each element to an int.
        public static Dictionary<Region, ElementSamples> Samples = new Dictionary<Region, ElementSamples>();

        /*
         * Initializes a set of N element samples for a given region.
         */
        public static void AddSamplesForRegion(Region region, int N)
        {
            for (int i = 0; i < N; i++) {
                Element el = ProbabilitySystem.SampleElementGivenRegion(region);
                PlayerStats.Samples[region][el] += 1;
            }
        }

        public static void Reset()
        {
            Health = 100;
            
            // Set samples for each element in each region to zero.
            foreach (Region r in System.Enum.GetValues(typeof(Region))) {
                PlayerStats.Samples[r] = new ElementSamples();
                foreach (Primitives.Element el in System.Enum.GetValues(typeof(Primitives.Element))) {
                    Samples[r][el] = 0;
                }
            }

            // Give the player 15 samples for each region.
            foreach (Region r in System.Enum.GetValues(typeof(Region))) {
                AddSamplesForRegion(r, 15);
            }
        }
    };

    /*
     * The enemy state is stored in this static class.
     *
     * Note: when entering the battle scene, the enemy stats should already be set
     * here; the code in the battle scene expects to read what type of enemy to 
     * render and face the player against from here.
     */
    public static class UpcomingEnemyStats {
        public static int Health { get; private set; }
        public static Attribute Attribute { get; private set; }
        public static Element Element { get; private set; }

        /*
         * Generates an upcoming enemy based on the distribution over elements
         * in a region. Also generates an attribute for the enemy based on the
         * distribution of attributes given an element.
         */
        public static void Generate(Region upcoming_region)
        {
            MapState.CurrentRegion = upcoming_region; // Just to be safe.

            // The enemy health increases with each battle to raise difficulty.
            Health = DetermineEnemyMaxHealth();

            Element = ProbabilitySystem.SampleElementGivenRegion(upcoming_region);
            Attribute = ProbabilitySystem.SampleAttributeGivenElement(Element);

            Debug.Log("============= GENERATED UPCOMING BATTLE =============");
            Debug.Log("Region: " + upcoming_region);
            Debug.Log("Enemy Element: " + Element);
            Debug.Log("Enemy Attribute: " + Attribute);
            Debug.Log("Enemy Health: " + Health);
            Debug.Log("===================================================");
        }

        public static int DetermineEnemyMaxHealth()
        {
            return (100 + 5*MapState.BattlesCompleted);
        }
    };
}
