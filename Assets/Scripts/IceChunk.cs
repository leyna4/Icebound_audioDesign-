using UnityEngine;

public class IceChunk : MonoBehaviour
{
    [Header("Enemy")]
    public bool hasEnemy;
    public GameObject enemyPrefab;

    [Header("Floating Movement")]
    public float floatSpeed = 0.6f;
    public float swayAmount = 0.3f;
    public float swaySpeed = 1.2f;

    [Header("Main Ice Reference")]
    public Transform mainIcePlatform;
    public float mainIceHalfWidth = 1.5f; // orta alan güvenliði

    private Rigidbody2D rb;
    private float startX;
    private bool isAttached = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        if (isAttached) return;

        // Yukarý doðru süzülme
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        // Sað-sol salýným (ORTAYA GÝRMEZ)
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        float newX = startX + sway;

        if (mainIcePlatform != null)
        {
            float minX = mainIcePlatform.position.x - mainIceHalfWidth;
            float maxX = mainIcePlatform.position.x + mainIceHalfWidth;

            // Orta alana girmesin
            if (newX > minX && newX < maxX)
            {
                newX = startX;
            }
        }

        transform.position = new Vector3(newX, transform.position.y, 0f);
    }

    void LateUpdate()
    {
        // Ekran dýþýna çýktýysa sil
        if (Camera.main == null) return;

        float camTop = Camera.main.orthographicSize + 2f;
        if (transform.position.y > camTop)
        {
            Destroy(gameObject);
        }
    }

    //  Hook buza saplandýðýnda
    public void AttachToHook(Transform hook)
    {
        isAttached = true;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; //  daha güvenli
        }

        transform.SetParent(hook);
    }

    //  Platforma varýnca
    public void AttachToPlatform(Transform platform)
    {
        isAttached = true;

        transform.SetParent(platform);
        transform.localPosition = Vector3.zero;

        if (hasEnemy && enemyPrefab != null)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }

        //  Ana buz alaný geniþlesin
        platform.localScale += new Vector3(0.15f, 0.15f, 0f);
    }
}
