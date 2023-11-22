//Created by Ru McPhalin
//Last edited by Jackson Lucas

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
    public bool TabletState;

    //[Header("Components")]
    [SerializeField]
    private Animator _UI_animator;
    private PlayerMovement _playerMovement;
    private PylonManager _pylonManager;

    [Header("Tabs")]
    [Tooltip("Array that stores the tabs of the tablet in order")]
    [SerializeField] private GameObject[] _tabs;
    [SerializeField] private GameObject _tutorialTab;
    private bool _tutorRead = false;
    [Tooltip("index that stores which tab is the map tab")]
    [SerializeField] public int MapTabIndex;
    //[Tooltip("index that stores the current tab in the tablet")]
    [HideInInspector] public int TabIndex;

    [Header("Teleport Locations")]
    [Tooltip("array that stores the location image components")]
    [SerializeField] private Image[] _teleportLocationImages;
    //[Tooltip("index that stores what teleport location is currently selected")]
    private int _teleportIndex;
    //[Tooltip("array that stores if the teleport location has been activated")]
    private bool[] _hasTeleportLocationBeenActivated;
    [Space]
    [SerializeField] private Color _color_currentlySelectedTeleport = Color.blue;
    [SerializeField] private Color _color_unavailableTeleport = Color.red;
    [SerializeField] private Color _color_availableTeleport = Color.green;

    private UI_Manager _Manager;

    private Inventory _invRef;
    [SerializeField] private List<Image> _largeBackpackSlots;
    [SerializeField] private List<Image> _smallBackpackSlots;
    [SerializeField] private List<Sprite> _critterIcons;

    private void Start()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _pylonManager = FindObjectOfType<PylonManager>();
        _Manager = FindObjectOfType<UI_Manager>();
        _invRef = FindObjectOfType<Inventory>();

        _hasTeleportLocationBeenActivated = new bool[_teleportLocationImages.Length];

        SetupBackpack();
        //FindObjectOfType<GameManager>().UpdateAllBars();
        SetTeleportLocationColors();
    }

    #region ToggleTablet
    /// <summary>
    /// Turns the tablet on and off
    /// </summary>
    public void ToggleTablet()
    {
        if (TabletState)
        {
            _UI_animator.SetTrigger("UI_Tablet_OFF");
            SetTabletState(false);

            if (!_tutorRead)
            {
                _tutorialTab.SetActive(false);
                foreach (GameObject tab in _tabs)
                {
                    tab.SetActive(false);
                }
                _tabs[0].SetActive(true);
                _tutorRead = true;
            }

            _playerMovement.DoMovement = true;

        }
        else if (!TabletState)
        {
            _UI_animator.SetTrigger("UI_Tablet_ON");
            SetTabletState(true);

            _playerMovement.DoMovement = false;

            UpdateCurrentTab();
        }
    }

    /// <summary>
    /// Method that sets the bool tabletState
    /// </summary>
    /// <param name="inputBool"></param>
    public void SetTabletState(bool inputBool)
    {
        TabletState = inputBool;
    }

    /// <summary>
    /// turns all the tabs off and then activates the correct one
    /// also resets the colors of the teleport icons
    /// </summary>
    public void ActivateCorrectTab()
    {
        if (_tutorRead && _tutorialTab.activeSelf)
        {
            _tutorialTab.SetActive(false);
        }
        foreach (GameObject tab in _tabs)
        {
            tab.SetActive(false);
        }

        _tabs[TabIndex].SetActive(true);

        UpdateCurrentTab();
    }
    private void UpdateCurrentTab() 
    { 
        switch (TabIndex)
        {
            case 0:
                SetupBackpack();
                break;
            case 1:
                //FindObjectOfType<GameManager>().UpdateAllBars();
                break;
            case 2:
                SetTeleportLocationColors();
                break;
        }
    }

    /// <summary>
    /// moves the tablet tab to the left
    /// </summary>
    public void MoveTabLeft()
    {
        if (TabletState)
        {
            if (TabIndex > 0)
            {
                TabIndex--;
            }
            else
            {
                TabIndex = _tabs.Length - 1;
            }
        }

        ActivateCorrectTab();
    }

    /// <summary>
    /// moves tab right
    /// </summary>
    public void MoveTabRight()
    {
        if (TabletState)
        {
            if (TabIndex < _tabs.Length - 1)
            {
                TabIndex++;
            }
            else
            {
                TabIndex = 0;
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
        for (int i = 0; i < _teleportLocationImages.Length; i++)
        {
            if (_hasTeleportLocationBeenActivated[i])
            {
                _teleportLocationImages[i].color = _color_availableTeleport;
            }
            else
            {
                _teleportLocationImages[i].color = Color.red;
            }
        }

        _teleportLocationImages[_teleportIndex].color = _color_currentlySelectedTeleport;
    }

    /// <summary>
    /// move teleport index left and update colours
    /// </summary>
    public void MoveTeleportIndexLeft()
    {
        if (TabletState)
            if (TabIndex == MapTabIndex)
            {
                if (_teleportIndex > 0)
                {
                    _teleportIndex--;
                }
                else
                {
                    _teleportIndex = _teleportLocationImages.Length - 1;
                }

                SetTeleportLocationColors();
            }
    }

    /// <summary>
    /// move teleport index right and update colours
    /// </summary>
    public void MoveTeleportIndexRight()
    {
        if (TabletState)
            if (TabIndex == MapTabIndex)
            {
                if (_teleportIndex < _teleportLocationImages.Length - 1)
                {
                    _teleportIndex++;
                }
                else
                {
                    _teleportIndex = 0;
                }

                SetTeleportLocationColors();
            }
    }

    /// <summary>
    /// select teleport location
    /// </summary>
    public void SelectTeleport()
    {
        if (TabletState)

            if (_hasTeleportLocationBeenActivated[_teleportIndex])
            {
                ToggleTablet();
                _pylonManager.GoToPylon(_teleportIndex);
            }
    }

    public void SethasTeleportBeenActivated(int pylonIndex)
    {
        _hasTeleportLocationBeenActivated[pylonIndex] = true;
    }







    #endregion

    #region BackpackTab
    private void EmptySlots()
    {
        foreach(Image slot in _largeBackpackSlots)
        {
            slot.gameObject.SetActive(false);
        }
        foreach(Image slot in _smallBackpackSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }
    private void SetupBackpack()
    {
        EmptySlots();
        
        for(int i = 0; i < _invRef.LargeCount; i++)
        {
            _largeBackpackSlots[i].gameObject.SetActive(true);
            if(i < _invRef.Player_Fungi_Large)
                _largeBackpackSlots[i].sprite = _critterIcons[0];
            else
                _largeBackpackSlots[i].sprite = _critterIcons[1];
            if (i >= _largeBackpackSlots.Count - 1)
                break;
        }
        for(int i = 0; i < _invRef.SmallCount; i++)
        {
            _smallBackpackSlots[i].gameObject.SetActive(true);
            if(i < _invRef.Player_Fungi_Small)
                _smallBackpackSlots[i].sprite = _critterIcons[2];
            else
                _smallBackpackSlots[i].sprite = _critterIcons[3];
            if (i >= _smallBackpackSlots.Count - 1)
                break;
        }
    }
    #endregion
}
