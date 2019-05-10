using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject UIRegionSamplesPanel;
    public GameObject UIAttributeInfoPanel;
    public GameObject UIChooseAttackPanel;
    public GameObject UIMoveLogScrollView;
    public GameObject enemyObject;
    public GameObject UIPlayerStatsPanel;
    public GameObject UIConfirmInfoButton;

    private Queue<Action> queue = new Queue<Action>();

    private void HideAll()
    {
        UIRegionSamplesPanel.SetActive(false);
        UIAttributeInfoPanel.SetActive(false);
        UIChooseAttackPanel.SetActive(false);
        UIMoveLogScrollView.SetActive(false);
        enemyObject.SetActive(false);
        UIPlayerStatsPanel.SetActive(false);
        UIConfirmInfoButton.SetActive(false);
    }

    private void ShowUIPlayerStats()
    {
        UIPlayerStatsPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIRegionSamples()
    {
        UIRegionSamplesPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIAttributeInfo()
    {
        UIAttributeInfoPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowEnemy()
    {
        enemyObject.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIChooseAttack()
    {
        UIChooseAttackPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIMoveLog()
    {
        UIMoveLogScrollView.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Running the tutorial controller!");
        HideAll();

        UIConfirmInfoButton.GetComponent<Button>().onClick.AddListener(() => this.OnClick());

        // Add all of the UI reveal actions to a queue.
        queue.Enqueue(() => this.ShowUIPlayerStats());
        queue.Enqueue(() => this.ShowUIRegionSamples());
        queue.Enqueue(() => this.ShowUIAttributeInfo());
        queue.Enqueue(() => this.ShowEnemy());
        queue.Enqueue(() => this.ShowUIChooseAttack());
        queue.Enqueue(() => this.ShowUIMoveLog());

        UIConfirmInfoButton.SetActive(true);
    }

    // Each confirm button click removes something from the queue.
    private void OnClick()
    {
        if (queue.Count > 0) {
            Action action = queue.Dequeue();
            action();
        } else {
            UIConfirmInfoButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
