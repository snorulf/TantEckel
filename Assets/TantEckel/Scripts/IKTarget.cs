using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IKTarget : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var sceneView = SceneView.currentDrawingSceneView;
        if (sceneView != null && sceneView.drawGizmos)
            Gizmos.DrawIcon(transform.position, "tentacle.png", allowScaling: false);
    }
}
