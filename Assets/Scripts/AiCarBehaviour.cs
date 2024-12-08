using UnityEngine;
using UnityEngine.AI;

public class AiCarBehaviour : MonoBehaviour
{
    [SerializeField] private float carSpeed;
    [SerializeField] private float wayPointThreshold;

    Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        agent.speed = carSpeed;

        waypoints = GameManager.instance.carWayPoints;

        if (waypoints.Length != 0)
        {
            currentWaypointIndex = GetClosestWaypointIndex();

            // Set the first destination
            if (waypoints.Length > 0)
            {
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
        
    }

    void Update()
    {

        if (waypoints.Length != 0)
        {
            // Check if the agent is close to the current waypoint
            if (!agent.pathPending && Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) <= wayPointThreshold)
            {
                // Move to the next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }

    }

    private int GetClosestWaypointIndex()
    {
        int closestIndex = 0;
        float closestDistance = Mathf.Infinity;

        //Finding the closest Waypoint and it will start there

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
