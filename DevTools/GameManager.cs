using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Vida do Player")]
    public PlayerController player;

    void Awake()
    {
        player.ResetarVida();
    }
}
