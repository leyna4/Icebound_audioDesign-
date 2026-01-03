using UnityEngine;
using System.Collections;

public class HookController : MonoBehaviour
{
    public Transform hookOrigin;
    public LineRenderer lineRenderer;
    public LayerMask iceLayer;
    public Transform mainIcePlatform;

    public float maxHookDistance = 10f;
    public float hookSpeed = 12f;

    bool isHookBusy = false;

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isHookBusy)
        {
            StartCoroutine(HookRoutine());
        }
    }

    IEnumerator HookRoutine()
    {
        isHookBusy = true;

        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 dir =
            (mouseWorld - hookOrigin.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            hookOrigin.position,
            dir,
            maxHookDistance,
            iceLayer
        );

        Vector3 targetPoint =
            hit.collider != null
            ? (Vector3)hit.point
            : hookOrigin.position + dir * maxHookDistance;

        IceChunk hitChunk = null;
        if (hit.collider != null)
            hitChunk = hit.collider.GetComponent<IceChunk>();

        //  ATILMA
        yield return StartCoroutine(ExtendHook(targetPoint));

        // BUZA SAPLANDIYSA
        if (hitChunk != null)
        {
            yield return new WaitForSeconds(0.15f);
        }

        //  GERÝ ÇEK
        yield return StartCoroutine(RetractHook(targetPoint, hitChunk));

        lineRenderer.enabled = false;
        isHookBusy = false;
    }

    IEnumerator ExtendHook(Vector3 target)
    {
        lineRenderer.enabled = true;

        float t = 0f;
        float dist =
            Vector3.Distance(hookOrigin.position, target);

        while (t < 1f)
        {
            t += Time.deltaTime * hookSpeed / dist;
            Vector3 pos =
                Vector3.Lerp(hookOrigin.position, target, t);

            lineRenderer.SetPosition(0, hookOrigin.position);
            lineRenderer.SetPosition(1, pos);

            yield return null;
        }

        lineRenderer.SetPosition(1, target);
    }

    IEnumerator RetractHook(Vector3 target, IceChunk chunk)
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * hookSpeed;
            Vector3 pos =
                Vector3.Lerp(hookOrigin.position, target, t);

            lineRenderer.SetPosition(0, hookOrigin.position);
            lineRenderer.SetPosition(1, pos);

            // buz varsa, kancayla birlikte gelsin
            if (chunk != null)
            {
                chunk.transform.position = pos;
            }

            yield return null;
        }

        if (chunk != null)
        {
            chunk.AttachToPlatform(mainIcePlatform);
        }
    }
}
