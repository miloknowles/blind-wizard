using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateLevelNumber : MonoBehaviour
{
    public TMP_Text leveltext;
    // Start is called before the first frame update
    void Start()
    {
        leveltext.text = "Level " + (GameStateManager.MapState.BattlesCompleted+1); //1 indexing level numbers for player
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
