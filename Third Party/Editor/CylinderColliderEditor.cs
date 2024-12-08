using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

[CustomEditor(typeof(CylinderCollider))]
public class CylinderCollderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CylinderCollider script = (CylinderCollider)target;
        if (GUILayout.Button("Build Object"))
        {
            script.BuildCollider();
        }
        if (GUILayout.Button("Clear Collider"))
        {
            script.ClearCollider();
        }
    }
}