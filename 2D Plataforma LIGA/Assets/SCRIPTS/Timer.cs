using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    //Basicamente a mesma coisa que a classe PlayerHealth
    private float currentTimer;
    private float secondsHandler;

    private bool isCounting = true;

    public event EventHandler OnSecondPassed;

    private void Start()
    {
        isCounting = true;
        currentTimer = 0;
    }

    private void Update()
    {
        if (isCounting)
        {
            //Incrementa o Timer pelo Time.deltaTime
            currentTimer += Time.deltaTime;
            secondsHandler += Time.deltaTime;
        }

        //Como os milésimos de segundos não entram na UI, eu pego apenas os segundos
        if (secondsHandler > 1f)
        {
            //E disparo o evento sempre que 1 segundo inteiro passar
            OnSecondPassed?.Invoke(this, EventArgs.Empty);
            secondsHandler -= 1;
        }
    }

    //Para de contar
    public void StopTimer()
    {
        isCounting = false;
    }

    public float GetCurrentTime()
    {
        return currentTimer;
    }
}
