using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject UIRegionSamplesPanel;
    public GameObject UIAttributeInfoPanel;
    public GameObject TutorialText_Attribute;
    public GameObject UIChooseAttackPanel;
    public GameObject TutorialText_AttackPanel;
    public GameObject UIMoveLogScrollView;
    public GameObject TutorialText_MoveLog;
    public GameObject enemyObject;
    public GameObject TutorialText_Enemy;
    public GameObject UIPlayerStatsPanel;
    public GameObject TutorialText_PlayerStats;
    public GameObject UIConfirmInfoButton;
    public GameObject UIBlinkingDarkOccluder;

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

        HideAllTutorialText();
    }

    private void HideAllTutorialText()
    {
        TutorialText_Enemy.SetActive(false);
        TutorialText_AttackPanel.SetActive(false);
        TutorialText_MoveLog.SetActive(false);
        TutorialText_PlayerStats.SetActive(false);
        TutorialText_Attribute.SetActive(false);
    }

    /*
     * Fade in the scene by slowly reducing the alpha on an occluder.
     */
    private IEnumerator FadeInScene(float duration)
    {
        Color initial_color = UIBlinkingDarkOccluder.GetComponent<Image>().color;
        Color color = UIBlinkingDarkOccluder.GetComponent<Image>().color;
        color.a = 1.0f;

        UIBlinkingDarkOccluder.SetActive(true);

        float wait_time = duration / 100.0f;
        while (duration > 0.0f) {
            duration -= wait_time;
            color.a -= 0.01f;
            color.a = Mathf.Max(0, color.a);
            UIBlinkingDarkOccluder.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(wait_time);
        }

        UIBlinkingDarkOccluder.SetActive(false);
        UIBlinkingDarkOccluder.GetComponent<Image>().color = initial_color;
    }

    private IEnumerator FadeInEnemy(float duration)
    {
        Color color = enemyObject.GetComponent<Image>().color;
        color.a = 0.0f;

        float wait_time = duration / 100.0f;

        while (duration > 0.0f) {
            duration -= wait_time;
            color.a += 0.01f;
            color.a = Mathf.Min(1.0f, color.a);
            enemyObject.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(wait_time);
        }
    }

    private void ShowUIPlayerStats()
    {
        HideAllTutorialText();
        TutorialText_PlayerStats.SetActive(true);

        float button_height = UIConfirmInfoButton.GetComponent<RectTransform>().rect.height;
        float button_width = UIConfirmInfoButton.GetComponent<RectTransform>().rect.width;

        UIConfirmInfoButton.transform.SetParent(TutorialText_PlayerStats.transform, false);
        UIConfirmInfoButton.transform.localPosition = new Vector3(-0.5f*button_width, 10.0f, 0.0f);

        UIPlayerStatsPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIRegionSamples()
    {
        // This panel is anchored to the top left.
        UIConfirmInfoButton.transform.SetParent(UIRegionSamplesPanel.transform, false);
        UIConfirmInfoButton.transform.localPosition = new Vector3(0.0f, -90.0f, 0.0f);

        UIRegionSamplesPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIAttributeInfo()
    {
        HideAllTutorialText();
        TutorialText_Attribute.SetActive(true);

        // This panel is anchored to the bottom left.
        UIConfirmInfoButton.transform.SetParent(UIAttributeInfoPanel.transform, false);
        UIConfirmInfoButton.transform.localPosition = new Vector3(350.0f, 0.0f, 0.0f);

        UIAttributeInfoPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowEnemy()
    {
        StartCoroutine(FadeInEnemy(0.2f));

        HideAllTutorialText();
        TutorialText_Enemy.SetActive(true);

        float button_width = UIConfirmInfoButton.GetComponent<RectTransform>().rect.width;
        float button_height = UIConfirmInfoButton.GetComponent<RectTransform>().rect.height;
        UIConfirmInfoButton.transform.SetParent(TutorialText_Enemy.transform, false);
        UIConfirmInfoButton.transform.localPosition = new Vector3(0.0f, -button_height - 10.0f, 0.0f);

        enemyObject.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIChooseAttack()
    {
        HideAllTutorialText();
        TutorialText_AttackPanel.SetActive(true);

        float button_width = UIConfirmInfoButton.GetComponent<RectTransform>().rect.width;
        float button_height = UIConfirmInfoButton.GetComponent<RectTransform>().rect.height;

        UIConfirmInfoButton.transform.SetParent(TutorialText_AttackPanel.transform, false);
        UIConfirmInfoButton.transform.localPosition =
            new Vector3(0.0f, 10.0f, 0.0f);

        UIChooseAttackPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIMoveLog()
    {
        HideAllTutorialText();
        TutorialText_MoveLog.SetActive(true);

        float button_width = UIConfirmInfoButton.GetComponent<RectTransform>().rect.width;
        float button_height = UIConfirmInfoButton.GetComponent<RectTransform>().rect.height;

        float menu_height = UIMoveLogScrollView.GetComponent<RectTransform>().rect.height;

        UIConfirmInfoButton.transform.SetParent(TutorialText_MoveLog.transform, false);
        UIConfirmInfoButton.transform.localPosition = new Vector3(0.0f, -button_height - 10.0f, 0.0f);

        UIMoveLogScrollView.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Running the tutorial controller!");

        // Make the transition smoother with a fade in.
        StartCoroutine(FadeInScene(1.0f));

        UIConfirmInfoButton.GetComponent<Button>().onClick.AddListener(() => this.OnClick());
        UIConfirmInfoButton.SetActive(false);

        HideAllTutorialText();

        if (GameStateManager.MapState.TutorialMode) {
            // Battle #1: Only have region info and attack info.
            if (GameStateManager.MapState.BattlesCompleted == 0) {
                Debug.Log("Battle #3: Introduction region info.");
                HideAll();
                queue.Enqueue(() => this.ShowUIRegionSamples());
                queue.Enqueue(() => this.ShowEnemy());
                queue.Enqueue(() => this.ShowUIChooseAttack());
                queue.Enqueue(() => this.ShowUIPlayerStats());
                queue.Enqueue(() => this.ShowUIMoveLog());

                // Pop off the first action (player doesn't have to click yet).
                Action first_action = queue.Dequeue();
                first_action();
            
            // Battle #2 : Also have attribute info.
            } else if (GameStateManager.MapState.BattlesCompleted == 1) {
                Debug.Log("Battle #2: Introducing attribute info.");
                HideAll();
                this.ShowUIRegionSamples();
                this.ShowEnemy();
                this.ShowUIChooseAttack();
                this.ShowUIPlayerStats();
                this.ShowUIMoveLog();

                queue.Enqueue(() => this.ShowUIAttributeInfo());

                // Pop off the first action (player doesn't have to click yet).
                Action first_action = queue.Dequeue();
                first_action();
            } else {
                Debug.Log("Battle #3: No more tutorial info to present.");
            }
        }
    }

    // Each confirm button click removes something from the queue.
    private void OnClick()
    {
        if (queue.Count > 0) {
            Action action = queue.Dequeue();
            action();
        } else {
            UIConfirmInfoButton.SetActive(false);
            HideAllTutorialText();
        }
    }
}
