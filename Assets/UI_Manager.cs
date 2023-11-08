using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _playerMovement;
    private TrapDeploy _trapDeploy;

    #region Jetpack Variables
    [Header("Fuel Display")]
    [SerializeField, Tooltip("The display for the jet fuel.")]
    private Image _fuelBarMain;
    [SerializeField]
    private Image _delayedBar;
    [SerializeField]
    private Image _fuelBarBackground;
    [Space]
    [SerializeField]
    private Color[] _jetBackgroundColor;

    [Space]
    [SerializeField]
    public bool isJetpackUI;
    [SerializeField]
    private RectTransform _jetpackRectTransform;
    [SerializeField]
    private float[] _jetpack_Positions;
    [SerializeField]
    private float UI_speed;

    #endregion

    #region Vacuum Catcher Variables
    [Header("Vacuum Catcher Variables")]
    [SerializeField]
    RectTransform _VC_RectTransform;
    [SerializeField] 
    float[] VC_Positions;

    #endregion


    #region Trap Variables
    [Header("Trap Variables")]
    [SerializeField]
    RectTransform _Trap_RectTransform;
    [SerializeField]
    float[] Trap_Positions;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _trapDeploy = GetComponent<TrapDeploy>();
    }

    // Update is called once per frame
    void Update()
    {
        JetPackUI_Manager();
        VacuumCatcherUI_Manager();
        TrapUI_Manager();
    }

    private void TrapUI_Manager()
    {
        //ON
        if (_trapDeploy.currentlyHolding == TrapDeploy.CurrentlyHolding.trap)
        {
            _Trap_RectTransform.anchoredPosition = Vector2.Lerp(_Trap_RectTransform.anchoredPosition, new Vector2(Trap_Positions[0], _Trap_RectTransform.anchoredPosition.y), UI_speed * Time.deltaTime);
        }
        //OFF
        else
        {
            _Trap_RectTransform.anchoredPosition = Vector2.Lerp(_Trap_RectTransform.anchoredPosition, new Vector2(Trap_Positions[1], _Trap_RectTransform.anchoredPosition.y), UI_speed * Time.deltaTime);
        }
    }


    private void VacuumCatcherUI_Manager()
    {
        //ON
        if(_trapDeploy.currentlyHolding == TrapDeploy.CurrentlyHolding.vacuum)
        {
            _VC_RectTransform.anchoredPosition = Vector2.Lerp(_VC_RectTransform.anchoredPosition, new Vector2(VC_Positions[0], _VC_RectTransform.anchoredPosition.y), UI_speed * Time.deltaTime);
        }
        //OFF
        else
        {
            _VC_RectTransform.anchoredPosition = Vector2.Lerp(_VC_RectTransform.anchoredPosition, new Vector2(VC_Positions[1], _VC_RectTransform.anchoredPosition.y), UI_speed * Time.deltaTime);
        }
    }


    private void JetPackUI_Manager()
    {
        if (isJetpackUI)
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
            _delayedBar.fillAmount = _fuelBarMain.fillAmount;
    }
}
