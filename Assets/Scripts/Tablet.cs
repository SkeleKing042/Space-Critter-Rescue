using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    [Header("Enums")]
    [SerializeField] TabletState tabletState;

    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] PlayerInput input;

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

    public void ToggleTablet_On()
    {
        TurnTabletOn();
    }
    public void ToggleTablet_Off()
    {
        TurnTabletOff();
    }


    public void SetTabletState(Tablet.TabletState inputTabletState)
    {
        tabletState = inputTabletState;
    }

    public void TurnTabletOff()
    {
        animator.SetTrigger("Lower Tablet");
        SetTabletState(TabletState.off);
    }

    public void TurnTabletOn()
    {
        animator.SetTrigger("Raise Tablet");
        SetTabletState(TabletState.on);
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
    }


}
