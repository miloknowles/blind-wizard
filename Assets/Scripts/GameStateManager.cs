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
