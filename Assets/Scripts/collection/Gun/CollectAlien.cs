using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CollectAlien : MonoBehaviour
{
    public TMP_Text AlienUICount;
    private float AlienCount;
    public bool mouseDown = false;

    public VacuumGun Vac;
    public Collection collection;
    public Inventory PlayerInventory;

    GameObject Alien;

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
        AlienUICount.text = ": " + collection.TextNumber.ToString();

    }


    void OnTriggerEnter(Collider creature)
    {

        if ((creature.gameObject.tag == "alien" || creature.gameObject.tag == "bigAlien") && mouseDown)
        {
            collection.AddAlienToCollection(creature.gameObject);

            // AlienCount++;
            Alien = creature.gameObject;


            Vac.UnassignAlien();
            Destroy(creature.gameObject);
            PlayerInventory.AddToPlayerInventory(Alien);



            //Vac.EndPull();
            
        }
    }
}
