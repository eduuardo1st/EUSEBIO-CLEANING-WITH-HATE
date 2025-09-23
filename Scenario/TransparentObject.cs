using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class TransparentObject : MonoBehaviour
{
    [Range(0, 1)] 
    [SerializeField] private float targetAlpha = 0.4f;

    [SerializeField] private float fadeDuration = 0.3f;

    private TilemapRenderer tilemapRenderer;
    private Material tilemapMaterial;
    private Coroutine currentFade;

    private void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapMaterial = tilemapRenderer.material;

        tilemapRenderer.material = new Material(tilemapMaterial);
        tilemapMaterial = tilemapRenderer.material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            StartFade(tilemapMaterial.color.a, targetAlpha);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            StartFade(tilemapMaterial.color.a, 1f);
        }
    }

    private void StartFade(float fromAlpha, float toAlpha)
    {
        if (!gameObject.activeInHierarchy) return;

        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeAlpha(fromAlpha, toAlpha));
    }

    private IEnumerator FadeAlpha(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            Color color = tilemapMaterial.color;
            color.a = alpha;
            tilemapMaterial.color = color;
            yield return null;
        }

        Color finalColor = tilemapMaterial.color;
        finalColor.a = endAlpha;
        tilemapMaterial.color = finalColor;
    }
}
