using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateTowardsLayer : MonoBehaviour
{
    [SerializeField] LayerMask targetLayerMask;

    [SerializeField] float rotationSpeed = 1f;

    [SerializeField] float maxDistance = 2f;

    [SerializeField] Transform rayCastTransform;

    private Vector3 hitPosition = Vector3.zero;


    void OnDrawGizmosSelected()
    {
        if (hitPosition != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitPosition, 0.25f);
        }
    }

    void Update()
    {
        var rayDirection = rayCastTransform.transform.forward * maxDistance;

        Debug.DrawRay(rayCastTransform.transform.position, rayDirection, Color.blue, 0.1f);

        hitPosition = Vector3.zero;

        Ray ray = new Ray(rayCastTransform.transform.position, rayDirection);
        if (Physics.SphereCast(ray, 0.1f, out RaycastHit hit, maxDistance, targetLayerMask))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.green, 0.1f);

            var targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, hit.normal), rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.eulerAngles.y, targetRotation.eulerAngles.z);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);

            var targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.eulerAngles.y, targetRotation.eulerAngles.z);
        }

    }
}
