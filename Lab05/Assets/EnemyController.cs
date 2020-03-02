using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Waypoint waypoint;
    public Transform destination;
    public GameObject target;

    Vector3 lastSeenPosition;
    NavMeshAgent agent;
    SphereCollider sphereCollider;
    float fov = 110.0f;
    bool seenTarget;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.position;
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Waypoint nextWaypoint = waypoint.nextWaypoint;
            waypoint = nextWaypoint;
            agent.destination = waypoint.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == target)
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            seenTarget = false;

            RaycastHit hit;

            if (angle < fov * 0.5f)
            {
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, sphereCollider.radius))
                {
                    if (hit.collider.gameObject == target)
                    {
                        seenTarget = true;
                        lastSeenPosition = target.transform.position;
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (sphereCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
            if (seenTarget) Gizmos.DrawLine(transform.position, lastSeenPosition);
            if (lastSeenPosition != Vector3.zero) Debug.Log("draw smol sfir"); // draw small sphere

            Vector3 rightPeripheral = (Quaternion.AngleAxis(fov * 0.5f, Vector3.up) * transform.forward * sphereCollider.radius);
            Vector3 leftPeripheral = (Quaternion.AngleAxis(fov * 0.5f, Vector3.down) * transform.forward * sphereCollider.radius);

            Gizmos.DrawLine(transform.position, rightPeripheral);
            Gizmos.DrawLine(transform.position, leftPeripheral);
        }
    }

}
