//Created by Jackson Lucas
//Last edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonManager : MonoBehaviour
{
    [System.Serializable]
    public class arrayYAxis
    {
        public TeleportPylon[] Pylons;
    }
    [SerializeField]private arrayYAxis[] _pylonArray;
    public arrayYAxis[] PylonArray { get { return _pylonArray; } }
    //public List<TeleportPylon> Pylons = new List<TeleportPylon>();
    /// <summary>
    /// Moves the object to a pylon
    /// </summary>
    /// <param name="index"></param>
    /// <param name="sender"></param>
    public void GoToPylon(Vector2 index, GameObject sender)
    {
        _pylonArray[(int)index.x].Pylons[(int)index.y].PullObjectHere(sender);
    }
    /// <summary>
    /// Moves the player to a pylon
    /// </summary>
    /// <param name="index"></param>
    public void GoToPylon(Vector2 index)
    {
        _pylonArray[(int)index.x].Pylons[(int)index.y].PullObjectHere(GameObject.FindGameObjectWithTag("Player"));
    }
}
