//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PylonManager))]
public class PylonManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        PylonManager manager = (PylonManager)target;
        foreach(TeleportPylon pylon in manager.Pylons)
        {
            DrawPylonVisuals(pylon);
            Handles.color = Color.blue;
            Handles.DrawLine(manager.transform.position, pylon.transform.position);
        }
    }
    private void DrawPylonVisuals(TeleportPylon pylon)
    {
        Handles.color = new Color(0.5f, 0.65f, 0, 0.5f);
        Handles.DrawSolidDisc(pylon.transform.position, pylon.transform.up, 1);
    }
}
