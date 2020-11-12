using Elysium.AI.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float stoppingDistance = 0.01f;

    public bool Enabled { get; set; }

    private int node;
    private Vector3 target;
    private Vector3[] path;

    public void Init(Vector3[] _path)
    {
        this.path = _path;
        node = 0;
        target = path[node];
        Enabled = true;
    }

    private void Update()
    {
        if (!Enabled) { return; }                
        Move(target);
    }

    private void Move(Vector3 _destination)
    {
        var direction = _destination - transform.position;
        direction = direction.normalized;
        direction = transform.TransformVector(direction);
        Debug.DrawRay(transform.position, direction);
        transform.Translate(direction * Time.deltaTime * speed, Space.World);

        if (Vector3.Distance(transform.position, target) < stoppingDistance)
        {
            node++;

            if (node == path.Length)
            {
                // ARRIVED AT FINAL DESTINATION
                Enabled = false;
                return;
            }

            // ARRIVED AT NEXT NODE            
            target = path[node];
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target, 0.5f);

        for (int i = 0; i < path.Length; i++)
        {
            Vector3 currentNode = path[i];
            Vector3 previousNode = Vector3.zero;

            if (i > 0)
            {
                previousNode = path[i - 1];
            }
            else if (i == 0 && path.Length > 1)
            {
                previousNode = path[path.Length - 1];
            }

            if (i == 0)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.white;
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }
}
