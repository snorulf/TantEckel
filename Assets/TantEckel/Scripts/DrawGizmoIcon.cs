using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawGizmoIcon : MonoBehaviour
{
    [SerializeField] string iconName;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var sceneView = SceneView.currentDrawingSceneView;
        if (sceneView != null && sceneView.drawGizmos)
            Gizmos.DrawIcon(transform.position, iconName, allowScaling: false);
    }
#endif
}
