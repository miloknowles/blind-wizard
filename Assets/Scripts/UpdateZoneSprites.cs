using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primitives;
using TMPro;

public class UpdateZoneSprites : MonoBehaviour
{
    void Start()
    {
        GameObject[] battle_trigger_nodes = GameObject.FindGameObjectsWithTag("BattleTriggerNode");
        GameObject[] hearts = GameObject.FindGameObjectsWithTag("Heart");
        
        foreach (GameObject btn in battle_trigger_nodes) {
            // If this node hasn't already been assigned a Region, do that first.
            if (!GameStateManager.MapState.BattleNodes.ContainsKey(btn.name)) {
                Region random_region = ProbabilitySystem.SampleRegionUniform();
                GameStateManager.MapState.BattleNodes[btn.name] = new BattleNodeInfo(random_region);
            }
            // Now make sure this node's sprite is set to the correct file.
            btn.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(
                    GameStateManager.MapState.BattleNodes[btn.name].sprite);

            GameObject txt = btn.transform.Find("TextMeshPro").gameObject;
            txt.GetComponent<TextMeshPro>().text = GameStateManager.MapState.BattleNodes[btn.name].region.ToString();
        }

        foreach(GameObject ht in hearts)
        {
            // List all the hearts in the persistent dictionary with the flag collected = false.
            if(!GameStateManager.MapState.hearts.ContainsKey(ht.name))
            {
                GameStateManager.MapState.hearts[ht.name] = false;
            } // And destroy all collected hearts.
            else if(GameStateManager.MapState.hearts[ht.name])
            {
                Destroy(ht);
            }
        }
    }
}
