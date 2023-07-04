using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IKTarget : MonoBehaviour
{
    [SerializeField] string iconName = "IKTarget.png";

    private void OnDrawGizmos()
    {
        var sceneView = SceneView.currentDrawingSceneView;
        if (sceneView != null && sceneView.drawGizmos)
            Gizmos.DrawIcon(transform.position, iconName, allowScaling: false);
    }
}
