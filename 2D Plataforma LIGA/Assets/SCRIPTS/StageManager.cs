using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //Armazenam o dano tomado na fase (não conta resets) e o tempo em formato de string
    private int damageTaken = 0;
    private string time = "";

    //Singleton para acessar em outras classes, com set privado para não ser modificado por elas
    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void IncrementDamageTaken()
    {
        damageTaken++;
    }

    public void FillTimer(string s)
    {
        time = s;
    }

    public int GetDamageTaken()
    {
        return damageTaken;
    }

    public string GetTime()
    {
        return time;
    }

    public void BackToMenu(int index)
    {
        LoadingManager.Instance.LoadScene(index);
    }
}
