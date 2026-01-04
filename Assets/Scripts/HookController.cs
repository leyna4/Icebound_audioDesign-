using UnityEngine;
using System.Collections;

public class HookController : MonoBehaviour
{
    public Transform hookOrigin;
    public LineRenderer lineRenderer;
    public LayerMask iceLayer;
    public Transform mainIcePlatform;
    public Camera mainCamera;

    public float maxHookDistance = 20f;
    public float hookSpeed = 15f;
    public float pullSpeed = 10f;
    public float attachDistance = 0.4f;

    private bool isBusy;

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

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector3 origin = hookOrigin.position;
        origin.z = 0;

        Vector3 dir = (mouseWorld - origin).normalized;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxHookDistance, iceLayer);

        if (hit.collider != null)
        {
            IceChunk chunk = hit.collider.GetComponent<IceChunk>();
            if (chunk != null)
            {
                Vector3 hitPoint = hit.point;
                hitPoint.z = 0;

                yield return ExtendHook(hitPoint);

                chunk.AttachToHook(hookOrigin);

                yield return PullToPlatform(chunk);
            }
        }
        else
        {
            Vector3 missPoint = origin + dir * maxHookDistance;
            yield return ExtendHook(missPoint);
            yield return RetractHook(missPoint);
        }

        lineRenderer.enabled = false;
        isBusy = false;
    }

    IEnumerator ExtendHook(Vector3 target)
    {
        lineRenderer.enabled = true;

        float t = 0;
        float dist = Vector3.Distance(hookOrigin.position, target);

        while (t < 1)
        {
            t += Time.deltaTime * hookSpeed / dist;
            Vector3 pos = Vector3.Lerp(hookOrigin.position, target, t);

            lineRenderer.SetPosition(0, hookOrigin.position);
            lineRenderer.SetPosition(1, pos);

            yield return null;
        }
    }

    IEnumerator PullToPlatform(IceChunk chunk)
    {
        while (chunk != null &&
               Vector3.Distance(chunk.transform.position, mainIcePlatform.position) > attachDistance)
        {
            chunk.transform.position = Vector3.MoveTowards(
                chunk.transform.position,
                mainIcePlatform.position,
                pullSpeed * Time.deltaTime
            );

            lineRenderer.SetPosition(0, hookOrigin.position);
            lineRenderer.SetPosition(1, chunk.transform.position);

            yield return null;
        }

        if (chunk != null)
        {
            chunk.AttachToPlatform(mainIcePlatform);
            CameraZoomOut();
        }
    }

    IEnumerator RetractHook(Vector3 start)
    {
        float t = 0;
        float dist = Vector3.Distance(start, hookOrigin.position);

        while (t < 1)
        {
            t += Time.deltaTime * hookSpeed / dist;
            Vector3 pos = Vector3.Lerp(start, hookOrigin.position, t);

            lineRenderer.SetPosition(0, hookOrigin.position);
            lineRenderer.SetPosition(1, pos);

            yield return null;
        }
    }

    void CameraZoomOut()
    {
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize += 0.25f;
        }
    }
}
