using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;

public class StageClear : MonoBehaviour
{
    //Script presente no objeto final da fase, onde o player precisa tocar para terminá-la

    [SerializeField] private Transform endUI;
    [SerializeField] private TextMeshProUGUI deaths, time;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Se acontece uma colisão com a tag Player, da trigger na função Clear pra mostrar as informações
        if (other.CompareTag("Player"))
        {
            Clear();
        }
    }
    private void Clear()
    {
        Analytics.CustomEvent("LevelClear");
        endUI.gameObject.SetActive(true);
        deaths.text = StageManager.Instance.GetDamageTaken().ToString();
        time.text = StageManager.Instance.GetTime();
    }
}
