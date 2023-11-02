//Created by Jackson Lucas
//Last edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonManager : MonoBehaviour
{
    public List<TeleportPylon> Pylons = new List<TeleportPylon>();
    /// <summary>
    /// Moves the object to a pylon
    /// </summary>
    /// <param name="index"></param>
    /// <param name="sender"></param>
    public void GoToPylon(int index, GameObject sender)
    {
        Pylons[index].PullObjectHere(sender);
    }
    /// <summary>
    /// Moves the player to a pylon
    /// </summary>
    /// <param name="index"></param>
    public void GoToPylon(int index)
    {
        Pylons[index].PullObjectHere(GameObject.FindGameObjectWithTag("Player"));
    }
}
