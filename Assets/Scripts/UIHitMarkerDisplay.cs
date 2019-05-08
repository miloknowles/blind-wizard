using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Primitives;

public class UIHitMarkerDisplay : MonoBehaviour
{
    public GameObject UIPlayerHitEnemy;
    public GameObject UIPlayerHitEnemyText;
    public GameObject UIPlayerMissedEnemy;

    private System.Random rng = new System.Random();

    void Start()
    {
        // UIPlayerHitEnemy = this.transform.Find("UIPlayerHitEnemy").gameObject;
        // UIPlayerHitEnemyText = UIPlayerHitEnemy.transform.Find("UIPlayerHitEnemyText").gameObject;
        // UIPlayerMissedEnemy = this.transform.Find("UIPlayerMissedEnemy").gameObject;

        UIPlayerHitEnemy.SetActive(false);
        UIPlayerHitEnemyText.SetActive(true);
        UIPlayerMissedEnemy.SetActive(false);
    }

    private Quaternion ChooseRandomTilt()
    {
        double random_value = rng.NextDouble();
        float direction = (rng.NextDouble() >= 0.5) ? 1.0f : -1.0f;
        float z_tilt = 30.0f * direction * (float)random_value; // Choose a random tilt angle.
        return Quaternion.Euler(0, 0, z_tilt);
    }    

    public void ShowHit(Attack attack)
    {
        UIPlayerHitEnemy.gameObject.transform.rotation = ChooseRandomTilt();
        UIPlayerHitEnemyText.GetComponent<TextMeshProUGUI>().text = attack.GetType().Name + " hit!";

        // Appear, then disappear in a few seconds.
        UIPlayerHitEnemy.SetActive(true);
        IEnumerator coroutine = WaitAndDisappear(UIPlayerHitEnemy);
        StartCoroutine(coroutine);
    }

    public void ShowMiss(Attack attack)
    {
        UIPlayerMissedEnemy.gameObject.transform.rotation = ChooseRandomTilt();
        UIPlayerMissedEnemy.GetComponent<TextMeshProUGUI>().text = attack.GetType().Name + " missed!";

        // Appear, then disappear in a few seconds.
        UIPlayerMissedEnemy.SetActive(true);
        IEnumerator coroutine = WaitAndDisappear(UIPlayerMissedEnemy);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndDisappear(GameObject object_to_disappear)
    {
        yield return new WaitForSeconds(1.5f);
        object_to_disappear.SetActive(false);
    }
}
