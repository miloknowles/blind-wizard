using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primitives;

public class UpdateZoneSprites : MonoBehaviour
{
    void Start()
    {
        GameObject[] battle_trigger_nodes = GameObject.FindGameObjectsWithTag("BattleTriggerNode");
        
        foreach (GameObject btn in battle_trigger_nodes) {
            // If this node hasn't already been assigned a Region, do that first.
            if (!GameStateManager.MapState.BattleNodes.ContainsKey(btn.name)) {
                Region random_region = ProbabilitySystem.SampleRegionUniform();
                GameStateManager.MapState.BattleNodes[btn.name] = new BattleNodeInfo(random_region);
            }
            // Now make sure this node's sprite is set to the correct file.
            btn.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(
                    GameStateManager.MapState.BattleNodes[btn.name].sprite);
        }
    }
}
