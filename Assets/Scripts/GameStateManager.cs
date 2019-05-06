﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primitives;

using ElementSamples = System.Collections.Generic.Dictionary<Primitives.Element, int>;

public class GameStateManager : MonoBehaviour
{
    public static class MapState {
        public static Region CurrentRegion { get; set; }

        // The world coordinates of the wizard in the MapScene. This is saved right before battle
        // and restored after leaving the battle.
        public static Vector3 WizardPosition = new Vector3(0, 0, 0);
        public static int BattlesCompleted = 0;
        public static List<string> CompletedBattleTriggerNodeNames = new List<string>();
    };

    /*
     * Store all of the importants constants here so we only have to change them
     * in one place. This should also make it easier to adjust game difficulty.
     */
    public static class GameConstants
    {
        public static double SUPER_EFFECTIVE_ACCURACY_BONUS = 0.3;
        public static int GENERIC_ENEMY_ATTACK_DAMAGE = 15;
        public static double GENERIC_ENEMY_ATTACK_ACCURACY = 0.6;
        public static int PUNCH_DAMAGE = 20;
        public static double PUNCH_ACCURACY = 70.0;
        public static int KICK_DAMAGE = 40;
        public static double KICK_ACCURACY = 0.5;
        public static int TACKLE_DAMAGE = 60;
        public static double TACKLE_ACCURACY = 0.3;

        public static void UpgradeDamage(Attack attack_to_upgrade, int hp)
        {
            if (attack_to_upgrade.GetType() == typeof(Punch)) {
                PUNCH_DAMAGE += hp;
            } else if (attack_to_upgrade.GetType() == typeof(Kick)) {
                KICK_DAMAGE += hp;
            } else if (attack_to_upgrade.GetType() == typeof(Tackle)) {
                TACKLE_DAMAGE += hp;
            }
        }
    };

    /*
     * The player state is stored in this static class. This is a way to persist
     * the player state globally across multiple scenes.
     *
     * Note: player stats should be retrieved from here when entering the battle scene.
     */
    public static class PlayerStats {
        // Initialize player stats at the start of a game.
        // To ensure that this is only done once, we maintain an isInitialized flag.
        public static void Initialize() {
            Debug.Log(GameStateManager.PlayerStats.Health);
            if (!isInitialized) {
                isInitialized = true;
                Health = 100;
            }
            Debug.Log(GameStateManager.PlayerStats.Health);
        }
        public static bool isInitialized = false;
        public static bool samplesInitialized = false;

        public static int Health { get; set; }
        public static Region Region { get; set; }

        // For each region, store a dictionary that maps each element to an int.
        public static Dictionary<Primitives.Region, ElementSamples> Samples =
                new Dictionary<Primitives.Region, ElementSamples>();

        /*
         * Initializes a set of N element samples for a given region.
         */
        public static void AddSamplesForRegion(Primitives.Region region, int N)
        {
            PlayerStats.samplesInitialized = true;

            if (!PlayerStats.Samples.ContainsKey(region)) {
                PlayerStats.Samples[region] = new ElementSamples();
                foreach (Primitives.Element el in System.Enum.GetValues(typeof(Primitives.Element)))
                {
                    GameStateManager.PlayerStats.Samples[region][el] = 0;
                }
            }

            for (int i = 0; i < N; i++) {
                Element el = ProbabilitySystem.SampleElementGivenRegion(region);
                PlayerStats.Samples[region][el] += 1;
            }

            Debug.Log(PlayerStats.Samples[region][Element.Water]);
            Debug.Log(PlayerStats.Samples[region][Element.Fire]);
            Debug.Log(PlayerStats.Samples[region][Element.Air]);
            Debug.Log(PlayerStats.Samples[region][Element.Earth]);
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
        public static int Health { get; set; }
        public static Attribute Attribute { get; set; }
        public static Element Element { get; set; }

        // TODO: take in a region here eventually, just making something to test...
        public static void Generate()
        {
            Region random_region = ProbabilitySystem.SampleRegionUniform();
            MapState.CurrentRegion = random_region;

            Health = 100 + 5*MapState.BattlesCompleted;
            Element = ProbabilitySystem.SampleElementGivenRegion(random_region);
            Attribute = ProbabilitySystem.SampleAttributeGivenElement(Element);

            Debug.Log("============= GENERATED RANDOM BATTLE =============");
            Debug.Log("Region: " + random_region);
            Debug.Log("Enemy Element: " + Element);
            Debug.Log("Enemy Attribute: " + Attribute);
            Debug.Log("===================================================");
        }
    };
}
