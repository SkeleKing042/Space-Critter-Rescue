//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;

public class RoamingState : State
{
    public RoamingState(CreatureAI AI) : base(AI)
    {

    }
    public override void StartState()
    {
        Agent.isStopped = false;
        AI.Animator.SetBool("RoamingState", true);
    }
    public override void Update()
    {
        //Lose energy over time
        Mathf.Clamp(AI.Energy -= Time.deltaTime * 2, 0, 100);
        //Call checks
        if (AI.GetComponent<FieldOfView>().CanSeeTarget)
        {
            AI.PrepareUpdateState(new PanicState(AI));
            return;
        }
        if (AI.GoingForDrink)
            if (AI.POITarget.GetComponent<DrinkableSource>())
                //Are we at the drinking source
                if (Vector3.Distance(AI.transform.position, AI.POITarget.position) < AI.POITarget.GetComponent<DrinkableSource>().GetRadius())
                {
                    //... if so, drink
                    AI.PrepareUpdateState(new DrinkingState(AI));
                    return;
                }
                else if (AI.CheckForPOIs(1)) return;
        if (AI.CheckForSleep()) return;
        //Are we at the point?
        if (Vector3.Distance(AI.transform.position, Agent.destination) <= 1f)
        {
            //If so, stop
            AI.PrepareUpdateState(new IdleState(AI));
            return;
        }
        //Otherwise, keep moving
    }
    public override void EndState()
    {
        AI.Animator.SetBool("RoamingState", false);
    }
}
