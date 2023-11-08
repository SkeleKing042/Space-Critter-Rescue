using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _playerMovement;

    [Header("Fuel Display")]
    [SerializeField, Tooltip("The display for the jet fuel.")]
    private Image _fuelBarMain;
    [SerializeField]
    private Image _delayedBar;
    [SerializeField]
    private Image _fuelBarBackground;
    [SerializeField]
    private Color[] _jetBackgroundColor;
    [SerializeField]
    public bool isJetpackUI;
    [SerializeField]
    private RectTransform jetpackUI;
    [SerializeField]
    private float[] jetpackU_Positions;
    [SerializeField]
    private float _jetpackUI_speed;

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isJetpackUI)
        {
            jetpackUI.anchoredPosition = iTween.Vector2Update(jetpackUI.anchoredPosition, new Vector2(jetpackU_Positions[0], jetpackUI.anchoredPosition.y), _jetpackUI_speed);
            Debug.Log("ON");
        }
        else
        {
            jetpackUI.anchoredPosition = iTween.Vector2Update(jetpackUI.anchoredPosition, new Vector2(jetpackU_Positions[1], jetpackUI.anchoredPosition.y), _jetpackUI_speed);
            Debug.Log("OFF");
        }





        //Always update the ui
        if (_playerMovement.RefuelTime > 0)
            _fuelBarBackground.color = _jetBackgroundColor[1];
        else
            _fuelBarBackground.color = _jetBackgroundColor[0];

        _fuelBarMain.fillAmount = _playerMovement.JetFuel;

        if (_delayedBar.fillAmount > _fuelBarMain.fillAmount)
            _delayedBar.fillAmount = iTween.FloatUpdate(_delayedBar.fillAmount, _fuelBarMain.fillAmount, 10);
        else
            _delayedBar.fillAmount = _fuelBarMain.fillAmount;
    }
}
