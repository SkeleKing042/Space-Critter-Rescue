//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;

public class SleepState : State
{
    public SleepState(CreatureAI AI) : base(AI)
    {

    }
    public override void StartState()
    {
        if (AIBrainReady())
        {
            Agent.isStopped = true;
            AI.Animator.SetBool("SleepState", true);
        }
    }
    public override void Update()
    {
        if (AIBrainReady())
            //Stop moving and replenish energy
            if (AI.Energy >= 100)
                AI.PrepareUpdateState(new IdleState(AI));
            else
                Mathf.Clamp(AI.Energy += Time.deltaTime * 10, 0, 100);
    }
    public override void EndState()
    {
        if (AIBrainReady())
        {
            Agent.isStopped = false;
            AI.Animator.SetBool("SleepState", false);
        }
    }
}
