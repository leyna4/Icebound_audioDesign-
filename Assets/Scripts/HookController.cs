using UnityEngine;

public class HookController : MonoBehaviour
{
    [Header("References")]
    public Transform hookOrigin;              // IcePlatform merkezi
    public LineRenderer lineRenderer;
    public Transform mainIcePlatform;
    public Camera mainCamera;

    [Header("Hook Settings")]
    public float shootSpeed = 15f;
    public float returnSpeed = 10f;
    public float maxHookDistance = 5f;

    private bool isShooting;
    private bool isReturning;

    private Vector3 targetPosition;
    private IceChunk caughtChunk;

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;

        // Hook baþlangýçta platformda dursun
        transform.position = hookOrigin.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isShooting && !isReturning)
        {
            LaunchHook();
        }

        HandleMovement();
        DrawLine();
    }

    void LaunchHook()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 dir = (mouseWorld - hookOrigin.position).normalized;
        targetPosition = hookOrigin.position + dir * maxHookDistance;

        isShooting = true;
    }

    void HandleMovement()
    {
        if (isShooting)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                shootSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isShooting = false;
                isReturning = true;
            }
        }
        else if (isReturning)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                hookOrigin.position,
                returnSpeed * Time.deltaTime
            );

            // Buzu beraber çek
            if (caughtChunk != null)
            {
                caughtChunk.transform.position = transform.position;
            }

            if (Vector3.Distance(transform.position, hookOrigin.position) < 0.05f)
            {
                FinishRetrieval();
            }
        }
    }

    void DrawLine()
    {
        lineRenderer.SetPosition(0, hookOrigin.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isShooting) return;

        if (collision.CompareTag("IceChunk"))
        {
            IceChunk chunk = collision.GetComponent<IceChunk>();
            if (chunk == null) return;

            isShooting = false;
            isReturning = true;

            caughtChunk = chunk;

            Rigidbody2D rb = chunk.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = false;
            }
        }
    }

    void FinishRetrieval()
    {
        isReturning = false;

        if (caughtChunk != null)
        {
            caughtChunk.AttachToPlatform(mainIcePlatform);

            // Kamera geri çekilsin
            CameraZoomOut();

            caughtChunk = null;
        }

        // Hook tekrar platforma sabitlensin
        transform.position = hookOrigin.position;
    }

    void CameraZoomOut()
    {
        if (mainCamera != null && mainCamera.orthographic)
        {
            mainCamera.orthographicSize += 0.25f;
        }
    }
}
