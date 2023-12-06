//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TeleportPylon))]
public class TeleportPylonEditor : Editor
{
    private void OnSceneGUI()
    {
        TeleportPylon pylon = (TeleportPylon)target;

        Handles.color = new Color(0.5f, 0.65f, 0, 0.5f);
        Handles.DrawSolidDisc(pylon.transform.position, pylon.transform.up, 1);
        Handles.color = new Color(0, 0, 0, 1);
        Handles.DrawLine(pylon.transform.position, pylon.transform.position + pylon.transform.forward, 1);
    }
}
