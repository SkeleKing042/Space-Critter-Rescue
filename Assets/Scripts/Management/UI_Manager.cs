using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Equipment;

public class UI_Manager : MonoBehaviour
{
    //VARIABLES
    #region Component Variables
    [Header("Components")]
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private Equipment _trapDeploy;
    [SerializeField]
    private Tablet _tablet;
    [SerializeField]
    private Inventory _inventory;
    [SerializeField]
    private Animator _UI_animator;
    #endregion

    #region Input Mode Variables and Enum
    [SerializeField]
    private InputMode inputMode;

    public enum InputMode
    {
        gamepad,
        keyboard,
    }

    #endregion

    #region UI State Variable and Enum
    [SerializeField, Tooltip("The current state the UI is in, used to define the current icon and inpute sprites")]
    private UIState _UIState;

    public enum UIState
    {
        VC_Suck_Trap,
        VC_Suck_Detonator,
        VC_DepositCritters_Trap,
        VC_DepositCritters_Detonator,
        Trap_ThrowTrap_VC,
        Detonator_ActivateTrap_VC,
        Detonator_PickUpTrap_VC,
    }



    #endregion

    #region Input Icons
    [Header("Input Icons")]

    //ARRAY INDEX LIST
    //0 FIRE
    //1 ALT FIRE
    //2 PICKUP TRAP
    //3 ENABLE TRAP
    //4 SWITCH TOOL
    //5 ALIEN DROP
    //6 TOGGLE TABLET

    [SerializeField]
    private Sprite[] _spritesGamepad;
    [SerializeField]
    private Sprite[] _spritesKeyboard;

    #endregion

    #region Backpack HUD Variables
    [Header("Backpack HUD")]
    [SerializeField, Tooltip("Slider that displays the amount of LC the player has collected")]
    private Image _LC_Slider;

    [SerializeField, Tooltip("Slider that displays the amount of SC the player has collected")]
    private Image _SC_Slider;
    #endregion

    #region Jetpack Variables
    [Header("Jetpack Variables")]
    [SerializeField, Tooltip("The display for the jet fuel.")]
    private Image _fuelBarMain;

    [Space]
    [SerializeField]
    private float _fuelSliderMax;
    [SerializeField]
    private float _fuelSliderMin;
    [SerializeField]
    private float _fuelSliderDelta;

    [Space]
    [SerializeField, Tooltip("The sprite for using the jetpack, gamepad")]
    private Sprite _jetpack_SpriteGamepad;
    [SerializeField, Tooltip("The sprite for using the jetpack, keyboard")]
    private Sprite _jetpack_SpriteKeyboard;

    [Space]

    [SerializeField, Tooltip("The image which displays the jetpack input")]
    private Image _jetpack_InputImage;

    [Space]
    [SerializeField, Tooltip("Image that covers the jetpack UI and shows if the jetpack is enabled / disabled")]
    private Image _jetpack_CoverImage;
    [SerializeField, Tooltip("The max opacity of the cover image")]
    private float _jetpack_CoverImage_OpacityMax;
    [SerializeField, Tooltip("The min opacity of the cover image")]
    private float _jetpack_CoverImage_OpacityMin;



    #endregion

    #region Vacuum Catcher Variables
    [Header("Vacuum Catcher Variables")]
    [SerializeField, Tooltip("The sprite of the VC")]
    private Sprite _VCSprite;
    [SerializeField, Tooltip("The sprite that shows the critters are going to be deposited in the ship")]
    private Sprite _ShipSprite;

    [Space]

    [SerializeField, Tooltip("The image that displays the Icon for the VC")]
    private Image VC_IconImage;

    [SerializeField, Tooltip("The image that displays the Input for the VC")]
    private Image VC_InputImage;

    #endregion

    #region Trap and Detonator Variables
    [Header("Trap and Detonator Variables")]
    [SerializeField, Tooltip("The sprite of the trap")]
    private Sprite _trapSprite;

