using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damagable playerDamagable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player == null )
        {
            Debug.Log("No player found");
        }

        playerDamagable = player.GetComponent<Damagable>();
    }
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerDamagable.Health, playerDamagable.MaxHealth);
        healthBarText.text = playerDamagable.Health + " / " + playerDamagable.MaxHealth;
    }

    private void OnEnable()
    {
        playerDamagable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        playerDamagable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = newHealth + " / " + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
