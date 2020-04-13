using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Search : IState
{
    public EnemyController owner;
    NavMeshAgent agent;

    public State_Search(EnemyController owner) { this.owner = owner; }

    public void Enter()
    {
        agent = owner.GetComponent<NavMeshAgent>();
        agent.acceleration = 8;
        agent.destination = owner.lastSeenPosition;
        agent.isStopped = false;
        
    }

    public void Execute()
    {
        Debug.Log("updating search state");

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            owner.stateMachine.ChangeState(new State_Patrol(owner));
        }

        if (owner.seenTarget)
        {
            owner.stateMachine.ChangeState(new State_Attack(owner));
        }
    }

    public void Exit()
    {
        Debug.Log("exiting patrol state"); // stop moving
        agent.isStopped = true;
    }
}
