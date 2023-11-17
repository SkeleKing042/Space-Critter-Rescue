using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField]
    private PlayerMovement _playerMovement;
    private TrapDeploy _trapDeploy;
    private Tablet _tablet;
    private PlayerInventory _playerInventory;


    [Header("Universal Variables")]
    [SerializeField]
    private float UI_speed;
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

    #region Backpack HUD
    [SerializeField, Tooltip("Slider that displays the amount of LC the player has collected")]
    private Image _LC_Slider;
    [SerializeField, Tooltip("Max")]
    private 
    [SerializeField, Tooltip("Slider that displays the amount of SC the player has collected")]
    private Image _SC_Slider;





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
        _playerMovement = GetComponent<PlayerMovement>();
        _trapDeploy = GetComponent<TrapDeploy>();
        _tablet = GetComponentInChildren<Tablet>();

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
        /*//ON
        if (_trapDeploy.currentlyHolding == TrapDeploy.CurrentlyHolding.trap || _trapDeploy.TrapDeployed == true)
        {
            _Trap_RectTransform.anchoredPosition = Vector2.Lerp(_Trap_RectTransform.anchoredPosition, new Vector2(Trap_Positions[0], _Trap_RectTransform.anchoredPosition.y), UI_speed * Time.deltaTime);
            Trap_InputIcon.sprite = inputIcon_RT;
        }
        //OFF
        else
        {
            _Trap_RectTransform.anchoredPosition = Vector2.Lerp(_Trap_RectTransform.anchoredPosition, new Vector2(Trap_Positions[1], _Trap_RectTransform.anchoredPosition.y), UI_speed * Time.deltaTime);
            Trap_InputIcon.sprite = inputIcon_LB;
        }*/

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

    }
}
