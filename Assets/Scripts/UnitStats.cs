using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour, IComparable
{
    public float health;
    public float attack;
    public float defense;
    public float speed;

    public int nextActTurn;
    private bool dead = false;

    public enum Element { Fire, Water, Earth, Air}
    public Element currElement;

    public void calculateNextActTurn(int currentTurn)
    {
        this.nextActTurn = currentTurn + (int)Math.Ceiling(100.0f / this.speed);
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
        if(currElement == Element.Fire)
        {
            if(other.currElement == Element.Fire || other.currElement == Element.Earth)
            {
                return 0;
            } else if (other.currElement == Element.Water)
            {
                return -1;
            } else
            {
                return 1;
            }
        } else if (currElement == Element.Water)
        {
            if (other.currElement == Element.Water || other.currElement == Element.Air)
            {
                return 0;
            }
            else if (other.currElement == Element.Earth)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        } else if (currElement == Element.Earth)
        {
            if (other.currElement == Element.Fire || other.currElement == Element.Earth)
            {
                return 0;
            }
            else if (other.currElement == Element.Air)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        } else 
        {
            if (other.currElement == Element.Air || other.currElement == Element.Water)
            {
                return 0;
            }
            else if (other.currElement == Element.Fire)
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
        this.health -= damage;
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
