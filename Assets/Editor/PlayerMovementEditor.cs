using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerMovement player = (PlayerMovement)target;
        Handles.color = Color.yellow;
        Handles.DrawLine(player.transform.position, player.transform.position + -Vector3.up * player.PlayerHeight, 1);
    }
}
