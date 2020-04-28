using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour {
    // [SerializeField] bool openWhenEnemiesCleared;
    // [SerializeField] List<GameObject> enemies = new List<GameObject> ();
    [SerializeField] AstarPath astarPath;

    [HideInInspector] public Room theRoom;

    // Start is called before the first frame update
    void Start () {
        if (astarPath != null) {
            astarPath.data.gridGraph.center = transform.position;
            astarPath.Scan ();
        }
    }

    // Update is called once per frame
    void Update () {
        // if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
        // {
        //     for (int i = 0; i < enemies.Count; i++)
        //     {
        //         if (enemies[i] == null)
        //         {
        //             enemies.RemoveAt(i);
        //             i--;
        //         }
        //     }
        // }
        // if (enemies.Count == 0)
        // {
        //     theRoom.OpenDoors();
        // }
    }
}