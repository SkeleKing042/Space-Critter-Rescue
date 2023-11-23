
using UnityEngine.AI;

public abstract class State
{
    private CreatureAI _ai;
    private NavMeshAgent _agent;
    public State(CreatureAI ai)
    {
        _ai = ai;
        _agent = _ai.GetAgent;
    }
    public State(CreatureAI ai, NavMeshAgent agent)
    {
        _ai = ai;
        _agent = agent;
    }
    public CreatureAI AI { get { return _ai; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public abstract void StartState();
    public abstract void Update();
    public abstract void EndState();
    protected bool AIBrainReady()
    {
        if (_ai != null && _agent != null && _ai.isActiveAndEnabled && _agent.isActiveAndEnabled)
            return true;
        else return false;
    }
}

