using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]private GameObject telaDeCarregamento;
    [SerializeField] private Slider barraDeCarregamento;
    [SerializeField] private Text textoDeCarregamento;

    public void CarregarNovaCena(string nomeDaNovaCena)
    {
        XpManager.SaveXpAsCheckpoint();
        //SceneManager.LoadScene(nomeDaNovaCena);
        StartCoroutine(CarregarNovaCenaEmSegundoPlano(nomeDaNovaCena));
    }

    private IEnumerator CarregarNovaCenaEmSegundoPlano(string nomeDaNovaCena)
    {
        AsyncOperation carregamento = SceneManager.LoadSceneAsync(nomeDaNovaCena);

        telaDeCarregamento.SetActive(true);

        while (carregamento.isDone == false)
        {
            float progressoDoCarregamento = Mathf.Clamp01(carregamento.progress / 0.9f);
            barraDeCarregamento.value = progressoDoCarregamento;
            textoDeCarregamento.text = progressoDoCarregamento * 100f + "%";

            yield return null;
        }
    }
}
