using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    PlayerMovement player;
    private void OnSceneGUI()
    {
        player = (PlayerMovement)target;
        DrawGroundCheckBox();
    }

    private void DrawGroundCheckBox()
    {
        Handles.color = new Color(0, 0, 0.5f, 1f);
        Vector3[] points = new Vector3[4];

        points[0] = player.GroundPoints[0];
        points[1] = player.GroundPoints[1];
        points[2] = player.GroundPoints[2];
        points[3] = player.GroundPoints[3];
        Handles.DrawLine(points[0], points[1]);
        Handles.DrawLine(points[1], points[2]);
        Handles.DrawLine(points[2], points[3]);
        Handles.DrawLine(points[3], points[0]);

        Handles.DrawLine(points[0], points[0] + Vector3.down * player.PlayerHeight);
        Handles.DrawLine(points[1], points[1] + Vector3.down * player.PlayerHeight);
        Handles.DrawLine(points[2], points[2] + Vector3.down * player.PlayerHeight);
        Handles.DrawLine(points[3], points[3] + Vector3.down * player.PlayerHeight);

        Handles.DrawLine(points[0] + Vector3.down * player.PlayerHeight, points[1] + Vector3.down * player.PlayerHeight);
        Handles.DrawLine(points[1] + Vector3.down * player.PlayerHeight, points[2] + Vector3.down * player.PlayerHeight);
        Handles.DrawLine(points[2] + Vector3.down * player.PlayerHeight, points[3] + Vector3.down * player.PlayerHeight);
        Handles.DrawLine(points[3] + Vector3.down * player.PlayerHeight, points[0] + Vector3.down * player.PlayerHeight);
    }
}
