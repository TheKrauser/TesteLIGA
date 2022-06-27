using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    //Basicamente igual a classe PlayerHealthUI
    [SerializeField] private TextMeshProUGUI txtTimer;

    [SerializeField] private Timer timer;

    private void Start()
    {
        timer.OnSecondPassed += Timer_OnSecondPassed;
    }

    private void Timer_OnSecondPassed(object sender, System.EventArgs e)
    {
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        float timerMax = timer.GetCurrentTime();

        txtTimer.text = ConvertToHours(timerMax);

        StageManager.Instance.FillTimer(ConvertToHours(timerMax));
    }

    //Função para converter o timer total da fase em (horas)h(minutos)m(segundos)s para encaixar na UI
    private string ConvertToHours(float toConvert)
    {
        float horas, minutos, segundos;
        float resto;

        if (toConvert < 3600)
        {
            horas = 0;
            resto = toConvert;
        }
        else
        {
            horas = toConvert / 3600;
            resto = toConvert % 3600;
        }

        if (toConvert < 60)
        {
            minutos = 0;
            resto = toConvert;
        }
        else
        {
            minutos = resto / 60;
            resto = resto % 60;
        }

        segundos = resto;
        
        return horas.ToString("F0") + "h" + minutos.ToString("F0") + "m" + segundos.ToString("F0") + "s";;
    }
}
