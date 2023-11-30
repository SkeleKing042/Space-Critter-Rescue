//Created by Ru McPhalin
//Last edited by Jackson Lucas

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tablet : MonoBehaviour
{
    //Variables
    #region Variables
    [Header("Tablet State"), Tooltip("if the tablet is on or off")]
    private bool _tabletState;
    public bool TabletState { get { return _tabletState; } }

    [Header("Components")]
    [SerializeField] private Animator _UI_animator;
    //[SerializeField]
    //private UI_Manager _UI_Manager;
    private Equipment _equipment;
    private PlayerMovement _playerMovement;
    private PylonManager _pylonManager;


    [Header("Tabs")]
    [SerializeField, Tooltip("An array that stores the tabs of the tablet")] 
    private GameObject[] _tabs;

    [SerializeField, Tooltip("index that stores which tab is the map tab")] 
    public int MapTabIndex;
    
    [SerializeField, Tooltip("index that stores the current tab in the tablet")]
    [HideInInspector] public int TabIndex = 0;

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

    [Header("Inventory")]
    [SerializeField] private GameObject _largeBackpackSlotParent;
    private List<Image> _largeBackpackSlots = new List<Image>();
    [SerializeField] private GameObject _smallBackpackSlotParent;
    private List<Image> _smallBackpackSlots = new List<Image>();
    [SerializeField] private List<Sprite> _critterIcons;
    private Inventory _invRef;

    [Header("SFX")]
    [SerializeField] private AudioSource _SFXSource_Tablet;
    [SerializeField] private AudioClip _SFXClip_TabletOn;
    [SerializeField] private AudioClip _SFXClip_TabletOff;
    [SerializeField] private AudioClip _SFXClip_ChangeTeleportLocation;
    [SerializeField] private AudioClip _SFXClip_ChangeTab;

    #endregion

    //Methdods
    #region Start
    private void Awake()
    {
        //find components
        //_UI_Manager = FindObjectOfType<UI_Manager>();
        _equipment = FindObjectOfType<Equipment>();
        _invRef = FindObjectOfType<Inventory>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _pylonManager = FindObjectOfType<PylonManager>();

        //declare array
        _hasTeleportLocationBeenActivated = new bool[_teleportLocationImages.Length];

        foreach(var child in _largeBackpackSlotParent.GetComponentsInChildren<Image>())
        {
            if (child.name == "Critter Sprite")
                _largeBackpackSlots.Add(child);
        }
        foreach (var child in _smallBackpackSlotParent.GetComponentsInChildren<Image>())
        {
            if (child.name == "Critter Sprite")
                _smallBackpackSlots.Add(child);
        }

        //setup backpack
        SetupBackpack();
        //FindObjectOfType<GameManager>().UpdateAllBars();

        //setup map tab
        SetTeleportLocationColors();
    }

    #endregion

    #region ToggleTablet
    /// <summary>
    /// Turns the tablet on and off
    /// </summary>
    public void ToggleTablet()
    {
        //turn tablet off
        if (TabletState)
        {
            //play SFX
            PlaySFX_TabletOff();

            //animate tablet down
            _equipment._animation_Tablet_Down();
            
            //up the correct equipment
            switch (_equipment._currentlyHolding)
            {
                case Equipment.CurrentlyHolding.VC:
                    {
                        _equipment._animation_VC_Up();
                        break;
                    }
                case Equipment.CurrentlyHolding.trap:
                    {
                        _equipment._animation_Trap_Up();
                        break;
                    }
                case Equipment.CurrentlyHolding.detonator:
                    {
                        _equipment._animation_Detonator_Up();
                        break;
                    }
            }

            _UI_animator.SetTrigger("UI_Tablet_OFF");
            SetTabletState(false);
            _playerMovement.DoMovement = true;

        }
        //turn tablet on
        else
        {
            //play SFX
            PlaySFX_TabletOn();

            //animate tablet up
            _equipment._animation_TabletUp();

            //up the correct equipment
            switch (_equipment._currentlyHolding)
            {
                case Equipment.CurrentlyHolding.VC:
                    {
                        _equipment._animation_VC_Down();
                        break;
                    }
                case Equipment.CurrentlyHolding.trap:
                    {
                        _equipment._animation_Trap_Down();
                        break;
                    }
                case Equipment.CurrentlyHolding.detonator:
                    {
                        _equipment._animation_Detonator_Down();
                        break;
                    }
            }

            _UI_animator.SetTrigger("UI_Tablet_ON");
            SetTabletState(true);

            _playerMovement.DoMovement = false;

            ActivateCorrectTab();
        }
    }

    /// <summary>
    /// Method that sets the bool tabletState
    /// </summary>
    /// <param name="inputBool"></param>
    public void SetTabletState(bool inputBool)
    {
        _tabletState = inputBool;
    }

    /// <summary>
    /// turns all the tabs off and then activates the correct one
    /// also resets the colors of the teleport icons
    /// </summary>
    public void ActivateCorrectTab()
    {
        //disactivate all tabs
        foreach (GameObject tab in _tabs)
        {
            tab.SetActive(false);
        }

        //activate correct tab
        _tabs[TabIndex].SetActive(true);


        //Update current tab
        UpdateCurrentTab();
    }

    /// <summary>
    /// Runs a method dependant on the tab index
    /// </summary>
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

    #region SFX
    //play the SFX tablet on
    public void PlaySFX_TabletOn()
    {
        //assign the clip
        _SFXSource_Tablet.clip = _SFXClip_TabletOn;
        //play the clip
        _SFXSource_Tablet.Play();
    }

    //play the SFX Tablet Off
    public void PlaySFX_TabletOff()
    {
        //assign the clip
        _SFXSource_Tablet.clip = _SFXClip_TabletOff;
        //play the clip
        _SFXSource_Tablet.Play();
    }

    //play the SFX change tab
    public void PlaySFX_ChangeTab()
    {
        //assign the clip
        _SFXSource_Tablet.clip = _SFXClip_ChangeTab;
        //play the clip
        _SFXSource_Tablet.Play();
    }

    //play SFX change teleport location
    public void PlaySFX_ChangeTeleportLocation()
    {
        //assign the clip
        _SFXSource_Tablet.clip = _SFXClip_ChangeTeleportLocation;
        //play the clip
        _SFXSource_Tablet.Play();
    }

    #endregion

}
