//Created by Ru McPharlin
//Last Edited by Jackson Lucas

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    //VARIABLES
    #region Component Variables
    [Header("Components")]
    private PlayerMovement _playerMovement;
    private Equipment _equipment;
    private Tablet _tablet;
    private Inventory _inventory;
    private Animator _UI_animator;
    #endregion

    //VARIABLES THAT CONTROL WHAT ICONS ARE DISPLAYED
    #region Input Mode Variables and Enum
    [SerializeField]
    private InputMode inputMode;
    public enum InputMode
    {
        gamepad,
        keyboard,
    }

    #endregion

    //VARIABLES THAT CONTROL WHAT THE UI STATE IS
    #region UI State Variable and Enum
    [SerializeField, Tooltip("The current state the UI is in, used to define the current icon and inpute sprites")]
    private UIState _UIState;
    public UIState Get_UIState { get { return _UIState; } }

    public enum UIState
    {
        VC_Trap,
        VC_Detonator,
        Trap_VC,
        Detonator_VC,
    }

    [SerializeField]
    public BackpackState _backpackState;

    public enum BackpackState
    {
        notFull,
        Full,
        DepositCritters
    }
    #endregion

    //THE INPUT ICONS
    #region Input Icons
    [Header("Input Icons and Text")]

    //ARRAY INDEX LIST
    //0 FIRE
    //1 THROW TRAP
    //2 PICKUP TRAP
    //3 ENABLE TRAP
    //4 SWITCH TOOL
    //5 ALIEN DROP
    //6 TOGGLE TABLET

    [SerializeField]
    private Sprite[] _spritesGamepad;
    [SerializeField]
    private Sprite[] _spritesKeyboard;

    [SerializeField]
    private string[] _inputStrings;

    #endregion

    //THE REFERENCES TO THE SLOTS ICON, INPUT AND TEXT
    #region Slots
    [SerializeField] private Image _Slot1_ToolIcon;
    [SerializeField] private Image _Slot1_InputIcon;
    [SerializeField] private TextMeshProUGUI _Slot1_Text;
    [Space]

    [SerializeField] private Image _Slot2_ToolIcon;
    [SerializeField] private Image _Slot2_InputIcon;
    [SerializeField] private TextMeshProUGUI _Slot2_Text;
    [Space]

    [SerializeField] private Image _Slot3_ToolIcon;
    [SerializeField] private Image _Slot3_InputIcon;
    [SerializeField] private TextMeshProUGUI _Slot3_Text;
    [Space]

    #endregion

    //THE SPRITES FOR THE VC
    #region Vacuum Catcher Variables
    [Header("Vacuum Catcher Variables")]
    [SerializeField, Tooltip("The sprite of the VC idle")]
    private Sprite _VCSpriteIdle;
    [SerializeField, Tooltip("The sprite of the VC idle")]
    private Sprite _VCSpriteSuck;
    [SerializeField, Tooltip("The sprite that shows the critters are going to be deposited in the ship")]
    private Sprite _ShipSprite;
    #endregion

    //THE SPRITES FOR THE TRAP
    #region Trap Variables
    [Header("Trap Variables")]
    [SerializeField, Tooltip("The sprite of the trap")]
    private Sprite _trapSpriteIdle;
    [SerializeField, Tooltip("The sprite of the trap being picked up")]
    private Sprite _trapPickUpSprite;
    [SerializeField, Tooltip("The sprite of the trap being picked up")]
    private Sprite _trapThrowSprite;
    #endregion

    //THE SPRITES FOR THE DEONTATOR
    #region Detonator Variables
    [Header("Detonator Variables")]
    [SerializeField, Tooltip("The sprite of the detonator")]
    private Sprite _detonatorSpriteIdle;
    [SerializeField, Tooltip("The sprite of the detonator")]
    private Sprite _detonatorSpriteActivate;
    #endregion

    //THE SPRITES FOR THE TABLET
    #region Tablet Variables
    [Header("Tablet Variables")]
    [SerializeField, Tooltip("Tablet toggle sprite, gamepad")]
    private Sprite _toggleTabletIcon_Gamepad;

    [SerializeField, Tooltip("Tablet toggle sprite, keyboard")]
    private Sprite _toggleTabletIcon_Keyboard;

    [Space]

    [SerializeField, Tooltip("tablet input icon Image")]
    private Image _TabletToggleIcon;
    [SerializeField, Tooltip("tablet input text")]
    private TextMeshProUGUI _TabletToggleText;

    #endregion region

    //BACKPACK VARIABLES
    #region Backpack HUD Variables
    

    

    [Header("Backpack HUD")]
    [SerializeField] private Image _backpackIconImage;
    [SerializeField, Tooltip("Backpack sprite, not full")]
    private Sprite _backpackSprite_NotFull;
    [SerializeField, Tooltip("Backpack sprite, full")]
    private Sprite _backpackSprite_Full;
    [SerializeField, Tooltip("Backpack sprite, deposit")]
    private Sprite _backpackSprite_Deposit;

    [SerializeField, Tooltip("Slider that displays the amount of LC the player has collected")]
    private Image _LC_Slider;

    [SerializeField, Tooltip("Slider that displays the amount of SC the player has collected")]
    private Image _SC_Slider;
    #endregion

    //JETPACK VARIABLES
    #region Jetpack Variables
    [Header("Jetpack Variables")]
    [SerializeField]
    private Image _jetpackIconImage;
    [SerializeField, Tooltip("The sprite for the idle jetpack sprite")]
    private Sprite _jetpackSpriteIdle;
    [SerializeField, Tooltip("The sprite for the idle jetpack sprite")]
    private Sprite _jetpackSpriteActive;

    [Space]
    [SerializeField, Tooltip("The image which displays the jetpack input")]
    private Image _jetpack_InputImage;
    [SerializeField, Tooltip("The sprite for using the jetpack, gamepad")]
    private Sprite _jetpack_SpriteGamepad;
    [SerializeField, Tooltip("The sprite for using the jetpack, keyboard")]
    private Sprite _jetpack_SpriteKeyboard;

    [Space]
    [SerializeField, Tooltip("The display for the jet fuel.")]
    private Image _fuelBarMain;
    [SerializeField]
    private float _fuelSliderMax;
    [SerializeField]
    private float _fuelSliderMin;
    [SerializeField]
    private float _fuelSliderDelta;


    #endregion

    //METHODS
    #region Start and Update

    // Start is called before the first frame update
    void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _equipment = FindObjectOfType<Equipment>();
        _tablet = FindObjectOfType<Tablet>();
        _inventory = FindObjectOfType<Inventory>();
        _UI_animator = GetComponent<Animator>();

        _fuelSliderDelta = _fuelSliderMax - _fuelSliderMin;

        ResetUI();
    }

    public void ResetUI()
    {
        SetUIState(_UIState);
        SetJetpackUI();
        SetTabletToggleUI();
    }

    // Update is called once per frame
    void Update()
    {
        JetPackUI_Manager();
        BackpackHUDManager();
    }

    #endregion

    #region HUD Methods

    public void SetUIState(UIState _inputUIState)
    {
        _UIState = _inputUIState;

        switch (_UIState)
        {
            case UIState.VC_Trap:
                SetUI_VC_Trap();
                break;
            case UIState.VC_Detonator:
                SetUI_VC_Detonator();
                break;
            case UIState.Trap_VC:
                SetUI_Trap_VC();
                break;
            case UIState.Detonator_VC:
                SetUI_Detonator_VC();
                break;
        }
    }

    //tested
    public void SetUI_VC_Trap()
    {
        //text
        _Slot1_Text.text = "Swap";
        _Slot2_Text.text = "Catch";
        _Slot3_Text.text = "Deposit";
        _TabletToggleText.text = "On";

        //input icons
        switch (inputMode)
        {
            case InputMode.gamepad:
                _Slot1_InputIcon.sprite = _spritesGamepad[4];
                _Slot2_InputIcon.sprite = _spritesGamepad[0];
                _Slot3_InputIcon.sprite = _spritesGamepad[5];
                break;
            case InputMode.keyboard:
                _Slot1_InputIcon.sprite = _spritesKeyboard[4];
                _Slot2_InputIcon.sprite = _spritesKeyboard[0];
                _Slot3_InputIcon.sprite = _spritesKeyboard[5];
                break;
        }

        //tool icons
        _Slot1_ToolIcon.sprite = _trapSpriteIdle;
        _Slot2_ToolIcon.sprite = _VCSpriteSuck;
        _Slot3_ToolIcon.sprite = _ShipSprite;
    }

    //tested
    public void SetUI_VC_Detonator()
    {
        //text
        _Slot1_Text.text = "Swap";
        _Slot2_Text.text = "Catch";
        _Slot3_Text.text = "Deposit";
        _TabletToggleText.text = "On";

        //input icons
        switch (inputMode)
        {
            case InputMode.gamepad:
                _Slot1_InputIcon.sprite = _spritesGamepad[4];
                _Slot2_InputIcon.sprite = _spritesGamepad[0];
                _Slot3_InputIcon.sprite = _spritesGamepad[5];
                break;
            case InputMode.keyboard:
                _Slot1_InputIcon.sprite = _spritesKeyboard[4];
                _Slot2_InputIcon.sprite = _spritesKeyboard[0];
                _Slot3_InputIcon.sprite = _spritesKeyboard[5];
                break;
        }

        //tool icons
        _Slot1_ToolIcon.sprite = _detonatorSpriteIdle;
        _Slot2_ToolIcon.sprite = _VCSpriteSuck;
        _Slot3_ToolIcon.sprite = _ShipSprite;
    }

    //tested
    public void SetUI_Detonator_VC()
    {
        //text
        _Slot1_Text.text = "Swap";
        _Slot2_Text.text = "Activate";
        _Slot3_Text.text = "Pick-Up";
        _TabletToggleText.text = "On";

        //input icons
        switch (inputMode)
        {
            case InputMode.gamepad:
                _Slot1_InputIcon.sprite = _spritesGamepad[4];
                _Slot2_InputIcon.sprite = _spritesGamepad[2];
                _Slot3_InputIcon.sprite = _spritesGamepad[3];
                break;
            case InputMode.keyboard:
                _Slot1_InputIcon.sprite = _spritesKeyboard[4];
                _Slot2_InputIcon.sprite = _spritesKeyboard[2];
                _Slot3_InputIcon.sprite = _spritesKeyboard[3];
                break;
        }

        //tool icons
        _Slot1_ToolIcon.sprite = _VCSpriteIdle;
        _Slot2_ToolIcon.sprite = _detonatorSpriteActivate;
        _Slot3_ToolIcon.sprite = _trapPickUpSprite;
    }

    //tested
    public void SetUI_Trap_VC()
    {
        //text
        _Slot1_Text.text = "Swap";
        _Slot2_Text.text = "Throw";
        _Slot3_Text.text = "Throw";
        _TabletToggleText.text = "On";

        //input icons
        switch (inputMode)
        {
            case InputMode.gamepad:
                _Slot1_InputIcon.sprite = _spritesGamepad[4];
                _Slot2_InputIcon.sprite = _spritesGamepad[1];
                _Slot3_InputIcon.sprite = _spritesGamepad[1];
                break;
            case InputMode.keyboard:
                _Slot1_InputIcon.sprite = _spritesKeyboard[4];
                _Slot2_InputIcon.sprite = _spritesKeyboard[1];
                _Slot3_InputIcon.sprite = _spritesKeyboard[1];
                break;
        }

        //tool icons
        _Slot1_ToolIcon.sprite = _VCSpriteIdle;
        _Slot2_ToolIcon.sprite = _trapThrowSprite;
        _Slot3_ToolIcon.sprite = _trapThrowSprite;
    }

    #endregion

    #region Jetpack Methods
    private void JetPackUI_Manager()
    {

        _fuelBarMain.fillAmount = (_fuelSliderDelta * _playerMovement.JetFuel) + _fuelSliderMin;

        if(!_playerMovement.GroundedCheck())
        {
            _jetpackIconImage.sprite = _jetpackSpriteActive;
        }
        else
        {
            _jetpackIconImage.sprite = _jetpackSpriteIdle;
        }

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
    #endregion

    #region Backpack HUD Methods
    private void BackpackHUDManager()
    {
        _LC_Slider.fillAmount = (float)_inventory.LargeCount / (float)_inventory.LargeCap;
        _SC_Slider.fillAmount = (float)_inventory.SmallCount / (float)_inventory.SmallCap;

        switch (_backpackState)
        {
            case BackpackState.notFull:
                _backpackIconImage.sprite = _backpackSprite_NotFull;
                break;
            case BackpackState.Full:
                _backpackIconImage.sprite = _backpackSprite_Full;
                break;
            case BackpackState.DepositCritters:
                _backpackIconImage.sprite = _backpackSprite_Deposit;
                break;
        }
    }
    #endregion

    #region Tablet Methods
    public void SetTabletToggleUI()
    {

        switch (_UI_animator.GetBool("UI_TabletState"))
        {
            case true:
                _TabletToggleText.text = "Off";
                break;
            case false:
                _TabletToggleText.text = "On";
                break;
        }        

        switch (inputMode)
        {
            case InputMode.gamepad:
                _TabletToggleIcon.sprite = _toggleTabletIcon_Gamepad;
                break;
            case InputMode.keyboard:
                _TabletToggleIcon.sprite = _toggleTabletIcon_Keyboard;
                break;
        }
    }




    #endregion
}
