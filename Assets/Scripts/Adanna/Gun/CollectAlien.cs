using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CollectAlien : MonoBehaviour
{
    public TMP_Text AlienUICount;
    private int AlienCount;
    public bool mouseDown = false;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            mouseDown = true;
            //Vector3 position = PlayerGun.GetComponent<Vector3>;

        }
        if (Input.GetButtonUp("Fire1"))
        {
            mouseDown = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

       
    }
    void OnTriggerEnter(Collider creature)
    {
        Debug.Log("collision");
        if (creature.gameObject.tag == "Alien" && mouseDown)
        {

            creature.gameObject.SetActive(false);
            AlienCount++;
            AlienUICount.text = "Alien Count: " + AlienCount.ToString();
        }
    }
}