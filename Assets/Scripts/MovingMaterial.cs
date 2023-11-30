using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMaterial : MonoBehaviour
{
    Material _selfMatRef;
    [SerializeField] private Vector2 _scrollSpeed;

    private Vector2 _mapOffset;
    // Start is called before the first frame update
    void Start()
    {
        _selfMatRef = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _mapOffset += _scrollSpeed * Time.deltaTime;

        if (_mapOffset.x > 1) _mapOffset = new Vector2(_mapOffset.x - 1, _mapOffset.y);
        if (_mapOffset.y > 1) _mapOffset = new Vector2(_mapOffset.x, _mapOffset.y - 1);

        _selfMatRef.mainTextureOffset = _mapOffset;
    }
}
