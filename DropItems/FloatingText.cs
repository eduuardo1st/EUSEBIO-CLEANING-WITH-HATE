using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FloatingText : MonoBehaviour
{
    public float floatSpeed = 50f; 
    public float duration = 1.5f;  
    private Text text;
    private float timer;
    private Color startColor;

    void Awake()
    {
        
        text = GetComponent<Text>();
        startColor = text.color;
    }

    void OnEnable()
    {
        timer = 0f;
        text.color = startColor; 
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        float progress = timer / duration;

        Color newColor = startColor;
        
        newColor.a = Mathf.Lerp(1f, 0f, progress);
        
        text.color = newColor;

        if (timer >= duration)
        {
            gameObject.SetActive(false);
        }
    }
}