using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DrinkenFinden))]
public class DrinkenFindenEditor : Editor
{
    private void OnSceneGUI()
    {
        DrinkenFinden dF = (DrinkenFinden)target;
        Handles.color = Color.blue;
        foreach (GameObject source in dF.Sources)
        {
            Handles.DrawLine(dF.transform.position, source.transform.position);
        }
    }
}
