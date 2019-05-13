using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Primitives;
using TMPro;

public class RewardPanel : MonoBehaviour
{
    public Button healButton;
    public Button sampleButton;
    public Button damageButton;

    public GameObject UIDescribeUpdatesText;

    private Region region;
    private Attack attack;
    private static System.Random rng = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        this.region = ProbabilitySystem.SampleRegionUniform();
        this.attack = SampleAttackUniform();

        // Fill in the attack placeholder.
        UIDescribeUpdatesText.GetComponent<TextMeshProUGUI>().text =
            UIDescribeUpdatesText.GetComponent<TextMeshProUGUI>().text.Replace("$ATTACK$", this.attack.GetType().Name);

        // Fill in the region placeholder.
        UIDescribeUpdatesText.GetComponent<TextMeshProUGUI>().text =
            UIDescribeUpdatesText.GetComponent<TextMeshProUGUI>().text.Replace("$REGION$", System.Enum.GetName(typeof(Region), this.region));

        healButton.onClick.AddListener(HealButtonClicked);
        sampleButton.onClick.AddListener(SampleButtonClicked);
        damageButton.onClick.AddListener(DamageButtonClicked);

        // Indicate that we have completed another battle!
        GameStateManager.MapState.BattlesCompleted += 1;
        GameStateManager.GameConstants.GENERIC_ENEMY_ATTACK_DAMAGE += 3; //Makes the enemy more powerful for each battle you fight
    }

    void HealButtonClicked()
    {
        //Instead of healing the player, increase the wizard's maximum health
        //GameStateManager.PlayerStats.Health = Mathf.Min(GameStateManager.PlayerStats.Health + 20, 100);
        GameStateManager.PlayerStats.MaxHealth = GameStateManager.PlayerStats.MaxHealth + 20;
        GameStateManager.PlayerStats.Health = GameStateManager.PlayerStats.Health + 20;
        SceneManager.LoadScene("MapScene");
    }

    void SampleButtonClicked()
    {
        GameStateManager.PlayerStats.AddSamplesForRegion(this.region, 15);
        SceneManager.LoadScene("MapScene");
    }

    void DamageButtonClicked()
    {
        GameStateManager.GameConstants.UpgradeDamage(this.attack, 10);
        SceneManager.LoadScene("MapScene");
    }

    private Attack SampleAttackUniform()
    {
        List<Attack> choices = new List<Attack>(){ new Punch(), new Kick(), new Tackle() };
        List<double> uniform_attack_dist = new List<double>(){ 0.333, 0.333, 0.333 };
        int random_index = ProbabilitySystem.SampleIndex(uniform_attack_dist);
        return choices[random_index];
    }
}