    [SerializeField, Tooltip("The sprite of picking up the trap")]
    private Sprite _pickupTrapSprite;

    [SerializeField, Tooltip("The sprite of the detonator")]
    private Sprite _detonatorSprite;

    [Space]

    [SerializeField, Tooltip("The image that displays the icon of the trap or the detonator")]
    private Image Trap_IconImage;

    [SerializeField, Tooltip("The image that displays input for the trap / detonator")]
    private Image Trap_InputImage;
    #endregion

    #region Tablet Variables
    [Header("Tablet Variables")]
    [SerializeField, Tooltip("Tablet toggle sprite, gamepad")]
    private Sprite _toggleTabletIcon_Gamepad;

    [SerializeField, Tooltip("Tablet toggle sprite, keyboard")]
    private Sprite _toggleTabletIcon_Keyboard;

    [Space]

    [SerializeField, Tooltip("tablet input icon Image")]
    private Image _TabletToggleImage;

    #endregion region

    //METHODS
    #region Start and Update

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _trapDeploy = FindObjectOfType<Equipment>();
        _tablet = FindObjectOfType<Tablet>();
        _inventory = FindObjectOfType<Inventory>();
        _UI_animator = GetComponent<Animator>();

        _fuelSliderDelta = _fuelSliderMax - _fuelSliderMin;

