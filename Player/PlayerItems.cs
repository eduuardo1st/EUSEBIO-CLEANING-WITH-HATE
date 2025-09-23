using UnityEngine;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] private Text xpText;
    [SerializeField] private Image XpBar;
    [SerializeField] private int XpTotal = 72;

    private int currentXPdoPlayer;

    void Start()
    {
        currentXPdoPlayer = XpManager.XPdoPlayer;

        xpText.gameObject.SetActive(false);

        UpdateXpBar();
    }

    void Update()
    {
        if (currentXPdoPlayer < XpManager.XPdoPlayer)
        {
            int ganhoXP = XpManager.XPdoPlayer - currentXPdoPlayer;

            xpText.text = $"+{ganhoXP} XP";
            xpText.gameObject.SetActive(true);

            currentXPdoPlayer = XpManager.XPdoPlayer;
        }

        UpdateXpBar();
    }

    private void UpdateXpBar()
    {
        if (XpBar != null)
        {
            XpBar.fillAmount = (float)XpManager.XPdoPlayer / (float)XpTotal;
        }
    }

    public void XpHack()
    {
        XpManager.XPdoPlayer = XpTotal;
    }
}
