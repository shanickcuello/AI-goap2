using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors;
    public List<GameObject> enemies;
    public LayerMask myLayermask;
    private void Start()
    {
      
        GetNeightbourd(Vector3.right);
        GetNeightbourd(Vector3.left);
        GetNeightbourd(Vector3.forward);
        GetNeightbourd(Vector3.back);
    }
    void GetNeightbourd(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, myLayermask))
        {
            var node = hit.collider.GetComponent<Node>();
            if (node != null)
            neighbors.Add(node);
        }

        
    }
}
