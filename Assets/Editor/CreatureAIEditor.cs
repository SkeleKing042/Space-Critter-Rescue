using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreatureAI))]
public class CreatureAIEditor : Editor
{
    private void OnSceneGUI()
    {
        CreatureAI cAI = (CreatureAI)target;

        if (cAI.GetAgent != null)
        {
            Handles.color = Color.blue;
            Handles.DrawLine(cAI.transform.position, cAI.GetAgent.destination);
        }

    }
}
