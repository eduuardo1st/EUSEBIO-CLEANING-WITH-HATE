using System.Collections;
using UnityEngine;
using TMPro;

public class EusebioFuryManager : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private TextMeshProUGUI furyText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private UnityEngine.UI.Image buffIcon;

    [Header("Configurações da Mensagem")]
    [SerializeField] private int xpTotalParaAtivar = 74;
    [SerializeField] private PlayerItems playerItems;

    [Header("Configurações do Fade")]
    [SerializeField] private float fadeInTime = 1.0f;
    [SerializeField] private float displayTime = 4.0f;
    [SerializeField] private float fadeOutTime = 1.5f;

    // <<< MUDANÇA AQUI: Adicionado 'static' para persistir entre as cenas
    private static bool messageShown = false; 
    private static bool buffAtivado = false;

    private const string PtText = "O balde de Eusébio está cheio,\nEusébio está <color=red>FURIOSO</color>.";
    private const string EngText = "Eusébio's bucket is full,\nEusébio is <color=red>FURIOUS</color>.";
    private const string SpaText = "El balde de Eusébio está lleno,\nEusébio está <color=red>FURIOSO</color>.";

    void Start()
    {
        // Esta verificação garante que o buff e o ícone estejam ativos se já tiverem sido ganhos
        if (XpManager.XPdoPlayer >= xpTotalParaAtivar)
        {
            // Aplica o buff sem mostrar a mensagem se ele já não estiver ativo
            if (!buffAtivado) 
            {
                Buff(buffIcon);
                buffAtivado = true;
            }
            // Apenas ativa o ícone visualmente, pois a mensagem já foi mostrada
            else 
            {
                 buffIcon.gameObject.SetActive(true);
            }
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
        else
        {
            Debug.LogError("CanvasGroup não foi atribuído no Inspector!");
        }

        if (furyText != null)
        {
            furyText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("O componente de texto (TextMeshProUGUI) não foi atribuído no Inspector!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerItems.XpHack();
        }

        // Esta condição agora só será verdadeira UMA VEZ durante todo o jogo
        if (!messageShown && XpManager.XPdoPlayer >= xpTotalParaAtivar)
        {
            messageShown = true; // Marca como mostrada IMEDIATAMENTE
            buffAtivado = true;  // Marca o buff como ativo
            Buff(buffIcon);
            ShowFuryMessage();
        }
    }

    private void ShowFuryMessage()
    {
        furyText.gameObject.SetActive(true);
        int idiomaSalvo = PlayerPrefs.GetInt("IdiomaSelecionado", 0);
        DialogueControl.idiom idioma = (DialogueControl.idiom)idiomaSalvo;

        switch (idioma)
        {
            case DialogueControl.idiom.pt:
                furyText.text = PtText;
                break;
            case DialogueControl.idiom.eng:
                furyText.text = EngText;
                break;
            case DialogueControl.idiom.spa:
                furyText.text = SpaText;
                break;
        }
        StartCoroutine(FadeMessage());
    }

    private IEnumerator FadeMessage()
    {
        float timer = 0f;
        while (timer < fadeInTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeInTime);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(displayTime);

        timer = 0f;
        while (timer < fadeOutTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeOutTime);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
        furyText.gameObject.SetActive(false);
    }

    private void Buff(UnityEngine.UI.Image buffIcon)
    {
        buffIcon.gameObject.SetActive(true);
        AttackManager.DanoPlayer = 2f;
    }

    public static void ResetFuryState()
    {
        messageShown = false;
        buffAtivado = false;
        AttackManager.DanoPlayer = 1f;
    }
}