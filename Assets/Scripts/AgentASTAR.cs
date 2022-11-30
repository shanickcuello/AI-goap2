using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentASTAR : MonoBehaviour
{
    public float radius;
    public Vector3 offset;
    public Node starterNode;
    public Node endNode;

    public List<Node> _list;
   
    public ASTARMovement<Node> AstarMovement = new ASTARMovement<Node>();
 
    public void PathFindingA()
    {
        _list = AstarMovement.Run(starterNode, Satisfies, GetNeightAstar, Heuristic);
    }
  
    bool Satisfies(Node curr)
    {
        return curr.Equals(endNode);
    }
    float Heuristic(Node curr)
    {
        return Vector3.Distance(curr.transform.position, endNode.transform.position);
    }
    Dictionary<Node, float> GetNeightAstar(Node curr)
    {
        var dic = new Dictionary<Node, float>();

        foreach (var nodeNeigh in curr.neighbors)
        {
            var cost = Vector3.Distance(nodeNeigh.transform.position, curr.transform.position);
            dic.Add(nodeNeigh, cost);
        }
        return dic;
    }


    Dictionary<Node, float> GetNeighborsCost(Node curr)
    {
        var dic = new Dictionary<Node, float>();

        foreach (var nodeNeigh in curr.neighbors)
        {
            var cost = Vector3.Distance(nodeNeigh.transform.position, endNode.transform.position);
            dic.Add(nodeNeigh, cost);
        }
        return dic;
    }
    List<Node> Neighbors(Node curr)
    {
        var list = new List<Node>();
        foreach (var item in curr.neighbors)
        {
            list.Add(item);
        }
        return list;
    }
    Dictionary<Node, float> NeighboursDic(Node curr)
    {
        var dic = new Dictionary<Node, float>();
        foreach (var item in curr.neighbors)
        {
            dic.Add(item, Vector3.Distance(curr.transform.position, item.transform.position));
        }
        return dic;
    }
}
