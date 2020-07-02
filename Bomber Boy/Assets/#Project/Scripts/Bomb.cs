using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float range = 5f;
    public float timer = 3f;
    public float force = 500f;

    public GameObject[] walls;
    public GameObject player;
    public float firstImpulse=100f;

    // Start is called before the first frame update
    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * firstImpulse);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        if(timer <= 0) {
            if (player) {
                if (Vector3.Distance(player.transform.position, transform.position) <= range) {
                    Destroy(player);
                }
            }
            foreach (GameObject wall in walls) {
                Rigidbody rb = wall.GetComponent<Rigidbody>();
                rb.AddExplosionForce(force, transform.position, range);
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() {
        Color color = new Color(1f, 0.5f, 0.25f, 0.75f);
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position, range);
    }
}
