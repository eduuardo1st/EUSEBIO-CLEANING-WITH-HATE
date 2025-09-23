using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarCena : MonoBehaviour
{
    [SerializeField] private string nomeDaCenaDoMenuPrincipal;
    
    public void MainMenu()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeDaCenaDoMenuPrincipal);
    }
}
