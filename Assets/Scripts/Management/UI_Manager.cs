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

    //THE SPRITES FOR THE VC
    #region Vacuum Catcher Variables
    [Header("Vacuum Catcher Variables")]
    [SerializeField] private Image _VC_ToolIcon;
    [SerializeField] private Image _VC_InputIcon;
    [SerializeField] private TextMeshProUGUI _VC_Text;
    [Space]

    [SerializeField, Tooltip("The sprite of the VC idle")]
    private Sprite _VCSpriteIdle;
    [SerializeField, Tooltip("The sprite of the VC idle")]
    private Sprite _VCSpriteSuck;
    [SerializeField, Tooltip("The sprite that shows the critters are going to be deposited in the ship")]
    private Sprite _ShipSprite;
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
        _UI_animator = GetComponent<Animator>();

        _fuelSliderDelta = _fuelSliderMax - _fuelSliderMin;

        ResetUI();
    }

    public void ResetUI()
    {
        SetJetpackUI();
        SetTabletToggleUI();
    }

    // Update is called once per frame
    void Update()
    {
        JetPackUI_Manager();
        //BackpackHUDManager();
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
