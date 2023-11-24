//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;
public class IdleState : State
{
    public IdleState(CreatureAI ai) : base(ai)
    {

    }
    public override void StartState()
    {
        if (AIBrainReady())
        {
            //AI.Animator.SetBool("IdleState", true);
            AI.RigidMode(false);
        }
    }
    public override void Update()
    {
        if (AIBrainReady())
        {
            //Call check
            if (AI.GetComponent<FieldOfView>().CanSeeTarget)
            {
                AI.RunFromPlayer(1);
                return;
            }
            if (AI.CheckForPOIs(1)) return;
            if (AI.CheckForSleep()) return;
            else
            {
                //Choose a random spot
                float n = AI.TravelDistance;
                Vector3 newPos = new Vector3(AI.HomePoint.x + UnityEngine.Random.Range(-n, n), AI.HomePoint.y + UnityEngine.Random.Range(-n, n), AI.HomePoint.z + UnityEngine.Random.Range(-n, n));
                //Move there
                Agent.SetDestination(newPos);
                //Start roaming
                AI.PrepareUpdateState(new RoamingState(AI));
                return;
            }
        }
    }
    public override void EndState()
    {
        //if(AIBrainReady())
        //AI.Animator.SetBool("IdleState", false);
    }
}
