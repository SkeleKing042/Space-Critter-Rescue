using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    [Header("Enums")]
    [SerializeField] TabletState tabletState;
    [SerializeField] TabletTab tabletTab;

    [Header("Components")]
    [SerializeField] Animator animator;

    [Header("Tabs")]
    [SerializeField] GameObject[] tabs;
    [SerializeField] int tabIndex;

    public enum TabletState
    {
        on,
        off,
    }

    enum TabletTab
    {
        Backpack,
        Progress,
        Options,
        Controls,
        Quit
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ToggleTablet();
        MoveTabs();
    }

    public void ToggleTablet()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(tabletState == TabletState.on)
            {
                TurnTabletOff();
            }
            else
            {
                TurnTabletOn();
            }
        }
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

    public void MoveTabs()
    {
        if(tabletState == TabletState.on)
        {
            if(Input.GetKeyDown(KeyCode.Z) && tabIndex > 0)
            {
                tabIndex--;
                ActivateCorrectTab();
            }
            if(Input.GetKeyDown(KeyCode.X) && tabIndex < tabs.Length-1)
            {
                tabIndex++;
                ActivateCorrectTab();
            }
        }
    }


}
