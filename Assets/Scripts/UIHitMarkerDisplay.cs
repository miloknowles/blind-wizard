using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHitMarkerDisplay : MonoBehaviour
{
    private GameObject UIHitText;
    private System.Random rng = new System.Random();

    void Start()
    {
        this.gameObject.SetActive(false); // Hide at start of the game.
        UIHitText = transform.Find("UIHitText").gameObject;
    }

    private void ChooseRandomTilt()
    {
        double random_value = rng.NextDouble();
        float direction = (rng.NextDouble() >= 0.5) ? 1.0f : -1.0f;
        float z_tilt = 30.0f * direction * (float)random_value; // Choose a random tilt angle.
        this.transform.rotation = Quaternion.Euler(0, 0, z_tilt);
    }    

    public void ShowHit() { ShowHitSuccess(true); }

    public void ShowMiss() { ShowHitSuccess(false); }

    private void ShowHitSuccess(bool did_hit)
    {
        ChooseRandomTilt();
        UIHitText.GetComponent<TextMeshProUGUI>().text = did_hit ? "Attack hit!" : "Attack missed!";
        UIHitText.GetComponent<TextMeshProUGUI>().color = did_hit ? new Color32(0, 255, 0, 255) : new Color32(255, 0, 0, 255);
        this.gameObject.SetActive(true);

        IEnumerator coroutine = WaitAndDisappear();
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndDisappear()
    {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }
}
