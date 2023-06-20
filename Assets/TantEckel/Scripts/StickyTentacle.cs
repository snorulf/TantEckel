using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class StickyTentacle : MonoBehaviour
{
    enum State { Idle, Seek, Moving, Attached }
    [SerializeField] State state = State.Idle;

    [SerializeField] float maxDistance = 2f;
    [SerializeField] public Transform IKTarget;
    [SerializeField] public Transform idlePosition;
    [SerializeField] public Transform rootBone;
    [SerializeField] public Transform tipBone;
    [SerializeField] Transform rayCastTransform;

    [SerializeField] LayerMask attachLayerMaskName;
    [SerializeField] float tentacleSpeed = 1f;

    public Vector3 targetPosition;

    [SerializeField] TantEckel tantEckel;

    [SerializeField] public float maxStretchDistance = 2.0f;
    [SerializeField] public float maxStretchRandomness = 0.2f;
    [SerializeField] public float attachDistance = 0.1f;
    [SerializeField] public float attachDistanceRandomness = 0.05f;
    
    [SerializeField] public float rayStartVelocityModifier = 20f;

    [SerializeField] float idleTimer = 1f;
    private float idleTime = 0f;

    [SerializeField] float moveTimer = 1f;
    private float moveTime = 0f;

    [SerializeField] float attachedTimer = 2f;
    private float attachedTime = 0f;

    private Vector3 rayStart;

    void Start()
    {
        targetPosition = idlePosition.position;

        maxStretchDistance = Random.Range(maxStretchDistance - maxStretchRandomness, maxStretchDistance + maxStretchRandomness);
        attachDistance = Random.Range(attachDistance - attachDistanceRandomness, attachDistance + attachDistanceRandomness);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition, 0.25f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rayStart, 0.25f);
    }

    void Update()
    {
        if (state == State.Idle)
        {
            Idle();
        }
        else if (state == State.Seek)
        {
            Seek();
        }
        else if (state == State.Moving)
        {
            MoveToTarget();
        }
        else if (state == State.Attached)
        {
            Attached();
        }
    }
    private void Seek()
    {
        rayStart = rayCastTransform.transform.position + (tantEckel.velocity * rayStartVelocityModifier);
        var rayDirection = -rayCastTransform.transform.forward * maxDistance;

        Debug.DrawRay(rayStart, rayDirection, Color.green, 0.1f);

        Ray ray = new Ray(rayStart, rayDirection);
        if (Physics.SphereCast(ray, 2f, out RaycastHit hit, maxDistance, attachLayerMaskName))
        {
            state = State.Moving;
            targetPosition = hit.point;
        }
        else
        {
            state = State.Idle;
        }
    }

    private void Attached()
    {
        IKTarget.transform.position = targetPosition;

        attachedTime += Time.deltaTime;

        if (moveTime >= moveTimer)
        {
            attachedTime = 0f;
            state = State.Seek;
        }
        else if (CheckOverStretched())
        {
            attachedTime = 0f;
            state = State.Idle;
        }
    }

    private bool CheckAttached()
    {
        var targetDistance = Vector3.Distance(targetPosition, tipBone.transform.position);

        bool reachable = targetDistance < attachDistance;

#if false
        if (reachable)
        {
            Debug.Log("<color=green>" + targetDistance + "</color>");
            Debug.DrawLine(targetPosition, tipBone.transform.position, Color.green, 0.1f);
        }
        else
        {
            Debug.Log("<color=blue>" + targetDistance + "</color>");
            Debug.DrawLine(targetPosition, tipBone.transform.position, Color.blue, 0.1f);
        }
#endif

        return reachable;
    }

    private bool CheckOverStretched()
    {
        var rayDirection = -rayCastTransform.transform.forward;
        if (Physics.Raycast(rayCastTransform.transform.position, rayDirection, out RaycastHit hit, maxDistance, attachLayerMaskName))
        {
            Debug.DrawLine(targetPosition, hit.point, Color.red, 0.1f);

            return Vector3.Distance(targetPosition, hit.point) > maxStretchDistance;
        }
        return true;
    }
    
    private void MoveToTarget()
    {
        IKTarget.transform.position = Vector3.Slerp(IKTarget.transform.position, targetPosition, tentacleSpeed * Time.deltaTime);

        moveTime += Time.deltaTime;

        if (moveTime >= moveTimer)
        {
            if (CheckOverStretched())
            {
#if false
                Debug.DrawLine(targetPosition, rootBone.transform.position, Color.red, 0.1f);
#endif
                moveTime = 0f;
                state = State.Idle;
                return;
            }
        }
        else if (CheckAttached())
        {
            state = State.Attached;
        }
    }

    private void Idle()
    {
        idleTime += Time.deltaTime;

        if (idleTime >= idleTimer)
        {
            idleTime = 0f;
            state = State.Seek;
        }
        else
        {
            IKTarget.transform.position = Vector3.Slerp(IKTarget.transform.position, idlePosition.position, tentacleSpeed * Time.deltaTime);
        }
    }
}


