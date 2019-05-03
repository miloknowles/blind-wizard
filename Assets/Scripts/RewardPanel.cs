using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RewardPanel : MonoBehaviour
{
    public Button healButton;
    public Button sampleButton;
    public Text text;
    private Primitives.Region region;

    // Start is called before the first frame update
    void Start()
    {
        region = ProbabilitySystem.SampleRegionUniform();
        text.text = text.text.Replace("%", System.Enum.GetName(typeof(Primitives.Region), region));
        healButton.onClick.AddListener(HealButtonClicked);
        sampleButton.onClick.AddListener(SampleButtonClicked);

        // Indicate that we have completed another battle!
        GameStateManager.MapState.BattlesCompleted += 1;
    }

    void HealButtonClicked()
    {
        GameStateManager.PlayerStats.Health = Mathf.Min(GameStateManager.PlayerStats.Health + 10, 100);
        SceneManager.LoadScene("MapScene");
    }

    void SampleButtonClicked()
    {
        GameStateManager.PlayerStats.AddSamplesForRegion(region, 15);
        SceneManager.LoadScene("MapScene");
    }
}
