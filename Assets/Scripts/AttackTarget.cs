using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour
{
    public GameObject owner;

    public float accuracy;
    public float accuracyModifier;
    public float damage;

    public void hit(GameObject target)
    {
        UnitStats ownerStats = this.owner.GetComponent<UnitStats>();
        UnitStats targetStats = target.GetComponent<UnitStats>();

        float hit = Random.value;
        if(accuracy + ownerStats.superEffective(targetStats) >= hit)
        {
            targetStats.recieveDamage(this.damage);
        } else
        {
            Debug.Log("Missed!");
        }
        GameObject.Find("TurnSystem").GetComponent<TurnSystem>().nextTurn();
    }
}
