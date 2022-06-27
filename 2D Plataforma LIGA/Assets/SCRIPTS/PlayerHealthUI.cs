using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    //Controla a UI da vida do player

    [SerializeField] private TextMeshProUGUI healthAmount;

    private void Start()
    {
        //Se inscreve no evento, coloquei a classe PlayerHealth com um método Singleton justamente pra fazer a
        //inscrição sem referenciar
        PlayerHealth.Instance.OnHealthChanged += PlayerHealth_OnHealthChanged;

        UpdateHealthUI();
    }

    //A função de evento que vai ser chamada sempre que o evento do PlayerHealth for acionado
    private void PlayerHealth_OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthAmount.text = PlayerHealth.Instance.GetHealthAmount().ToString();
    }
}
