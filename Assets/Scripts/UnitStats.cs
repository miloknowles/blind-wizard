using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour, IComparable
{
    public int health;
    public int attack;
    public float accuracy;

    public int nextActTurn;
    private bool dead = false;

    public Primitives.Element currElement;
    public Primitives.Attribute currAttribute;

    private void Start()
    {
        // health = Stats.EnemyStats.Health;
        // attack = Stats.EnemyStats.Attack;
        // currElement = Stats.EnemyStats.Element;
        // currAttribute = Stats.EnemyStats.Attribute;
    }

    public void calculateNextActTurn(int currentTurn)
    {
        //this.nextActTurn = currentTurn + (int)Math.Ceiling(100.0f / this.speed);
        this.nextActTurn = currentTurn + 100;
    }

    public int CompareTo(object otherStats)
    {
        return nextActTurn.CompareTo(((UnitStats)otherStats).nextActTurn);
    }

    public bool isDead()
    {
        return this.dead;
    }

    public int superEffective(UnitStats other)
    {
        if(currElement == Primitives.Element.Fire)
        {
            if(other.currElement == Primitives.Element.Fire || other.currElement == Primitives.Element.Earth)
            {
                return 0;
            } else if (other.currElement == Primitives.Element.Water)
            {
                return -1;
            } else
            {
                return 1;
            }
        } else if (currElement == Primitives.Element.Water)
        {
            if (other.currElement == Primitives.Element.Water || other.currElement == Primitives.Element.Air)
            {
                return 0;
            }
            else if (other.currElement == Primitives.Element.Earth)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        } else if (currElement == Primitives.Element.Earth)
        {
            if (other.currElement == Primitives.Element.Fire || other.currElement == Primitives.Element.Earth)
            {
                return 0;
            }
            else if (other.currElement == Primitives.Element.Air)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        } else 
        {
            if (other.currElement == Primitives.Element.Air || other.currElement == Primitives.Element.Water)
            {
                return 0;
            }
            else if (other.currElement == Primitives.Element.Fire)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

    public void recieveDamage(float damage)
    {
        this.health -= (int)damage;
        Debug.Log("Recieved " + damage + " damage.");
        if (this.health <= 0)
        {
            this.dead = true;
            this.gameObject.tag = "DeadUnit";
            Debug.Log(this.gameObject.name + " killed!");
            Destroy(this.gameObject);
        }
    }
}
