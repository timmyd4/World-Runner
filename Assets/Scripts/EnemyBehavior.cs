using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public Transform target; // Set this to the player
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
            agent.SetDestination(target.position);
    }
}
