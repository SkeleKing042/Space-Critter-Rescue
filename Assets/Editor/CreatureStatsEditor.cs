//Created by Jackson Lucas
//Last edited by Jackson Lucas

using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreatureStats))]
public class CreatureStatsEditor : Editor
{
    private void OnSceneGUI()
    {
        CreatureStats stats = (CreatureStats)target;
        Handles.color = new Color(0, 0.5f, 0, 0.25f);
        Handles.DrawSolidDisc(target.GameObject().transform.position, Vector3.up, stats.ExplorationRange);
    }
}
