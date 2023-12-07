using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrappedState : State
{
    public TrappedState(CreatureAI AI) : base(AI)
    {

    }
    public override void StartState()
    {
        if (AIBrainReady())
        {
            AI.GetAgent.isStopped = true;
        }
    }
    public override void Update()
    {

    }
    public override void EndState()
    {
        if (AIBrainReady())
        {
            AI.GetAgent.isStopped = false;
        }
    }

}
