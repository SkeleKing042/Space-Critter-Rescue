//Created by Adanna Okoye
//Last Edited by Adanna Okoye
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManger : MonoBehaviour
{
    [SerializeField]
    //public  RumbleManger instance;

    private Gamepad _gamepad;

    // Start is called before the first frame update
    void Start()
    {
       // instance = this;
  
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Ruble frquency low is 1/4 and the high at 3/4
    /// </summary>
    /// <param name="LowFrequency"></param>
    /// <param name="HighFrequency"></param>
    /// <param name="Rumbletime"></param>
    public void RumbleStart(float LowFrequency, float HighFrequency, float Rumbletime)
    {
        _gamepad = Gamepad.current;
        if(_gamepad != null)
        {
            _gamepad.SetMotorSpeeds(LowFrequency, HighFrequency);
            StartCoroutine(ControllerRumble(Rumbletime));
            _gamepad.SetMotorSpeeds(0, 0);
        }

    }

    public IEnumerator ControllerRumble(float Rumbletime)
    {
      float time = 0f;
        while (time < Rumbletime)
        {
            time =+ Time.deltaTime;
            yield return null;
        }
        
    }

}
