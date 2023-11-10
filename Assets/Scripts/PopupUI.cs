using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    public Image PopImage;
    // Start is called before the first frame update
    void Start()
    {
        PopImage.enabled = false;
    }

    public void EnablePopup()
    {
        PopImage.enabled = true;
    }
}
