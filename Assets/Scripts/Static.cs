using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : MonoBehaviour
{
    public static class PlayerStats
    {
        private static int health; 

        public static int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }
    }

    public static class EnemyStats
    {
        private static int health, damage, accuracy;

        public static int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public static int Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }

        public static int Accuracy
        {
            get
            {
                return accuracy;
            }
            set
            {
                accuracy = value;
            }
        }
    }
}
