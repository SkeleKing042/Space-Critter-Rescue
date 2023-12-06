//Created by Jackson Lucas
//Last Edited by Jackson Lucas

using System;
using UnityEngine;

public class StunnedState : State
{
    private float _stateTime;
    public StunnedState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        if (AIBrainReady())
        {
            //AI.Animator.SetBool("StunnedState", true);
            AI.RigidMode(true);
            AI.Rb.useGravity = true;
            AI.PrepareUpdateState(new IdleState(AI), 5);
            Failsafe();
        }
    }
    public override void Update()
    {

    }
    public override void EndState()
    {
        if (AIBrainReady())
        {
            //AI.Animator.SetBool("StunnedState", false);
            AI.RigidMode(false);
            AI.Rb.useGravity = false;
        }
    }
    private void Failsafe()
    {
        _stateTime += Time.deltaTime;
        if (_stateTime >= 2)
        {
            AI.StunThenRun(2);
        }
    }

}

