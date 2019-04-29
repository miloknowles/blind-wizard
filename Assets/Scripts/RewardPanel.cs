using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    public Button healButton;
    public Button sampleButton;
    public Text text;
    private Primitives.Region region;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameStateManager.PlayerStats.isInitialized)
            gameObject.SetActive(false);
        region = ProbabilitySystem.SampleRegionUniform();
        text.text = text.text.Replace("%", System.Enum.GetName(typeof(Primitives.Region), region));
        healButton.onClick.AddListener(HealButtonClicked);
        sampleButton.onClick.AddListener(HealButtonClicked);
    }

    void HealButtonClicked()
    {
        GameStateManager.PlayerStats.Health = Mathf.Min(GameStateManager.PlayerStats.Health + 10, 100);
        gameObject.SetActive(false);
    }

    void SampleButtonClicked()
    {
        GameStateManager.PlayerStats.AddSamplesForRegion(region, 15);
        gameObject.SetActive(false);
    }

}
