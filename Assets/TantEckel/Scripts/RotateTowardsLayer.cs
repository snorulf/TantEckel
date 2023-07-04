using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class RotateTowardsLayer : MonoBehaviour
{
    [SerializeField] LayerMask targetLayerMask;

    [SerializeField] float rotationSpeed = 1f;

    [SerializeField] float maxDistance = 2f;

    [SerializeField] Transform rayCastTransform;

    [SerializeField] Transform seekTransform;

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
            hitPosition = hit.point;
            Debug.DrawRay(hit.point, hit.normal, Color.green, 0.1f);

            // lerp transform up to hit normal
            var targetRotation = Quaternion.LookRotation(hit.normal, Vector3.up);

            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // rotate towards seekTransform
            var targetRotation = Quaternion.LookRotation(seekTransform.transform.position - transform.position, Vector3.up);

            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
    }
}
