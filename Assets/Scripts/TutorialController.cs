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
        // This panel is anchored to the bottom left.
        UIConfirmInfoButton.transform.SetParent(UIAttributeInfoPanel.transform, false);
        UIConfirmInfoButton.transform.localPosition = new Vector3(350.0f, 0.0f, 0.0f);

        UIAttributeInfoPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowEnemy()
    {
        StartCoroutine(FadeInEnemy(0.2f));

        float button_width = UIConfirmInfoButton.GetComponent<RectTransform>().rect.width;
        float button_height = UIConfirmInfoButton.GetComponent<RectTransform>().rect.height;
        UIConfirmInfoButton.transform.SetParent(this.gameObject.transform, false);
        UIConfirmInfoButton.transform.position =
            new Vector3(0.5f*Screen.width - 0.5f*button_width, 0.5f * Screen.height - button_height - 10.0f, 0.0f);

        enemyObject.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIChooseAttack()
    {
        float button_width = UIConfirmInfoButton.GetComponent<RectTransform>().rect.width;
        float button_height = UIConfirmInfoButton.GetComponent<RectTransform>().rect.height;

        UIConfirmInfoButton.transform.SetParent(this.gameObject.transform, false);
        UIConfirmInfoButton.transform.position =
            new Vector3(0.5f*Screen.width - 0.5f*button_width, 0.5f * Screen.height, 0.0f);

        UIChooseAttackPanel.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    private void ShowUIMoveLog()
    {
        float button_width = UIConfirmInfoButton.GetComponent<RectTransform>().rect.width;
        float button_height = UIConfirmInfoButton.GetComponent<RectTransform>().rect.height;

        float menu_height = UIMoveLogScrollView.GetComponent<RectTransform>().rect.height;

        UIConfirmInfoButton.transform.SetParent(UIMoveLogScrollView.transform, false);
        UIConfirmInfoButton.transform.localPosition = new Vector3(-button_width, -menu_height - button_height - 10.0f, 0.0f);

        UIMoveLogScrollView.SetActive(true);
        UIConfirmInfoButton.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Running the tutorial controller!");
        HideAll();

        // Make the transition smoother with a fade in.
        StartCoroutine(FadeInScene(1.0f));

        UIConfirmInfoButton.GetComponent<Button>().onClick.AddListener(() => this.OnClick());

        // Add all of the UI reveal actions that require a confirm button to a queue.
        queue.Enqueue(() => this.ShowUIRegionSamples());
        queue.Enqueue(() => this.ShowUIAttributeInfo());
        queue.Enqueue(() => this.ShowEnemy());
        queue.Enqueue(() => this.ShowUIChooseAttack());
        queue.Enqueue(() => this.ShowUIMoveLog());

        // Don't need to wait for a click to show the player stats bar at the bottom.
        ShowUIPlayerStats();

        // Pop off the first action (player doesn't have to click yet).
        Action first_action = queue.Dequeue();
        first_action();
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
