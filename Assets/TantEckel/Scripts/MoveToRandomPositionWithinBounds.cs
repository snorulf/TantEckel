using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRandomPositionWithinBounds : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float coefficient = 1f;

    void Update()
    {
        transform.localPosition = transform.localPosition + new Vector3(Mathf.Sin(speed * Time.time) * coefficient, Mathf.Cos(speed * Time.time) * coefficient, Mathf.Sin(speed * Time.time) * coefficient);
    }
}
