//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;

public class PanicState : State
{
    public PanicState(CreatureAI ai) : base(ai)
    {
    }
    public override void StartState()
    {
        if (AIBrainReady())
        {
            AI.GetAgent.enabled = true;
            Agent.isStopped = false;
            //AI.Animator.SetBool("PanicState", true);
            //Double movement speed
            Agent.speed = AI.BaseSpeed * AI.PanicSpeedIncrease;

            //Find place to move to
            Vector3 dir = (AI.transform.position - AI.Player.transform.position).normalized;
            Agent.SetDestination(AI.transform.position + dir * AI.GetComponent<FieldOfView>().Radius);
        }
    }
    public override void Update()
    {
        if (AIBrainReady())
        {
            //Lose energy twice as fast
            Mathf.Clamp(AI.Energy -= Time.deltaTime * 4, 0, 100);

            if (AI.GetComponent<FieldOfView>().CanSeeTarget)
            {
                Vector3 dir = (AI.transform.position - AI.Player.transform.position).normalized;
                Agent.SetDestination(AI.transform.position + dir * AI.GetComponent<FieldOfView>().Radius);
            }
            else if (Vector2.Distance(AI.transform.position, Agent.destination) < 1f)
                AI.PrepareUpdateState(new IdleState(AI));
        }
    }
    public override void EndState()
    {
        if (AIBrainReady())
        {
            //Return to normal speed
            Agent.speed = AI.BaseSpeed;
            //AI.Animator.SetBool("PanicState", false);
        }
    }

}
