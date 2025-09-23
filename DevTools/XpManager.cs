using UnityEngine;

public class XpManager : MonoBehaviour
{
    public static XpManager instance;
    public static int XPdoPlayer = 0;
    private const string CheckpointXpKey = "CheckpointXP";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            LoadXpFromCheckpoint();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SaveXpAsCheckpoint()
    {
        PlayerPrefs.SetInt(CheckpointXpKey, XPdoPlayer);
        PlayerPrefs.Save(); 
        Debug.Log($"XP salvo no checkpoint: {XPdoPlayer}");
    }

    public static void LoadXpFromCheckpoint()
    {

        XPdoPlayer = PlayerPrefs.GetInt(CheckpointXpKey, 0);
        Debug.Log($"XP carregado do checkpoint: {XPdoPlayer}");
    }

    public static void ResetXpToCheckpoint()
    {
        LoadXpFromCheckpoint(); 
        Debug.Log($"XP resetado para o checkpoint: {XPdoPlayer}");
    }

    public static void ResetTotalXpProgress()
    {
        PlayerPrefs.DeleteKey(CheckpointXpKey);
        XPdoPlayer = 0;
        Debug.Log("Progresso total de XP foi completamente zerado.");
    }
}
