using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primitives;

using ElementSamples = System.Collections.Generic.Dictionary<Primitives.Element, int>;

public class GameStateManager : MonoBehaviour
{
    public static class MapState {
        public static Region CurrentRegion { get; set; }
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
            if (!isInitialized) {
                isInitialized = true;
                Health = 100;
            }
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
            }
            
            foreach (Primitives.Element el in System.Enum.GetValues(typeof(Primitives.Element))) {
                GameStateManager.PlayerStats.Samples[region][el] = 0;
            }

            for (int i = 0; i < N; i++) {
                Element el = ProbabilitySystem.SampleElementGivenRegion(region);
                PlayerStats.Samples[region][el] += 1;
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
        public static int Health { get; set; }
        public static Attribute Attribute { get; set; }
        public static Element Element { get; set; }

        // TODO: take in a region here eventually, just making something to test...
        public static void Generate()
        {
            Region random_region = ProbabilitySystem.SampleRegionUniform();
            MapState.CurrentRegion = random_region;

            Health = 100;
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
