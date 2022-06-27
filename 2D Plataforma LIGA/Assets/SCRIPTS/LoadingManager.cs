using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class LoadingManager : MonoBehaviour
{
    [Header("Transition Duration")]
    //O tempo que demora para a transição fechar
    [SerializeField] private float outDuration;

    //O tempo que demora para ela abrir
    [SerializeField] private float inDuration;

    //E o tempo de espera após o Loading da fase terminar e ela realizar a próxima ação de abrir ou fechar
    [SerializeField] private float waitTimeAfterLoadingEnds;

    //Os Rects de cada parte das barras pretas e do centro da tela
    [SerializeField] private RectTransform center, up, down, left, right;

    //Variavel da classe SceneManagement que cuida do % que a nova cena já foi carregado
    private AsyncOperation operation;
    
    //Singleton
    public static LoadingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Por ser uma classe presente na cena dos Managers, é usado a função DontDestroy para que ela não seja
        //destruída nas trocas de cena, e como ta em uma cena inicial que só é acessada ao abrir o game,  não precisa
        //se preocupar com duplicatas
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Começa dando Load na cena principal, no caso a do index 1
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadScene((int)SceneIndexes.Indexes.MENU);
        }
    }

    public void LoadScene(int i)
    {
        StartCoroutine(BeginLoad(i));
        Analytics.CustomEvent("SceneLoaded: " + SceneIndexes.GetSceneByInt(i));
    }

    //Valores da largura e altura que o Canvas está usando como base para desenhar a UI na tela
    private static float altura = 900;
    private static float largura = 1600;

    //Valores de posicionamento de cada barra preta na hora da transição
    private float x = largura/4;
    private float y = altura/4;

    private IEnumerator BeginLoad(int sceneInt)
    {
        //Trava tudo que se move na base de tempo no jogo setando o timeScale em 0
        //Assim dando a sensação de que está pausado
        Time.timeScale = 0f;

        //Usando DOTween pra realizar a movimentação da transição mais facilmente

        //SetUpdate(true) serve para especificar que a transição não deve ligar para o timeScale atual
        //então irá funcionar mesmo com tudo parado no timeScale 0
        up.DOAnchorPosY(-y, outDuration).SetUpdate(true);
        down.DOAnchorPosY(y, outDuration).SetUpdate(true).OnComplete(() =>
        {
            //Ao completar a ação de uma delas, realiza essas, que posiciona todas no meio para tampar a tela
            right.DOAnchorPosX(x, 0).SetUpdate(true);
            left.DOAnchorPosX(-x, 0).SetUpdate(true);
            up.DOAnchorPosY(y, 0).SetUpdate(true);
            down.DOAnchorPosY(-y, 0).SetUpdate(true);
        });

        //Espera a duração de fechada
        yield return new WaitForSecondsRealtime(outDuration);

        //Começa a carregar a cena
        operation = SceneManager.LoadSceneAsync(sceneInt);

        //Enquanto não carrega, a Coroutine não faz nenhum avanço
        while (!operation.isDone)
        {
            yield return null;
        }

        //Ao terminar, seta o operation como null, espera o tempo de delay ao carregar tudo e realiza a ação
        //de abrir a tela

        operation = null;
        yield return new WaitForSecondsRealtime(waitTimeAfterLoadingEnds);

        right.DOAnchorPosX(x * 3, inDuration).SetUpdate(true);
        left.DOAnchorPosX(x * -3, inDuration).SetUpdate(true).OnComplete(() =>
        {
            //Volta o timeScale para 1
            Time.timeScale = 1f;
        });
    }
}
