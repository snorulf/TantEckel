using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class ProceduralLimb
{
    public Transform IKTarget;
    public Vector3 defaultPosition;
    public Vector3 lastPosition;
    public bool moving;
}
