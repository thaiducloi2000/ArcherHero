using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_agent;
    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;

    public void SetDestination(Vector3 position)
    {
        m_StartPosition = transform.position;
        m_EndPosition = position;
        m_agent.SetDestination(position);
    }

    protected void Set(EnemyData data)
    {
        m_agent.speed = data.Speed;
        m_agent.angularSpeed = data.AngularSpeed;
        m_agent.acceleration = data.Acceleration;
    }

    public float PathProccess => 1 - (m_agent.remainingDistance / Vector3.Distance(m_StartPosition, m_EndPosition));
    public float RemainDistance => m_agent.remainingDistance;

    public bool Stop
    {
        get => m_agent.isStopped;
        set => m_agent.isStopped = value;
    }
}
