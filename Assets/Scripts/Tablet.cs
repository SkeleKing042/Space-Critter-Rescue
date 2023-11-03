using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tablet : MonoBehaviour
{
    [Header("Enums")]
    [Tooltip("if the tablet is on or off")]
    public bool tabletState;

    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PylonManager pylonManager;

    [Header("Tabs")]
    [Tooltip("index that stores the current tab in the tablet")]
    [SerializeField] public int tabIndex;
    [Tooltip("index that stores which tab is the map tab")]
    [SerializeField] public int mapTabIndex;
    [Tooltip("Array that stores the tabs of the tablet in order")]
    [SerializeField] GameObject[] tabs;

    [Header("Teleport Locations")]
    [Tooltip("index that stores what teleport location is currently selected")]
    [SerializeField] public int teleportIndex;
    [Tooltip("array that stores the location image components")]
    [SerializeField] Image[] teleportLocationImages;
    [Tooltip("array that stores if the teleport location has been activated")]
    [SerializeField] public bool[] hasTeleportLocationBeenActivated;
    [Space]
    [SerializeField] Color color_currentlySelectedTeleport;
    [SerializeField] Color color_unavailableTeleport;
    [SerializeField] Color color_availableTeleport;


    #region ToggleTablet
    /// <summary>
    /// Turns the tablet on and off
    /// </summary>
    public void ToggleTablet()
    {
        if(tabletState)
        {
            animator.SetTrigger("Lower Tablet");
            SetTabletState(false);
        }
        else if(!tabletState)
        {
            animator.SetTrigger("Raise Tablet");
            SetTabletState(true);

            playerMovement._movementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// Method that sets the bool tabletState
    /// </summary>
    /// <param name="inputBool"></param>
    public void SetTabletState(bool inputBool)
    {
        tabletState = inputBool;
    }

    /// <summary>
    /// turns all the tabs off and then activates the correct one
    /// also resets the colors of the teleport icons
    /// </summary>
    public void ActivateCorrectTab()
    {
        foreach(GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        tabs[tabIndex].SetActive(true);

        if(tabIndex == mapTabIndex)
        {
            SetTeleportLocationColors();
        }
    }

    /// <summary>
    /// moves the tablet tab to the left
    /// </summary>
    public void MoveTabLeft()
    {
        if (tabletState)
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

    /// <summary>
    /// moves tab right
    /// </summary>
    public void MoveTabRight()
    {
        if (tabletState)
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

    #endregion

    #region MapTab

    /// <summary>
    /// sets the teleport locations to the correct colours
    /// </summary>
    private void SetTeleportLocationColors()
    {
        for(int i = 0; i < teleportLocationImages.Length; i++)
        {
            if (hasTeleportLocationBeenActivated[i])
            {
                teleportLocationImages[i].color = color_availableTeleport;
            }
            else
            {
                teleportLocationImages[i].color = Color.red;
            }
        }

        teleportLocationImages[teleportIndex].color = color_currentlySelectedTeleport;
    }

    /// <summary>
    /// move teleport index left and update colours
    /// </summary>
    public void MoveTeleportIndexLeft()
    {
        if (tabIndex == mapTabIndex)
        {
            if (teleportIndex > 0)
            {
                teleportIndex--;
            }
            else
            {
                teleportIndex = teleportLocationImages.Length - 1;
            }

            SetTeleportLocationColors();
        }
    }

    /// <summary>
    /// move teleport index right and update colours
    /// </summary>
    public void MoveTeleportIndexRight()
    {
        if (tabIndex == mapTabIndex)
        {
            if (teleportIndex < teleportLocationImages.Length - 1)
            {
                teleportIndex++;
            }
            else
            {
                teleportIndex = 0;
            }

            SetTeleportLocationColors();
        }
    }

    /// <summary>
    /// select teleport location
    /// </summary>
    public void SelectTeleport()
    {
        if (hasTeleportLocationBeenActivated[teleportIndex])
        {
            ToggleTablet();
            pylonManager.GoToPylon(teleportIndex);
        }
    }

    public void SethasTeleportBeenActivated(int pylonIndex)
    {
        hasTeleportLocationBeenActivated[pylonIndex] = true;
    }







    #endregion
}
