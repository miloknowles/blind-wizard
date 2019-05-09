using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primitives;

public class UpdateZoneSprites : MonoBehaviour
{
    private GameObject[] zones;
     void Start()     {
        zones = GameObject.FindGameObjectsWithTag("BattleTriggerNode");         foreach (GameObject zone in zones)
        {
            Region random_region = ProbabilitySystem.SampleRegionUniform();             string spr = "";
             if (random_region == Region.City)             {
                spr = "city";             }
            else if (random_region == Region.Forest)             {
                spr = "forest";
            }
            else if (random_region == Region.Mountain)             {                 spr = "mountain";             }
            else if (random_region == Region.Storm)             {
                spr = "storm";             }
            else if (random_region == Region.Plains)             {
                spr = "plains";             }
            else if (random_region == Region.Village)             {
                spr = "village";             }             zone.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spr);
        }
    }
} 
