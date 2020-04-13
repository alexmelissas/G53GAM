using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Waypoint waypoint;
    public Transform destination;
    public GameObject target;
    public StateMachine stateMachine = new StateMachine();

    public GameObject bullet;
    public Transform bulletTransform;

    public float firerate = 0.5f;
    private float nextFire = 0.0f;

    public Vector3 lastSeenPosition;
    public bool seenTarget;
    NavMeshAgent agent;

    SphereCollider sphereCollider;
    float fov = 110.0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.position;
        sphereCollider = GetComponent<SphereCollider>();
        stateMachine.ChangeState(new State_Patrol(this));
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Waypoint nextWaypoint = waypoint.nextWaypoint;
            waypoint = nextWaypoint;
            agent.destination = waypoint.transform.position;
        }
        stateMachine.Update();
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
                        Debug.Log("Saw player");
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
            if (lastSeenPosition != Vector3.zero) Gizmos.DrawSphere(lastSeenPosition, 0.5f);

            Vector3 rightPeripheral = (Quaternion.AngleAxis(fov * 0.5f, Vector3.up) * transform.forward * sphereCollider.radius);
            Vector3 leftPeripheral = (Quaternion.AngleAxis(-(fov * 0.5f), Vector3.up) * transform.forward * sphereCollider.radius);

            Gizmos.DrawRay(transform.position, rightPeripheral);
            Gizmos.DrawRay(transform.position, leftPeripheral);
        }
    }

    public void FireOnPlayer()
    {
        nextFire = Time.time + firerate;
        Instantiate(bullet,
            bulletTransform.position,
            Camera.main.transform.rotation);
        Debug.Log("FIRING ON PLAYER");
    }

}
