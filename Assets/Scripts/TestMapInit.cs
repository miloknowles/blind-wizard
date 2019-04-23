using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMapInit : MonoBehaviour
{
    public int enemyHealth = 100;
    public int enemyAttack = 10;
    public float enemyAccuracy = .6f;

    // Start is called before the first frame update
    void Start()
    {
        // Primitives.Region reg = Primitives.Region.MMTTG;
        // string st = "";
        // switch ((int) Random.Range(0, 3.99999999f))
        // {
        //     case 0:
        //         reg = Primitives.Region.Volcano;
        //         st = "Volcano";
        //         break;

        //     case 1:
        //         reg = Primitives.Region.Ocean;
        //         st = "Ocean";
        //         break;

        //     case 2:
        //         reg = Primitives.Region.Underground;
        //         st = "Underground";
        //         break;

        //     case 3:
        //         reg = Primitives.Region.Cloud;
        //         st = "Cloud";
        //         break;
        // }
        // Stats.PlayerStats.Region = reg;
        // gameObject.GetComponent<Text>().text = st;
        // Stats.EnemyStats.Health = enemyHealth;
        // Stats.EnemyStats.Attack = enemyAttack;
        // Stats.EnemyStats.Accuracy = enemyAccuracy;
        // Stats.EnemyStats.Element = ProbabilitySystem.SampleElementGivenRegion(reg);
    }
}
