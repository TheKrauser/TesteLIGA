using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Analytics;

public class PlayerHealth : MonoBehaviour
{
    //Singleton
    public static PlayerHealth Instance { get; private set; }

    //Valor da vida máxima, setado no Inspector
    [SerializeField] private int healthMax;

    //Valor real da atual vida do player
    private int health;

    //Evento que é acionado ao haver mudança na vida
    public event EventHandler OnHealthChanged;

    private PlayerMovement player;

    private void Awake()
    {
        Instance = this;

        player = GetComponent<PlayerMovement>();

        health = healthMax;
    }

    public void Damage()
    {
        if (health > 0)
        {
            health--;
            //Chama a classe StageManager pelo Singleton criado nela, assim não precisando referenciar nem nada
            StageManager.Instance.IncrementDamageTaken();

            if (health <= 0)
            {
                StageManager.Instance.StopTimer();
                InterstitialAD.Instance.ShowAd();
                player.SetDead();
            }
        }

        //Onde o evento é acionado, sempre que leva dano

        //Esse evento não liga se alguém está ou não ouvindo, então mesmo se remover a UI das vidas
        //ele vai funcionar normalmente sem apresentar dar erros
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetHealthAmount()
    {
        return health;
    }
}