        ResetUI();
    }

    private void ResetUI()
    {
        SetUIState(_UIState);
        SetJetpackUI();
        SetTabletToggleUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_tablet.TabletState)
        {
            JetPackUI_Manager();
            BackpackHUDManager();
        }
    }

    #endregion

    #region HUD Methods
    public void UpdateHUDAnimator()
    {
        if (_trapDeploy.currentlyHolding == Equipment.CurrentlyHolding.VC)
        {
            SetTrigger_UI_Vaccuum();
        }
        else
        {
            SetTrigger_UI_Trap();
        }
    }

    public void SetUIState(UIState _inputUIState)
    {
        _UIState = _inputUIState;

        switch (_UIState)
        {
            case UIState.VC_Suck_Trap:
                SetUI_VC_Suck_Trap();
                break;
            case UIState.VC_Suck_Detonator:
                SetUI_VC_Suck_Detonator();
                break;
            case UIState.VC_DepositCritters_Trap:
                SetUI_VC_DepositCritters_Trap();
                break;
            case UIState.VC_DepositCritters_Detonator:
                SetUI_VC_DepositCritters_Detonator();
                break;
            case UIState.Trap_ThrowTrap_VC:
                SetUI_Trap_ThrowTrap_VC();
                break;
            case UIState.Detonator_PickUpTrap_VC:
                SetUI_Detonator_PickUpTrap_VC();
                break;
            case UIState.Detonator_ActivateTrap_VC:
                SetUI_Detonator_ActivateTrap_VC();
                break;
        }
    }


    //Enlarges the UI for the Vaccuum Catcher
    public void SetTrigger_UI_Vaccuum()
    {
        _UI_animator.SetTrigger("UI_Vacuum");
    }

    //Enlarges the UI for the trap
    public void SetTrigger_UI_Trap()
    {
        _UI_animator.SetTrigger("UI_Trap");
    }

    //Sets UI
    //Holding VC, Succ, Swap to trap
    public void SetUI_VC_Suck_Trap()
    {
        //Icons
        VC_IconImage.sprite = _VCSprite;
        Trap_IconImage.sprite = _trapSprite;

        //Inputs
        //gamepad
        if(inputMode == InputMode.gamepad)
        {
            //fire
            VC_InputImage.sprite = _spritesGamepad[0];

            //switch
            Trap_InputImage.sprite = _spritesGamepad[4];
        }
        //keyboard
        else if(inputMode == InputMode.keyboard)
        {
            //fire
            VC_InputImage.sprite = _spritesKeyboard[0];

            //switch
            Trap_InputImage.sprite = _spritesKeyboard[4];
        }

        //trigger ui animator
        SetTrigger_UI_Vaccuum();
    }

    //Sets UI
    //Holding VC, Succ, Swap to Detonator 
    public void SetUI_VC_Suck_Detonator()
    {
        //Icons
        VC_IconImage.sprite = _VCSprite;
        Trap_IconImage.sprite = _detonatorSprite;

        //Inputs
        //gamepad
        if (inputMode == InputMode.gamepad)
        {
            //fire
            VC_InputImage.sprite = _spritesGamepad[0];

            //switch
            Trap_InputImage.sprite = _spritesGamepad[4];
        }
        //keyboard
        else if (inputMode == InputMode.keyboard)
        {
            //fire
            VC_InputImage.sprite = _spritesKeyboard[0];

            //switch
            Trap_InputImage.sprite = _spritesKeyboard[4];
        }

        //trigger ui animator
        SetTrigger_UI_Vaccuum();
    }

    //Sets UI
    //Holding VC, Deposit Critters, Swap to Trap
    public void SetUI_VC_DepositCritters_Trap()
    {
        //Icons
        VC_IconImage.sprite = _ShipSprite;
        Trap_IconImage.sprite = _trapSprite;

        //Inputs
        //gamepad
        if (inputMode == InputMode.gamepad)
        {
            //fire
            VC_InputImage.sprite = _spritesGamepad[5];

            //switch
            Trap_InputImage.sprite = _spritesGamepad[4];
        }
        //keyboard
        else if (inputMode == InputMode.keyboard)
        {
            //fire
            VC_InputImage.sprite = _spritesKeyboard[5];

            //switch
            Trap_InputImage.sprite = _spritesKeyboard[4];
        }

        //trigger ui animator
        SetTrigger_UI_Vaccuum();
    }

    //Sets UI
    //Holding VC, Deposit Critters, Swap to Trap
    public void SetUI_VC_DepositCritters_Detonator()
    {
        //Icons
        VC_IconImage.sprite = _ShipSprite;
        Trap_IconImage.sprite = _detonatorSprite;

        //Inputs
        //gamepad
        if (inputMode == InputMode.gamepad)
        {
            //fire
            VC_InputImage.sprite = _spritesGamepad[5];

            //switch
            Trap_InputImage.sprite = _spritesGamepad[4];
        }
        //keyboard
        else if (inputMode == InputMode.keyboard)
        {
            //fire
            VC_InputImage.sprite = _spritesKeyboard[5];

            //switch
            Trap_InputImage.sprite = _spritesKeyboard[4];
        }

        //trigger ui animator
        SetTrigger_UI_Vaccuum();
    }

    //Sets UI
    //Holding Trap, Swap to VC
    public void SetUI_Trap_ThrowTrap_VC()
    {
        //Icons
        VC_IconImage.sprite = _VCSprite;
        Trap_IconImage.sprite = _trapSprite;

        //Inputs
        //gamepad
        if (inputMode == InputMode.gamepad)
        {
            //fire
            VC_InputImage.sprite = _spritesGamepad[4];

            //switch
            Trap_InputImage.sprite = _spritesGamepad[0];
        }
        //keyboard
        else if (inputMode == InputMode.keyboard)
        {
            //fire
            VC_InputImage.sprite = _spritesKeyboard[4];

            //switch
            Trap_InputImage.sprite = _spritesKeyboard[0];
        }

        //trigger animator
        SetTrigger_UI_Trap();
    }

    //Sets UI
    //Holding Detonator, display activate input, Swap to VC
    public void SetUI_Detonator_ActivateTrap_VC()
    {
        //Icons
        VC_IconImage.sprite = _VCSprite;
        Trap_IconImage.sprite = _detonatorSprite;

        //Inputs
        //gamepad
        if (inputMode == InputMode.gamepad)
        {
            //swap
            VC_InputImage.sprite = _spritesGamepad[4];

            //enable
            Trap_InputImage.sprite = _spritesGamepad[3];
        }
        //keyboard
        else if (inputMode == InputMode.keyboard)
        {
            //swap
            VC_InputImage.sprite = _spritesKeyboard[4];

            //enable
            Trap_InputImage.sprite = _spritesKeyboard[3];
        }

        //trigger animator
        SetTrigger_UI_Trap();
    }

    //Sets UI, display pickup input, Swap to VC
    public void SetUI_Detonator_PickUpTrap_VC()
    {
        //Icons
        VC_IconImage.sprite = _VCSprite;
        Trap_IconImage.sprite = _pickupTrapSprite;

        //Inputs
        //gamepad
        if (inputMode == InputMode.gamepad)
        {
            //fire
            VC_InputImage.sprite = _spritesGamepad[4];

            //switch
            Trap_InputImage.sprite = _spritesGamepad[2];
        }
        //keyboard
        else if (inputMode == InputMode.keyboard)
        {
            //fire
            VC_InputImage.sprite = _spritesKeyboard[4];

            //switch
            Trap_InputImage.sprite = _spritesKeyboard[2];
        }

        //trigger animator
        SetTrigger_UI_Trap();
    }

    #endregion

    #region Jetpack Methods
    private void JetPackUI_Manager()
    {

        _fuelBarMain.fillAmount = (_fuelSliderDelta * _playerMovement.JetFuel) + _fuelSliderMin;



        /*if (!_playerMovement.GroundedCheck() || _playerMovement.JetFuel < 1)
        {
            //on
            _jetpackRectTransform.anchoredPosition = Vector2.Lerp(_jetpackRectTransform.anchoredPosition, new Vector2(_jetpack_Positions[0], 2), UI_speed * Time.deltaTime);

        }
        else
        {
            //off
            _jetpackRectTransform.anchoredPosition = Vector2.Lerp(_jetpackRectTransform.anchoredPosition, new Vector2(_jetpack_Positions[1], 2), UI_speed * Time.deltaTime);
        }

        //background color
        if (_playerMovement.RefuelTime > 0)
            _fuelBarBackground.color = _jetBackgroundColor[1];
        else
            _fuelBarBackground.color = _jetBackgroundColor[0];

        //set fill amount
        _fuelBarMain.fillAmount = _playerMovement.JetFuel;

        //set delay fill amount
        if (_delayedBar.fillAmount > _fuelBarMain.fillAmount)
            _delayedBar.fillAmount = iTween.FloatUpdate(_delayedBar.fillAmount, _fuelBarMain.fillAmount, 10);
        else
            _delayedBar.fillAmount = _fuelBarMain.fillAmount;*/
    }

    public void SetJetpackUI()
    {
        switch (inputMode)
        {
            case InputMode.gamepad:
                _jetpack_InputImage.sprite = _jetpack_SpriteGamepad;
                break;
            case InputMode.keyboard:
                _jetpack_InputImage.sprite = _jetpack_SpriteKeyboard;
                break;
        }
    }

    public void JetpackUI_Disable()
    {
        _jetpack_CoverImage.color = new Color(0, 0, 0, _jetpack_CoverImage_OpacityMax / 255);
    }

    public void JetpackUI_Enable()
    {
        _jetpack_CoverImage.color = new Color(0, 0, 0, _jetpack_CoverImage_OpacityMin/255);
    }

    #endregion

    #region Backpack HUD Methods
    private void BackpackHUDManager()
    {
        _LC_Slider.fillAmount = (float)_inventory.LargeCount / (float)_inventory.LargeCap;
        _SC_Slider.fillAmount = (float)_inventory.SmallCount / (float)_inventory.SmallCap;
    }
    #endregion

    #region Tablet Methods
    public void SetTabletToggleUI()
    {
        switch (inputMode)
        {
            case InputMode.gamepad:
                _TabletToggleImage.sprite = _toggleTabletIcon_Gamepad;
                break;
            case InputMode.keyboard:
                _TabletToggleImage.sprite = _toggleTabletIcon_Keyboard;
                break;
        }
    }


    #endregion

}
