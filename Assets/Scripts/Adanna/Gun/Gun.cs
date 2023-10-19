using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class Gun : MonoBehaviour
{
    public bool mouseDown = false;
    public GameObject PlayerGun;

    public float MaxRange = 5000;

    [SerializeField]
    // public float _force;

    private void Start()
    {
    }

    public GameObject creature;
    void Update()
    {


       // Shoot();

        //Collisionary

        //OnCollisionEnter(creature);


    }


    public void Shoot()
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

        if (mouseDown)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var origin = transform.position;
            Debug.Log("ray cast trying");
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("raycastOut");
                if (hit.transform != null)
                {
                    Debug.Log("raycastHit");
                    if (hit.transform.tag == "alien")
                    {
                        Debug.Log("creature hit");

                        creature = hit.transform.gameObject;
                        // Vector3 distance;
                        // distance = creature.transform.position - origin;


                        float distance = Vector3.Distance(creature.transform.position, transform.position);
                        Debug.Log(distance);

                        if (distance < MaxRange)
                        {
                            Debug.Log("is pulling");
                            Pull();
                        }


                        if (distance > MaxRange)
                        {
                            Debug.Log("out of range");
                        }
                    }

                }
            }

        }


        void Pull()
        {
            //  while(mouseDown == true)
            creature.transform.position = Vector3.Lerp(creature.transform.position, PlayerGun.transform.position, 1f * Time.deltaTime);

        }

    }
}


