using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tablet : MonoBehaviour
{
    [Header("Enums")]
    [SerializeField] TabletState tabletState;

    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] PlayerInput playerInput;

    [Header("Tabs")]
    [SerializeField] GameObject[] tabs;
    [SerializeField] int tabIndex;

    public enum TabletState
    {
        on,
        off,
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleTablet()
    {
        if(tabletState == TabletState.on)
        {
            animator.SetTrigger("Lower Tablet");
            SetTabletState(TabletState.off);
        }
        else if(tabletState == TabletState.off)
        {
            animator.SetTrigger("Raise Tablet");
            SetTabletState(TabletState.on);
        }
    }

    public void SetTabletState(Tablet.TabletState inputTabletState)
    {
        tabletState = inputTabletState;
    }

    public void ActivateCorrectTab()
    {
        foreach(GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        tabs[tabIndex].SetActive(true);
    }

    public void MoveTabLeft()
    {
        if (tabletState == TabletState.on)
        {
            if (tabIndex > 0)
            {
                tabIndex--;
            }
            else
            {
                tabIndex = tabs.Length - 1;
            }
        }

        ActivateCorrectTab();
    }

    public void MoveTabRight()
    {
        if (tabletState == TabletState.on)
        {
            if (tabIndex < tabs.Length - 1)
            {
                tabIndex++;
            }
            else
            {
                tabIndex = 0;
            }
        }

        ActivateCorrectTab();
    }


}
