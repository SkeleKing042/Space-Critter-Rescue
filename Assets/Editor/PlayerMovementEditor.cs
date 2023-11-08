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
        Vector3 playerForwards = Vector3.Cross(Camera.main.transform.forward, Vector3.up);
        if(player.VelocityBasedChecks)
        {
            playerForwards = Vector3.Cross(player.PlayerRigidbody.velocity.normalized, Vector3.up);
        }

        Handles.color = new Color(0, 0, 0.5f, 1f);
        Vector3[] points = new Vector3[8];
        
        points[0] = player.transform.position + Vector3.Cross(playerForwards, Vector3.up) * player.StrideLength + -Vector3.up * player.PlayerHeight;
        points[1] = player.transform.position + playerForwards * player.StrideWidth + -Vector3.up * player.PlayerHeight;
        points[2] = player.transform.position + Vector3.Cross(playerForwards, Vector3.up) * -player.StrideLength + -Vector3.up * player.PlayerHeight;
        points[3] = player.transform.position + playerForwards * - player.StrideWidth + -Vector3.up * player.PlayerHeight;
        Handles.DrawLine(points[0], points[1]);
        Handles.DrawLine(points[1], points[2]);
        Handles.DrawLine(points[2], points[3]);
        Handles.DrawLine(points[3], points[0]);

        points[4] = player.transform.position + Vector3.Cross(playerForwards, Vector3.up) * player.StrideLength;
        points[5] = player.transform.position + playerForwards * player.StrideWidth;
        points[6] = player.transform.position + Vector3.Cross(playerForwards, Vector3.up) * -player.StrideLength;
        points[7] = player.transform.position + playerForwards * -player.StrideWidth;
        Handles.DrawLine(points[4], points[5]);
        Handles.DrawLine(points[5], points[6]);
        Handles.DrawLine(points[6], points[7]);
        Handles.DrawLine(points[7], points[4]);

        Handles.DrawLine(points[0], points[4]);
        Handles.DrawLine(points[1], points[5]);
        Handles.DrawLine(points[2], points[6]);
        Handles.DrawLine(points[3], points[7]);
    }
}
