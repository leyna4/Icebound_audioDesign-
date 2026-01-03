using UnityEngine;
using System.Collections;

public class HookController : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform hookOrigin;
    public LineRenderer lineRenderer;
    public LayerMask iceLayer; // Inspector'dan "Ice" seçili olduðundan emin ol
    public Transform mainIcePlatform;

    [Header("Ayarlar")]
    public float maxHookDistance = 20f; // Mesafeyi biraz artýrdýk
    public float hookSpeed = 15f;
    public float pullSpeed = 10f;
    public float attachDistance = 0.5f;

    private bool isBusy = false;

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isBusy)
        {
            StartCoroutine(HookRoutine());
        }
    }

    IEnumerator HookRoutine()
    {
        isBusy = true;

        // 1. Mouse Pozisyonu ve Yön Hesaplama
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f; // Z eksenini sýfýrlýyoruz

        // Köken noktasýnýn Z eksenini de kontrol edelim (ÖNEMLÝ)
        Vector3 originPos = hookOrigin.position;
        originPos.z = 0f;

        Vector3 dir = (mouseWorld - originPos).normalized;

        // 2. Raycast Atýþý
        // Görselleþtirme: Sahne ekranýnda kýrmýzý bir çizgi çizer
        Debug.DrawRay(originPos, dir * maxHookDistance, Color.red, 2f);

        RaycastHit2D hit = Physics2D.Raycast(originPos, dir, maxHookDistance, iceLayer);

        if (hit.collider != null)
        {
            Debug.Log("Buz yakalandý: " + hit.collider.name);
            IceChunk chunk = hit.collider.GetComponent<IceChunk>();

            if (chunk != null)
            {
                // Hedef noktayý buzun merkezi yapalým (daha güvenli)
                Vector3 targetPoint = (Vector3)hit.point;
                targetPoint.z = 0f;

                yield return StartCoroutine(ExtendHook(targetPoint));

                // Buzun fiziðini kapat (Kendi hareket etmesin)
                Rigidbody2D rb = chunk.GetComponent<Rigidbody2D>();
                if (rb != null) { rb.isKinematic = true; rb.velocity = Vector2.zero; }

                chunk.StickToHook(targetPoint);
                yield return StartCoroutine(PullChunkToPlatform(chunk));
            }
        }
        else
        {
            Debug.Log("Kanca hiçbir þeye çarpmadý.");
            Vector3 targetPoint = originPos + dir * maxHookDistance;
            yield return StartCoroutine(ExtendHook(targetPoint));
            yield return StartCoroutine(RetractEmptyHook(targetPoint));
        }

        lineRenderer.enabled = false;
        isBusy = false;
    }

    IEnumerator ExtendHook(Vector3 target)
    {
        lineRenderer.enabled = true;
        float t = 0f;
        float dist = Vector3.Distance(hookOrigin.position, target);
        while (t < 1f)
        {
            t += Time.deltaTime * hookSpeed / dist;
            lineRenderer.SetPosition(0, hookOrigin.position);
            lineRenderer.SetPosition(1, Vector3.Lerp(hookOrigin.position, target, t));
            yield return null;
        }
    }

    IEnumerator PullChunkToPlatform(IceChunk chunk)
    {
        while (chunk != null && Vector3.Distance(chunk.transform.position, mainIcePlatform.position) > attachDistance)
        {
            // Buzu ve çizgiyi hareket ettir
            chunk.transform.position = Vector3.MoveTowards(chunk.transform.position, mainIcePlatform.position, pullSpeed * Time.deltaTime);

            lineRenderer.SetPosition(0, hookOrigin.position);
            lineRenderer.SetPosition(1, chunk.transform.position);
            yield return null;
        }

        if (chunk != null) chunk.AttachToPlatform(mainIcePlatform);
    }

    IEnumerator RetractEmptyHook(Vector3 start)
    {
        float t = 0f;
        float dist = Vector3.Distance(start, hookOrigin.position);
        while (t < 1f)
        {
            t += Time.deltaTime * hookSpeed / dist;
            lineRenderer.SetPosition(0, hookOrigin.position);
            lineRenderer.SetPosition(1, Vector3.Lerp(start, hookOrigin.position, t));
            yield return null;
        }
    }
}