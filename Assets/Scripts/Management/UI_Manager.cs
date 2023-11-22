using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TrapDeploy;

public class UI_Manager : MonoBehaviour
{
    //VARIABLES
    #region Component Variables
    [Header("Components")]
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private TrapDeploy _trapDeploy;
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


    //METHODS
    #region Start and Update

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _trapDeploy = FindObjectOfType<TrapDeploy>();
        _tablet = FindObjectOfType<Tablet>();
        _inventory = FindObjectOfType<Inventory>();
        _UI_animator = GetComponent<Animator>();

        _fuelSliderDelta = _fuelSliderMax - _fuelSliderMin;



    }

    // Update is called once per frame
    void Update()
    {
        if (!_tablet.TabletState)
        {
            JetPackUI_Manager();
            BackpackHUDManager();
        }
        else
        {

        }
    }

    #endregion

    #region HUD Methods
    public void UpdateHUD()
    {
        if (_trapDeploy.currentlyHolding == TrapDeploy.CurrentlyHolding.vacuum)
        {
            SetTrigger_UI_Vaccuum();
        }
        else
        {
            SetTrigger_UI_Trap();
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
    }

    //Sets UI
    //Holding VC, Deposit Critters, Swap to Trap
    public void SetUI_VC_Deposit_Trap()
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
    }

    //Sets UI
    //Holding VC, Deposit Critters, Swap to Trap
    public void SetUI_VC_Deposit_Detonator()
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
    }

    //Sets UI
    //Holding Trap, Swap to VC
    public void SetUI_Trap_Throw()
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
    }

    //Sets UI
    //Holding Detonator, display activate input, Swap to VC
    public void SetUI_Detonator_Activate()
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
    }

    //Sets UI, display pickup input, Swap to VC
    public void SetUI_Detonator_PickUpTrap()
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
    #endregion

    #region Backpack HUD
    private void BackpackHUDManager()
    {
        _LC_Slider.fillAmount = (float)_inventory.LargeCount / (float)_inventory.LargeCap;
        _SC_Slider.fillAmount = (float)_inventory.SmallCount / (float)_inventory.SmallCap;
    }
    #endregion

    

}
