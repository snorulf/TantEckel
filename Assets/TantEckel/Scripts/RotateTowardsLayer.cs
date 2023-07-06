using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsLayer : MonoBehaviour
{
    [SerializeField] LayerMask targetLayerMask;

    [SerializeField] float rotationSpeed = 1f;

    [SerializeField] Transform surfaceScanRayCast;
    [SerializeField] float rayLength = 10f;

    void Update()
    {
        var rayDirection = surfaceScanRayCast.transform.forward * rayLength;

        Debug.DrawRay(surfaceScanRayCast.transform.position, rayDirection, Color.blue, 0.1f);

        Ray ray = new Ray(surfaceScanRayCast.transform.position, rayDirection);
        if (Physics.SphereCast(ray, 0.1f, out RaycastHit hit, rayLength, targetLayerMask))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.green, 0.1f);

            var targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.localRotation;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * (rotationSpeed/10f));
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);

            // rotate down a bit
            transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
        }

    }
}
