using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BomberAI : MonoBehaviour {
    private NavMeshAgent agent;
    public Transform[] targets;
    public GameObject bomb;

    public GameObject player;
    public float viewRange = 40f;
    public float time = 1f;
    private float actualTimer = 0f;

    private bool playerFound;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = GiveDestination();
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;

        if(actualTimer > 0) {
            actualTimer -= Time.deltaTime;
        }

        if (player) {
            Vector3 direction = (player.transform.position - transform.position).normalized;

            playerFound = false;

            if (Physics.Raycast(transform.position, direction, out hit, viewRange)) {
                if (hit.transform.gameObject == player) {
                    playerFound = true;
                    agent.destination = player.transform.position;
                    if(hit.distance <= 5f) {
                        if(actualTimer <= 0) {
                            ThrowBomb();
                            actualTimer = time;
                        }
                    }
                }
            }
        }
        if (IsArrived()) {
            agent.destination = GiveDestination();
        }
    }

    private Vector3 GiveDestination() {
        int index = Random.Range(0, targets.Length);
        return targets[index].position;
    }

    private bool IsArrived() {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    private void OnDrawGizmos() {
        Color color = Color.green;
        if (playerFound) {
            color = Color.red;
        }
        color.a = 0.5f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, viewRange);
    }

    private void ThrowBomb() {
        Vector3 position = transform.position + transform.forward * 0.5f;
        GameObject bombInstance = Instantiate(bomb, position, transform.rotation);
        bombInstance.GetComponent<Bomb>().player = player;
    }
}
