using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform main, options, stages;

    public void StartGame()
    {
        main.gameObject.SetActive(false);
        stages.gameObject.SetActive(true);
    }

    public void Options()
    {
        main.gameObject.SetActive(false);
        options.gameObject.SetActive(true);
    }

    //0 == STAGE >>> MAIN
    //1 == OPTIONS >>> MAIN
    public void Back(int i)
    {
        if (i == 0)
        {
            stages.gameObject.SetActive(false);
            main.gameObject.SetActive(true);
        }
        else
        {
            options.gameObject.SetActive(false);
            main.gameObject.SetActive(true);
        }
    }

    public void StageSelect(int stageNumber)
    {
        LoadingManager.Instance.LoadScene(stageNumber);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
