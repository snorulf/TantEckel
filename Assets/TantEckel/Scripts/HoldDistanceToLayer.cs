using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDistanceToLayer : MonoBehaviour
{
    [SerializeField] LayerMask targetLayerMask;

    [SerializeField] float minDistanceToLayer = 1f;
    [SerializeField] float maxDistanceToLayer = 10f;
    [SerializeField] float adjustSpeed = 1f;

    [SerializeField] Transform surfaceScanRayCast;
    [SerializeField] float rayLength = 20f;

    void Update()
    {
        var rayDirection = surfaceScanRayCast.transform.forward * rayLength;

        Debug.DrawRay(surfaceScanRayCast.transform.position, rayDirection, Color.cyan, 0.1f);

        Ray ray = new Ray(surfaceScanRayCast.transform.position, rayDirection);
        if (Physics.SphereCast(ray, 0.1f, out RaycastHit hit, rayLength, targetLayerMask))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.green, 0.1f);

            var distance = Vector3.Distance(surfaceScanRayCast.position, hit.point);

            if (distance < minDistanceToLayer)
            {
                transform.localPosition += transform.up * adjustSpeed * Time.deltaTime;
            }
            else if (distance > maxDistanceToLayer)
            {
                transform.localPosition -= transform.up * adjustSpeed * Time.deltaTime;
            }
        }
    }
}
