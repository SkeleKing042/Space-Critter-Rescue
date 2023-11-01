using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreatureAI))]
public class CreatureAIEditor : Editor
{
    private void OnSceneGUI()
    {
        CreatureAI cAI = (CreatureAI)target;
        if (cAI != null)
        {
            Handles.color = new Color(0, 155, 0, 0.1f);
            Handles.DrawSolidDisc(cAI.HomePoint, Vector3.up, cAI.TravelDistance);
            Handles.color = Color.blue;
            Handles.DrawLine(cAI.transform.position, cAI.GetAgent.destination);
        }
    }
}
