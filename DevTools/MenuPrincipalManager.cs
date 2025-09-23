using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelDeJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelControles;
    [SerializeField] private Slider volumeSlider;
    public Dropdown dropdownIdioma;
    [SerializeField] private string nomeDaCenaDoMenuPrincipal;

    private void Start()
    {
        BossDialogue_ResetFlag();

        if (PlayerPrefs.HasKey("Volume"))
        {
            float volumeSalvo = PlayerPrefs.GetFloat("Volume", 1f);
            volumeSlider.SetValueWithoutNotify(volumeSalvo);
            AudioListener.volume = volumeSalvo;
        }

        volumeSlider.onValueChanged.AddListener(AlterarVolume);

        int idiomaSalvo = PlayerPrefs.GetInt("IdiomaSelecionado", 0);
        dropdownIdioma.value = idiomaSalvo;
        DialogueControl.selectedLanguageIndex = idiomaSalvo;
        Debug.Log("MenuPrincipalManager Start: Idioma inicial definido (static): " + DialogueControl.selectedLanguageIndex);
        dropdownIdioma.onValueChanged.AddListener(AtualizarIdioma);
    }

    public void Jogar()
    {
        Debug.Log("Resetando XP pelo XpManager");
        XpManager.ResetTotalXpProgress(); 
        EusebioFuryManager.ResetFuryState();
        SceneManager.LoadScene(nomeDoLevelDeJogo);
    }

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    public void AbrirControles()
    {
        painelOpcoes.SetActive(false);
        painelControles.SetActive(true);
    }

    public void FecharControles()
    {
        painelControles.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void SairDoJogo()
    {
        Debug.Log("Sair do jogo");
        Application.Quit();
    }

    public void AtualizarIdioma(int index)
    {
        Debug.Log("AtualizarIdioma chamado com index: " + index);
        DialogueControl.selectedLanguageIndex = index;
        Debug.Log("MenuPrincipalManager AtualizarIdioma: Idioma est�tico definido para: " + DialogueControl.selectedLanguageIndex);
        PlayerPrefs.SetInt("IdiomaSelecionado", index);
        PlayerPrefs.Save();
    }

    public void AlterarVolume(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("Volume", valor);
        PlayerPrefs.Save();
    }

    private void BossDialogue_ResetFlag()
    {
        System.Type bossType = typeof(BossDialogue);
        var field = bossType.GetField("bossDialogueShown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        if (field != null)
        {
            field.SetValue(null, false);
        }
    }
}
