using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager instance;
    public static float DanoPlayer = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetAtaque()
    {
        DanoPlayer = 1;
    }

    public static void ResetAtaqueEstatico()
    {
        DanoPlayer = 1;
    }

    public static void DestruirInstanciaEResetarAtaque()
    {
        
        DanoPlayer = 1;
        Debug.Log("XP estático resetado para 0.");

        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
            Debug.Log("Instância do AttackManager destruída.");
        }
    }
}
