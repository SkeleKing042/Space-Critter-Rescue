using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DrinkableSource : MonoBehaviour
{
    [SerializeField]
    private float Radius;

    private void Awake()
    {
        Radius = GetComponent<SphereCollider>().radius;
    }
    public float GetRadius()
    {
        return Radius;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 255, 255, 0.50f);
        Gizmos.DrawSphere(transform.position, Radius);
    }
}
