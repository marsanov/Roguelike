using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField] Transform[] points;
    
    private void OnDrawGizmos() {
        for(int point = 1; point<points.Length; point++)
        {
            Gizmos.DrawLine(points[point-1].position, points[point].position);
        }
        Gizmos.DrawLine(points[points.Length-1].position, points[0].position);
    }
}
