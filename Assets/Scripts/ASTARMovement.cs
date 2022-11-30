﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASTARMovement<T>
{
    public delegate Dictionary<T, float> GetNeighbours(T curr);//
    public delegate bool Satisfies(T curr);
    public delegate float Heuristic(T curr);
    public List<T> Run(T start, Satisfies satisfies, GetNeighbours getNeighbourds, Heuristic heuristic)
    {
        PriorityQueueMovement<T> pending = new PriorityQueueMovement<T>();
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T, T> parents = new Dictionary<T, T>();
        Dictionary<T, float> costs = new Dictionary<T, float>();//
        pending.Enqueue(start, 0);
        costs.Add(start, 0);
        while (!pending.IsEmpty)
        {
            T current = pending.Dequeue();
            if (satisfies(current))
            {
                return ContructPath(current, parents);
            }
            visited.Add(current);
            Dictionary<T, float> neighbours = getNeighbourds(current);//
            foreach (var item in neighbours)
            {
                var currNeigh = item.Key;
                var currNeighCost = item.Value;
                if (visited.Contains(currNeigh)) continue;
                if (costs.ContainsKey(currNeigh)) continue;//
                var tentativeCost = currNeighCost + costs[current];//
                parents.Add(currNeigh, current);
                pending.Enqueue(currNeigh, tentativeCost + heuristic(currNeigh));//
                costs.Add(currNeigh, tentativeCost);//
            }
        }
        return null;
    }
    List<T> ContructPath(T end, Dictionary<T, T> parents)
    {
        var path = new List<T>();
        path.Add(end);
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            var lastNode = path[path.Count - 1];
            path.Add(parents[lastNode]);
        }
        path.Reverse();
        return path;
    }
}
