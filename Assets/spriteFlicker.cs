using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteFlicker : MonoBehaviour
{
    [SerializeField] private Tablet _tablet;

    [SerializeField] private Image _image;

    [SerializeField] private Sprite _sprite1;
    [SerializeField] private Sprite _sprite2;

    [SerializeField] private float flickerRate;
    [SerializeField] private float timer;
 
    // Start is called before the first frame update
    void Start()
    {
        _image.sprite = _sprite1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_tablet.TabIndex == 0 && _tablet.TabletState)
        {
            if (timer <= 0)
            {
                timer = flickerRate;
                if (_image.sprite == _sprite1)
                {
                    _image.sprite = _sprite2;
                }
                else
                {
                    _image.sprite = _sprite1;
                }
            }

            timer -= Time.deltaTime;
        }
    }
}
