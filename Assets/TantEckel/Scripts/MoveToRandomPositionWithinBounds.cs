using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRandomPositionWithinBounds : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float coefficient = 1f;

    void Start()
    {
        coefficient = Random.Range(coefficient * -0.5f, coefficient * 0.5f);
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(Mathf.Sin(Time.time) * coefficient, Mathf.Cos(Time.time) * coefficient, Mathf.Sin(Time.time) * coefficient);
    }
}
