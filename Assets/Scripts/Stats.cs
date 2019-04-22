using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static class PlayerStats
    {
        private static int health; 
        private static Primitives.Region region;

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

        public static Primitives.Region Region
        {
            get
            {
                return region;
            }
            set
            {
                region = value;
            }
        }
    }

    public static class EnemyStats
    {
        private static int health, attack, accuracy;
        private static Primitives.Element element;
        private static Primitives.Attribute attribute;

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

        public static int Attack
        {
            get
            {
                return attack;
            }
            set
            {
                attack = value;
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

        public static Primitives.Element Element
        {
            get
            {
                return element;
            }
            set
            {
                element = value;
            }
        }

        public static Primitives.Attribute Attribute
        {
            get
            {
                return attribute;
            }
            set
            {
                attribute = value;
            }
        }
    }
}
