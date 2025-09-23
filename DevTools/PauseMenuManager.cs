using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    // AQUI EST� A DEFINI��O DA PROPRIEDADE SINGLETON 'instance'
    public static PauseMenuManager instance;

    [Header("UI do Menu de Pause")]
    public GameObject pausePanel;
    [SerializeField] private Slider volumeSlider;

    [Header("Nomes de Cenas")]
    [SerializeField] private string nomeDaCenaDoMenuPrincipal;

    [Header("UI do Menu de Controls")]
    public GameObject controlsPanel;

    private bool isPaused = false;


    void Awake()
    {
        
        if (instance == null) 
        {
            instance = this; 
        }
        else if (instance != this) 
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            float volumeSalvo = PlayerPrefs.GetFloat("Volume", 1f);
            volumeSlider.SetValueWithoutNotify(volumeSalvo); // evita disparar evento
            AudioListener.volume = volumeSalvo;
        }

        volumeSlider.onValueChanged.AddListener(AlterarVolume);

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true;

        if (pausePanel.activeSelf)
        {
            Button firstButton = pausePanel.GetComponentInChildren<Button>();
            if (firstButton != null)
            {
                EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
            }
        }
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;

        // Limpa a sele��o do EventSystem para que nenhum bot�o fique "selecionado"
        EventSystem.current.SetSelectedGameObject(null);

        Input.ResetInputAxes();

        StartCoroutine(ReativarInputNoProximoFrame());
    }

    private IEnumerator ReativarInputNoProximoFrame()
    {
        // Bloqueia input do jogador neste frame
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.SetPodeReceberInput(false);
        }

        yield return null; // espera 1 frame

        if (player != null)
        {
            player.SetPodeReceberInput(true);
        }
    }

    public void GoToMainMenu()
    {
        
        AttackManager.DestruirInstanciaEResetarAtaque();
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeDaCenaDoMenuPrincipal);
    }

    public void OpenControlsPanel()
    {
        controlsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void CloseControlsPanel()
    {
        controlsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do Jogo via Menu de Pause!");
        Application.Quit();
    }

    public bool IsGamePaused
    {
        get { return isPaused; }
    }
    
    public void AlterarVolume(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("Volume", valor);
        PlayerPrefs.Save();
    }
}