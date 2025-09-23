using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizacaoUI : MonoBehaviour
{
    [SerializeField] private Text textoLocal; 
    [SerializeField] private float duracao = 3f; // Tempo que o texto fica visível
    [SerializeField] private CanvasGroup canvasGroup;

    [TextArea]
    [SerializeField] private string nomeDoLocal = "Floresta Sombria";

    private void Start()
    {
        textoLocal.text = nomeDoLocal;
        StartCoroutine(MostrarTexto());
    }

    private IEnumerator MostrarTexto()
    {
        canvasGroup.alpha = 1;
        canvasGroup.gameObject.SetActive(true);

        yield return new WaitForSeconds(duracao);

        // Fade out
        float fadeDuration = 1f;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1 - (timer / fadeDuration);
            yield return null;
        }

        canvasGroup.gameObject.SetActive(false);
    }
}
