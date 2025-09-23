using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Porta : MonoBehaviour
{
    [SerializeField]
    private string nomeProximaFase;
    [SerializeField]
    private LoadingScene loadingScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        loadingScene.CarregarNovaCena(nomeProximaFase);
    }
}
