//Created by Jackson Lucas
//Last edited by Jackson

using UnityEngine;

public class StruggleState : State
{
    public StruggleState(CreatureAI AI) : base(AI)
    {

    }
    public override void StartState()
    {
        if (AIBrainReady())
        {
            AI.Animator.SetBool("StuggleState", true);
            //Double movement speed
            Agent.speed = AI.BaseSpeed * 4f;
        }
    }
    public override void Update()
    {
        if (AIBrainReady())
        {
            //Lose energy twice as fast
            Mathf.Clamp(AI.Energy -= Time.deltaTime * 4, 0, 100);
            //Find a point away from the player relative to our current position
            Vector3 dir = (AI.Player.transform.position - AI.transform.position).normalized;
            if (Agent.isActiveAndEnabled && Agent != null)
                Agent.SetDestination(AI.Player.transform.position + dir * AI.GetComponent<FieldOfView>().Radius * -1.5f);

            //Once we're far enough away...
            if (Vector3.Distance(AI.transform.position, Agent.destination) < 2f)
            {
                //Go idle
                AI.PrepareUpdateState(new IdleState(AI));
                return;
            }
        }
    }
    public override void EndState()
    {
        //Return to base speed
        if (AIBrainReady())
        {
            Agent.speed = AI.BaseSpeed;
            AI.Animator.SetBool("StuggleState", false);
        }
    }
}
