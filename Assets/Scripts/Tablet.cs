//Created by Ru McPhalin
//Last edited by Jackson Lucas

using System;
using System.Collections.Generic;
using System.Reflection;
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
    [SerializeField] private Animator _Equipment_animator;

    //[SerializeField]
    //private UI_Manager _UI_Manager;
    private Equipment _equipment;
    private PlayerMovement _playerMovement;
    private PylonManager _pylonManager;
    private UI_Manager _ui_Manager;


    [Header("Tabs")]
    [SerializeField, Tooltip("An array that stores the tabs of the tablet")]
    private GameObject[] _tabs;

    [SerializeField, Tooltip("index that stores which tab is the map tab")]
    public int MapTabIndex;

    [SerializeField, Tooltip("index that stores the current tab in the tablet")]
    [HideInInspector] public int TabIndex = 0;

    [System.Serializable]
    public class SubArray 
    {
        public Image[] Images;
        [HideInInspector]public int[] Indexes;
    }

    [Header("Teleport Locations")]
    [Tooltip("Array that stores the location image components")]
    [SerializeField] private SubArray[] _teleportLocationImages;
    // Columms going from top to bottom
    // Row going left to right
    // MAP LAYOUT ON TABLET
    //[0 0 0 X]
    //[0 X X X]
    //[X X 0 0]
    //[X 0 0 0]
    private Vector2 _teleportIndex = new Vector2();
    //[Tooltip("index that stores what teleport location is currently selected")]
    //[Tooltip("array that stores if the teleport location has been activated")]
    //private bool[,] _hasTeleportLocationBeenActivated;
    [Space]
    [SerializeField] private Color _color_currentlySelectedTeleport = Color.blue;
    [SerializeField] private Color _color_unavailableTeleport = Color.red;
    [SerializeField] private Color _color_availableTeleport = Color.green;

    [Header("Inventory")]
    /*[SerializeField] private GameObject _largeBackpackSlotParent;
    private List<Image> _largeBackpackSlots = new List<Image>();
    [SerializeField] private GameObject _smallBackpackSlotParent;
    private List<Image> _smallBackpackSlots = new List<Image>();
    [SerializeField] private List<Sprite> _critterIcons;*/
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
        _ui_Manager = FindObjectOfType<UI_Manager>();

        Initialize2DArray();
        Update2DArrayVisuals();

        //declare array
        //_hasTeleportLocationBeenActivated = new bool[_teleportLocationImages.Length];

        /*foreach(var child in _largeBackpackSlotParent.GetComponentsInChildren<Image>())
        {
            if (child.name == "Critter Sprite")
                _largeBackpackSlots.Add(child);
        }
        foreach (var child in _smallBackpackSlotParent.GetComponentsInChildren<Image>())
        {
            if (child.name == "Critter Sprite")
                _smallBackpackSlots.Add(child);
        }*/

        /*int itemIndex = 0;
        for (int i = 0; i < _teleportLocationImages.Length; i++)// (var segment in imageArray)
        {
            _teleportLocationImages[i].SetUpArray(itemIndex);
            if (_teleportLocationImages[i].Images.Length != _teleportLocationImages[0].Images.Length)
            {
                Debug.Log("SIZE MISSMATCH.\nArray cannot be concave.\nFound at index " + i);
                this.enabled = false;
            }
        }
        foreach (var segment in _teleportLocationImages)
            foreach (var image in segment.Images)
            {
                if (image != null)
                    image.color = _color_unavailableTeleport;
            }*/

        //setup backpack
        //SetupBackpack();
        //FindObjectOfType<GameManager>().UpdateAllBars();

        //setup map tab
        //SetTeleportLocationColors();
    }
    private void Initialize2DArray()
    {
        //Size check for the 2D array
        //Goes through each x of the array and compares that x's subarray to the first to check if they are the same size
        for (int i = 0; i < _teleportLocationImages.Length; i++)
        {
            if (_teleportLocationImages[i].Images.Length != _teleportLocationImages[0].Images.Length)
            {
                Debug.Log("SIZE MISSMATCH.\nArray cannot be concave.\nFound at index " + i);
                this.enabled = false;
            }
        }

        //Index assignment
        //Goes through each element in each subarray and assigns it a number
        int currentindex = 0;
        foreach (var subarray in _teleportLocationImages)
        {
            //Matches the index array length to the image length
            subarray.Indexes = new int[subarray.Images.Length];
            for (int i = 0; i < subarray.Images.Length; i++)
            {
                //If there is valid data...
                if (subarray.Images[i] != null)
                {
                    //Assign the new index and increment it.
                    subarray.Indexes[i] = currentindex;
                    currentindex++;
                    //Break if we have no more elements in the pylon array.
                    if (currentindex >= _pylonManager.PylonArray.Length)
                        break;
                }
                else
                    //...otherwise, set the index to -1 (impossible value)
                    subarray.Indexes[i] = -1;
            }
            //Stop if the index is greater then the number of pylons.
            if (currentindex > _pylonManager.PylonArray.Length)
            {
                Debug.Log("Size mismatch between the 2d array elements and linear array");
                break;
            }
        }
        //Debuging check for when theres not enough elements.
        if (currentindex < _pylonManager.PylonArray.Length)
        {
            Debug.Log("Not all bools in the linear array are accounted for.\nYou won't be able to toggle any after the " + (currentindex - 1) + " element.");
        }
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

            //set the right arm animation
            _Equipment_animator.SetBool("isHolding_Tablet", false);

            //switch case for which arm to bring up
            //up the correct equipment
            switch (_equipment._currentlyHolding)
            {
                case Equipment.CurrentlyHolding.VC:
                    {
                        _Equipment_animator.SetBool("isHolding_VC", true);
                        break;
                    }
                /*case Equipment.CurrentlyHolding.trap:
                    {
                        _Equipment_animator.SetBool("isHolding_Trap", true);
                        break;
                    }
                case Equipment.CurrentlyHolding.detonator:
                    {
                        _Equipment_animator.SetBool("isHolding_Detonator", true);
                        break;
                    }*/
            }

            //set ui
            _UI_animator.SetBool("UI_TabletState", false);
            _ui_Manager.SetTabletToggleUI();

            //set tablet state to false
            SetTabletState(false);

            //remove player movement
            _playerMovement.DoMovement = true;

        }
        //turn tablet on
        else
        {
            //play SFX
            PlaySFX_TabletOn();

            //set the right arm animation
            _Equipment_animator.SetBool("isHolding_Tablet", true);
            //switch case for which arm to bring up
            //up the correct equipment
            switch (_equipment._currentlyHolding)
            {
                case Equipment.CurrentlyHolding.VC:
                    {
                        _Equipment_animator.SetBool("isHolding_VC", false);
                        break;
                    }
                /*case Equipment.CurrentlyHolding.trap:
                    {
                        _Equipment_animator.SetBool("isHolding_Trap", false);
                        break;
                    }
                case Equipment.CurrentlyHolding.detonator:
                    {
                        _Equipment_animator.SetBool("isHolding_Detonator", false);
                        break;
                    }*/
            }

            //set ui
            _UI_animator.SetBool("UI_TabletState", true);
            _ui_Manager.SetTabletToggleUI();


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
                //SetupBackpack();
                break;
            case 1:
                //FindObjectOfType<GameManager>().UpdateAllBars();
                break;
            case 2:
                //SetTeleportLocationColors();
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

    #region indexGets
    public int GetIndexAtPosition()
    {
        return _teleportLocationImages[(int)_teleportIndex.x].Indexes[(int)_teleportIndex.y];
    }
    public int GetIndexAtPosition(Vector2 vector2)
    {
        return _teleportLocationImages[(int)vector2.x].Indexes[(int)vector2.y];
    }
    public int GetIndexAtPosition(int x, int y)
    {
        return _teleportLocationImages[x].Indexes[y];
    }
    #endregion
    /// <summary>
    /// Check if the index at the array position is within the length of pylons
    /// </summary>
    /// <returns></returns>
    private bool Validate2DArrayOutput()
    {
        bool b = false;
        if (GetIndexAtPosition() < 0 || GetIndexAtPosition() > _pylonManager.PylonArray.Length)
            b = false;
        else
            if (_pylonManager.PylonArray[GetIndexAtPosition()])
                b = true;
        //Debug.Log("The position of " + _teleportIndex.ToString() + " is " + b);
        return b;
    }
    /// <summary>
    /// Sets the teleport locations to the correct colours
    /// </summary>
    private void Update2DArrayVisuals()
    {
        //Goes through each element in each subarray
        for (int x = 0; x < _teleportLocationImages.Length; x++)
        {
            for (int y = 0; y < _teleportLocationImages[x].Images.Length; y++)
            {
                //Checks if there's a valid location set
                if (_teleportLocationImages[x].Images[y] != null)
                {
                    //Double check
                    if (_teleportLocationImages[x].Indexes[y] < 0)
                        continue;

                    //If this location is the teleport index, that is were the cursor should be
                    else
                    {
                        //Change the image colour depending of if the pylon is on or off.
                        if (_pylonManager.PylonArray[GetIndexAtPosition(x, y)].isOn)
                        {
                            if (!_pylonManager.PylonArray[GetIndexAtPosition()].isOn)
                            {
                                _teleportIndex = new Vector2(x, y);
                            }
                            else
                            _teleportLocationImages[x].Images[y].color = _color_availableTeleport;
                        }
                        else
                        {
                            _teleportLocationImages[x].Images[y].color = _color_unavailableTeleport;

                        }
                    }
                }
            }
        }
        _teleportLocationImages[(int)_teleportIndex.x].Images[(int)_teleportIndex.y].color = _color_currentlySelectedTeleport;
    }
    /*private void SetTeleportLocationColors()
    {
        foreach (TeleportPylon pylon in _pylonManager.PylonArray)
        {
            if (pylon != null)
                if (pylon.isOn)
                {

                }
        }
        *//*if (_pylonManager.PylonArray[_teleportLocationImages[(int)_teleportIndex.y].Index[(int)_teleportIndex.x]] == null)
        {
            for(int x = 0; x < _pylonManager.PylonArray.Length; x++)
                for(int y = 0; y < _pylonManager.PylonArray[x].Pylons.Length; y++)
                {
                    if (_pylonManager.PylonArray[x].Pylons[y] != null)
                        if (_pylonManager.PylonArray[x].Pylons[y].isOn)
                            _teleportIndex = new Vector2(x, y);
                }
        }*//*
        for (int x = 0; x < _teleportLocationImages.Length; x++)
            for (int y = 0; y < _teleportLocationImages[x].Images.Length; y++)
            {
                if (_teleportLocationImages[x].Images[y] == null)
                    continue;

                if (_pylonManager.PylonArray[_teleportLocationImages[(int)_teleportIndex.x].Index[(int)_teleportIndex.y]].isOn)
                {
                    if (_teleportIndex == new Vector2(x, y))
                        _teleportLocationImages[x].Images[y].color = _color_currentlySelectedTeleport;
                    else
                        _teleportLocationImages[x].Images[y].color = _color_availableTeleport;
                }
                else
                    _teleportLocationImages[x].Images[y].color = _color_unavailableTeleport;
            }

        *//*for (int i = 0; i < _teleportLocationImages.Length; i++)
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

        _teleportLocationImages[_teleportIndex].color = _color_currentlySelectedTeleport;*//*
    }*/

    /// <summary>
    /// move teleport index
    /// </summary>
    public void UpdateArrayIndex(Vector2 amount)
    {
        if (TabletState)
        {
            _teleportIndex += amount;
            //Check if the new index is within the bounds of the array
            if (_teleportIndex.x >= _teleportLocationImages.Length)
                _teleportIndex.x = 0;
            if (_teleportIndex.x < 0)
                _teleportIndex.x = _teleportLocationImages.Length - 1;
            if (_teleportIndex.y >= _teleportLocationImages[(int)_teleportIndex.x].Images.Length)
                _teleportIndex.y = 0;
            if (_teleportIndex.y < 0)
                _teleportIndex.y = _teleportLocationImages[(int)_teleportIndex.x].Images.Length - 1;

            if (GetIndexAtPosition(_teleportIndex) == -1 || _pylonManager.PylonArray[GetIndexAtPosition(_teleportIndex)] == null)
            {
                UpdateArrayIndex(amount);
            }
            Update2DArrayVisuals();
        }
    }
    /*public void MoveTeleportIndex(Vector2 dir)
    {
        dir = new Vector2((int)dir.x, (int)dir.y);
        if (TabletState)
            if (TabIndex == MapTabIndex)
            {
                //reset inital index
                if (_pylonManager.PylonArray[(int)_teleportIndex.x].Pylons[(int)_teleportIndex.y].isOn)
                    _teleportLocationImages[(int)_teleportIndex.x].Images[(int)_teleportIndex.y].color = _color_availableTeleport;
                else
                    _teleportLocationImages[(int)_teleportIndex.x].Images[(int)_teleportIndex.y].color = _color_unavailableTeleport;

                //update index
                _teleportIndex += new Vector2(dir.x, -dir.y);
                ValidateIndexBounds();

                while (_teleportLocationImages[(int)_teleportIndex.x].Images[(int)_teleportIndex.y] == null || !_pylonManager.PylonArray[(int)_teleportIndex.x].Pylons[(int)_teleportIndex.y].isOn)
                {
                    _teleportIndex += new Vector2(dir.x, -dir.y);
                    ValidateIndexBounds();
                }


                //update new index
                _teleportLocationImages[(int)_teleportIndex.x].Images[(int)_teleportIndex.y].color = _color_currentlySelectedTeleport;

                //SetTeleportLocationColors();
            }
    }
    private void ValidateIndexBounds()
    {
        //validation check
        if (_teleportIndex.x >= _teleportLocationImages.Length)
            _teleportIndex.x = 0;
        if (_teleportIndex.x < 0)
            _teleportIndex.x = _teleportLocationImages.Length - 1;
        if (_teleportIndex.y >= _teleportLocationImages[(int)_teleportIndex.x].Images.Length)
            _teleportIndex.y = 0;
        if (_teleportIndex.y < 0)
            _teleportIndex.y = _teleportLocationImages[(int)_teleportIndex.x].Images.Length - 1;
    }*/

    /// <summary>
    /// move teleport index right and update colours
    /// </summary>
    /*public void MoveTeleportIndexRight()
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
    }*/

    /// <summary>
    /// select teleport location
    /// </summary>
    public void SelectTeleport()
    {
        if (TabletState)
            if (_pylonManager.PylonArray[GetIndexAtPosition()].isOn)
            {
                ToggleTablet();
                _pylonManager.GoToPylon(GetIndexAtPosition());
            }
    }
    /*
    public void SethasTeleportBeenActivated(int pylonIndex)
    {
        _hasTeleportLocationBeenActivated[pylonIndex] = true;
    }*/
    #endregion

    #region BackpackTab
    /*private void EmptySlots()
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
    }*/
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
