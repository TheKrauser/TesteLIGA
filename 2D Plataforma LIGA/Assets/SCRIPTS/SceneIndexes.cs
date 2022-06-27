using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneIndexes
{
    //Classe para armazenar o Index de cada cena do projeto
    public enum Indexes
    {
        MENU = 1,
        FASE_1 = 2,
        FASE_2 = 3,
        FASE_3 = 4,
    }

    public static string GetSceneByInt(int index)
    {
        string sceneName = "";

        foreach (Indexes i in System.Enum.GetValues(typeof(Indexes)))
        {
            if ((int)i == index)
            {
                sceneName = i.ToString();
            }
        }

        return sceneName;
    }
}
