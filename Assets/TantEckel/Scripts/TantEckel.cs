using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TantEckel : MonoBehaviour
{
    private StickyTentacle[] tentacles;

    private Vector3 lastBodyPosition;
    public Vector3 velocity;

    [SerializeField] public float stepSize = 1;

    void Awake()
    {
        tentacles = GetComponentsInChildren<StickyTentacle>();
    }

    void FixedUpdate()
    {
        velocity = transform.position - lastBodyPosition;
        lastBodyPosition = transform.position;
    }
}
