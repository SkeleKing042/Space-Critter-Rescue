//Created by Jackson Lucas
//Last edited by Jackson Lucas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPylon : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private bool isUnlocked;
    [SerializeField]
    private GameObject On_GameObject;
    [SerializeField]
    private GameObject Off_GameObject;

    public bool[] PassOnRotation = new bool[3];
    public bool OffsetAffectedByRotation;
    public void PullObjectHere(GameObject sender)
    {
        sender.transform.position = transform.position;

        //x inheritance
        if (PassOnRotation[0]) sender.transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, sender.transform.rotation.eulerAngles.y, sender.transform.rotation.eulerAngles.z);
        //y inheritance
        if (PassOnRotation[1]) sender.transform.eulerAngles = new Vector3(sender.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, sender.transform.rotation.eulerAngles.z);
        //z inheritance
        if (PassOnRotation[2]) sender.transform.eulerAngles = new Vector3(sender.transform.rotation.eulerAngles.x, sender.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isUnlocked = true;
            Off_GameObject.SetActive(false);
            On_GameObject.SetActive(true);
        }
    }
}
