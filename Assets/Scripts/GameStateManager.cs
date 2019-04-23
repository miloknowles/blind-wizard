using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primitives;

public class GameStateManager : MonoBehaviour
{
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
        public static int Health { get; set; }
        public static Region Region { get; set; }
    };

    /*
     * The enemy state is stored in this static class.
     *
     * Note: when entering the battle scene, the enemy stats should already be set
     * here; the code in the battle scene expects to read what type of enemy to 
     * render and face the player against from here.
     */
    public static class EnemyStats {
        public static int Health { get; set; }
        public static int Damage { get; set; }
        public static double Accuracy { get; set; }
        public static Attribute Attribute { get; set; }
        public static Element Element { get; set; }
    };
}
