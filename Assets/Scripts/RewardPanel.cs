﻿using System.Collections;
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

    private Region currentRegion;

    // Start is called before the first frame update
    void Start()
    {
        currentRegion = GameStateManager.MapState.CurrentRegion;
        GameStateManager.PlayerStats.AddSamplesForRegion(currentRegion, 10);
        this.region = ProbabilitySystem.SampleRegionUniform();
        this.attack = SampleAttackUniform();

        damageButton.GetComponentInChildren<TextMeshProUGUI>().text = "+5 " + this.attack.GetType().Name + " Damage";
        sampleButton.GetComponentInChildren<TextMeshProUGUI>().text = "+20 samples for " + System.Enum.GetName(typeof(Region), this.region);

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
        GameStateManager.GameConstants.GENERIC_ENEMY_ATTACK_DAMAGE += (int) Mathf.Floor(4 / Mathf.Sqrt( GameStateManager.MapState.BattlesCompleted)); //Makes the enemy more powerful for each battle you fight
    }

    void HealButtonClicked()
    {
        //Instead of healing the player, increase the wizard's maximum health
        //GameStateManager.PlayerStats.Health = Mathf.Min(GameStateManager.PlayerStats.Health + 20, 100);
        GameStateManager.PlayerStats.MaxHealth = GameStateManager.PlayerStats.MaxHealth + 10;
        GameStateManager.PlayerStats.Health = GameStateManager.PlayerStats.Health + 10;
        SceneManager.LoadScene("MapScene");
    }

    void SampleButtonClicked()
    {
        GameStateManager.PlayerStats.AddSamplesForRegion(this.region, 20);
        SceneManager.LoadScene("MapScene");
    }

    void DamageButtonClicked()
    {
        GameStateManager.GameConstants.UpgradeDamage(this.attack, 5);
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
