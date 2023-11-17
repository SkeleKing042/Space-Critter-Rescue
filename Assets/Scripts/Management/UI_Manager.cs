using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TrapDeploy;

public class UI_Manager : MonoBehaviour
{
    #region Components
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


    [Header("Universal Variables")]
    [SerializeField]
    private Sprite inputIcon_LT;
    [SerializeField]
    private Sprite inputIcon_RT;
    [SerializeField]
    private Sprite inputIcon_LB;
    [SerializeField]
    private Sprite inputIcon_RB;
    [SerializeField]
    private Sprite inputIcon_Y;
    [SerializeField]
    private Sprite inputIcon_X;

    

    #region Backpack HUD
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
    [SerializeField]
    Image VC_InputIcon;

    #endregion

    #region Trap Variables
    [Header("Trap Variables")]
    [SerializeField]
    Image Trap_InputIcon;
    #endregion

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
            VacuumCatcherUI_Manager();
            TrapUI_Manager();
            BackpackHUDManager();
        }
        else
        {

        }
    }




    private void TrapUI_Manager()
    {
        //pickup trap / activate trap
        if(_trapDeploy.TrapDeployed == true)
        {
            if(_trapDeploy.CanPickUpTrap == false)
            {
                Trap_InputIcon.sprite = inputIcon_Y;
            }
            else
            {
                Trap_InputIcon.sprite = inputIcon_X;
            }
        }
    }


    private void VacuumCatcherUI_Manager()
    {
        //ON
        if(_trapDeploy.currentlyHolding == TrapDeploy.CurrentlyHolding.vacuum)
        {
            VC_InputIcon.sprite = inputIcon_RT;
        }
        //OFF
        else
        {
            VC_InputIcon.sprite = inputIcon_LB;
        }
    }


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

    private void BackpackHUDManager()
    {
        _LC_Slider.fillAmount = (float)_inventory.LargeCount / (float)_inventory.LargeCap;
        _SC_Slider.fillAmount = (float)_inventory.SmallCount / (float)_inventory.SmallCap;
    }

    public void UpdateHUD()
    {
        if (_trapDeploy.currentlyHolding == TrapDeploy.CurrentlyHolding.vacuum)
        {
            _UI_animator.SetTrigger("UI_Vacuum");
        }
        else
        {
            _UI_animator.SetTrigger("UI_Trap");
        }
    }

}
