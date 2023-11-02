//Created by Jackson Lucas
//Last edited by Jackson Lucas

using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor
{
    private InputManager iM;
    private void OnSceneGUI()
    {
        iM = (InputManager)target;
        Handles.color = Color.blue;

        //Conversion would be needed for this an i cannot be bothered to do that atm (2/11 - Jackson)
        for (int i = 0; i < iM.MovementAction.GetPersistentEventCount(); i++)
        {
            Handles.DrawLine(iM.transform.position, iM.MovementAction.GetPersistentTarget(i).GameObject().transform.position);
        }

        DrawToEvents(iM.SprintAction);
        DrawToEvents(iM.CrouchAction);
        DrawToEvents(iM.JumpAction);
        DrawToEvents(iM.JetPackAction.Action);
        DrawToEvents(iM.TrapInteractionAction);
        DrawToEvents(iM.EnableTrapAction);
        DrawToEvents(iM.TabletAction);
        DrawToEvents(iM.FireAction.Action);
        DrawToEvents(iM.AltFireAction);
        DrawToEvents(iM.ReturnToShipAction);
        DrawToEvents(iM.SwitchToolAction);
    }
    private void DrawToEvents(UnityEvent incomingEvent)
    {
        for (int i = 0; i < incomingEvent.GetPersistentEventCount(); i++)
        {
            Handles.DrawLine(iM.transform.position, incomingEvent.GetPersistentTarget(i).GameObject().transform.position);
        }
    }
}
