using UnityEngine;

public class DropItens : MonoBehaviour
{
    [SerializeField] private float speed = -5f;
    [SerializeField] private float timeMove = 0.3f;
    private float timeCount;

    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private float moveToPlayerSpeed = 2f;

    private bool isMoving = true;
    private Transform playerTransform;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        if (isMoving)
        {
            if (timeCount < timeMove)
            {
                Vector2 direction = Vector2.right * speed * Time.deltaTime;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude);
                if (hit.collider == null)
                {
                    transform.Translate(direction);
                }
                else
                {
                    isMoving = false;
                }
            }
            else
            {
                isMoving = false;
            }
        }
        else
        {
            if (playerTransform != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer <= detectionRadius)
                {
                    transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveToPlayerSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Adiciona XP no XpManager
            XpManager.XPdoPlayer++;
            Destroy(gameObject);
        }
    }
}
