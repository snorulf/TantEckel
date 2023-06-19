using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProceduralAnimator : MonoBehaviour
{
    //[SerializeField] private Transform[] limbTargets;

    [SerializeField] private float stepSize = 1;

    [SerializeField] private LayerMask groundLayerMask = default;
    [SerializeField] private float raycastRange = 2;

    private int _nLimbs;
    [SerializeField] private ProceduralLimb[] _limbs;

    private Vector3 _lastBodyPosition;
    private Vector3 _velocity;

    private bool _allLimbsResting;

    [SerializeField] private float _feetOffset = 0;

    void OnDrawGizmosSelected()
    {
        Vector3[] desiredPositions = new Vector3[_nLimbs];
        for (int i = 0; i < _nLimbs; ++i)
        {
            if (_limbs[i].moving) continue; // limb already moving: can't move again!

            desiredPositions[i] = transform.TransformPoint(_limbs[i].defaultPosition);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(desiredPositions[i], 0.25f);
        }
    }

    void Start()
    {
        var limbTargets = GetComponentsInChildren<IKTarget>();
        _nLimbs = limbTargets.Length; ;

        _limbs = new ProceduralLimb[_nLimbs];
        Transform t;
        for (int i = 0; i < _nLimbs; ++i)
        {
            t = limbTargets[i].transform;
            _limbs[i] = new ProceduralLimb()
            {
                IKTarget = t,
                defaultPosition = t.localPosition,
                lastPosition = t.position,
                moving = false
            };
        }
    }

    void FixedUpdate()
    {
        _velocity = transform.position - _lastBodyPosition;

        if (_velocity.magnitude > Mathf.Epsilon)
            _HandleMovement();
        else
            _BackToRestPosition();

    }

    private void _HandleMovement()
    {
        _lastBodyPosition = transform.position;

        Vector3[] desiredPositions = new Vector3[_nLimbs];
        float greatestDistance = stepSize;
        int limbToMove = -1;

        for (int i = 0; i < _nLimbs; ++i)
        {
            if (_limbs[i].moving) continue; // limb already moving: can't move again!

            desiredPositions[i] = transform.TransformPoint(_limbs[i].defaultPosition);
            float dist = (desiredPositions[i] + _velocity - _limbs[i].lastPosition).magnitude;
            if (dist > greatestDistance)
            {
                greatestDistance = dist;
                limbToMove = i;
            }
        }

        // keep non moving limbs in place
        for (int i = 0; i < _nLimbs; ++i)
            if (i != limbToMove)
                _limbs[i].IKTarget.position = _limbs[i].lastPosition;

        // move the selected leg to its "desired" position
        if (limbToMove != -1)
        {
            Vector3 targetOffset = desiredPositions[limbToMove] - _limbs[limbToMove].IKTarget.position;
            Vector3 targetPoint = desiredPositions[limbToMove] + _velocity.magnitude * targetOffset;
            targetPoint = _RaycastToGround(targetPoint, transform.up);
            targetPoint += transform.up * _feetOffset;
            _limbs[limbToMove].IKTarget.position = targetPoint;
            _limbs[limbToMove].lastPosition = targetPoint;
            _allLimbsResting = false;
        }
    }

    private void _BackToRestPosition()
    {
        Vector3 targetPoint; float dist;
        for (int i = 0; i < _nLimbs; ++i)
        {
            targetPoint = _RaycastToGround(
                transform.TransformPoint(_limbs[i].defaultPosition),
                transform.up) + transform.up * _feetOffset;
            dist = (targetPoint - _limbs[i].lastPosition).magnitude;
            if (dist > 0.005f)
            {
                _limbs[i].IKTarget.position = targetPoint;
                _limbs[i].lastPosition = targetPoint;
                return;
            }
        }
        _allLimbsResting = true;
    }

    private Vector3 _RaycastToGround(Vector3 pos, Vector3 up)
    {
        Vector3 point = pos;

        Ray ray = new Ray(pos + raycastRange * up, -up);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f * raycastRange, groundLayerMask))
            point = hit.point;
        return point;
    }
}
