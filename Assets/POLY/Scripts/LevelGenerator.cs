using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour {
    [SerializeField] GameObject layoutRoom;
    [SerializeField] Color startColor, endColor;
    [SerializeField] int distanceToEnd;
    [SerializeField] Transform generatorPoint;
    enum Direction { up, right, down, left }

    [SerializeField] Direction selectedDirection;
    [SerializeField] float xOffset = 18, yOffset = 10;
    [SerializeField] LayerMask whatIsRoom;

    GameObject endRoom;
    List<GameObject> layoutRoomObjects = new List<GameObject> ();

    public RoomPrefabs rooms;
    List<GameObject> generatedOutlines = new List<GameObject> ();

    [SerializeField] RoomCenter centerStart, centerEnd;
    [SerializeField] RoomCenter[] potentialCenters;
    private void Start () {
        Instantiate (layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer> ().color = startColor;
        selectedDirection = (Direction) Random.Range (0, 4);
        MoveGenerationPoint ();

        for (int i = 0; i < distanceToEnd; i++) {
            GameObject newRoom = Instantiate (layoutRoom, generatorPoint.position, generatorPoint.rotation);
            layoutRoomObjects.Add (newRoom);

            if (i + 1 == distanceToEnd) {
                newRoom.GetComponent<SpriteRenderer> ().color = endColor;
                endRoom = newRoom;
                layoutRoomObjects.RemoveAt (layoutRoomObjects.Count - 1);
            }

            selectedDirection = (Direction) Random.Range (0, 4);
            MoveGenerationPoint ();

            while (Physics2D.OverlapCircle (generatorPoint.position, 2f, whatIsRoom)) {
                MoveGenerationPoint ();
            }
        }

        //Создание стен
        CreateRoomOutline (Vector3.zero);
        foreach (GameObject room in layoutRoomObjects) {
            CreateRoomOutline (room.transform.position);
        }
        CreateRoomOutline (endRoom.transform.position);

        for (int outline = 0; outline < generatedOutlines.Count; outline++) {
            if (outline == 0) {
                Instantiate (centerStart, generatedOutlines[outline].transform.position, transform.rotation).theRoom = generatedOutlines[outline].GetComponent<Room> ();
            } else if (outline == generatedOutlines.Count - 1) {
                Instantiate (centerEnd, generatedOutlines[outline].transform.position, transform.rotation).theRoom = generatedOutlines[outline].GetComponent<Room> ();
            } else {
                int centerSelect = Random.Range (0, potentialCenters.Length);
                Instantiate (potentialCenters[centerSelect], generatedOutlines[outline].transform.position, transform.rotation).theRoom = generatedOutlines[outline].GetComponent<Room> ();
            }
            CreateGridGraph (generatedOutlines[outline].transform);
        }
    }

    private void Update () {

        if (Input.GetKeyDown (KeyCode.R)) {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }

    }

    public void MoveGenerationPoint () {
        switch (selectedDirection) {
            case Direction.up:
                generatorPoint.position += new Vector3 (0f, yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3 (xOffset, 0f, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3 (0f, -yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3 (-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline (Vector3 roomPosition) {
        bool roomAbove = Physics2D.OverlapCircle (roomPosition + new Vector3 (0f, yOffset, 0f), 2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle (roomPosition + new Vector3 (0f, -yOffset, 0f), 2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle (roomPosition + new Vector3 (-xOffset, 0f, 0f), 2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle (roomPosition + new Vector3 (xOffset, 0f, 0f), 2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove) directionCount++;
        if (roomBelow) directionCount++;
        if (roomLeft) directionCount++;
        if (roomRight) directionCount++;

        switch (directionCount) {
            case 0:
                Debug.LogError ("Found no room exists!");
                break;
            case 1:
                if (roomAbove) generatedOutlines.Add (Instantiate (rooms.singleUp, roomPosition, transform.rotation));
                if (roomBelow) generatedOutlines.Add (Instantiate (rooms.singleDown, roomPosition, transform.rotation));
                if (roomLeft) generatedOutlines.Add (Instantiate (rooms.singleLeft, roomPosition, transform.rotation));
                if (roomRight) generatedOutlines.Add (Instantiate (rooms.singleRight, roomPosition, transform.rotation));
                break;
            case 2:
                if (roomAbove && roomBelow) generatedOutlines.Add (Instantiate (rooms.doubleUpDown, roomPosition, transform.rotation));
                if (roomLeft && roomRight) generatedOutlines.Add (Instantiate (rooms.doubleLeftRight, roomPosition, transform.rotation));
                if (roomAbove && roomRight) generatedOutlines.Add (Instantiate (rooms.doubleUpRight, roomPosition, transform.rotation));
                if (roomRight && roomBelow) generatedOutlines.Add (Instantiate (rooms.doubleRightDown, roomPosition, transform.rotation));
                if (roomLeft && roomAbove) generatedOutlines.Add (Instantiate (rooms.doubleLeftUp, roomPosition, transform.rotation));
                if (roomLeft && roomBelow) generatedOutlines.Add (Instantiate (rooms.doubleLeftDown, roomPosition, transform.rotation));
                break;
            case 3:
                if (roomAbove && roomRight && roomBelow) generatedOutlines.Add (Instantiate (rooms.tripleUpRightDown, roomPosition, transform.rotation));
                if (roomRight && roomBelow && roomLeft) generatedOutlines.Add (Instantiate (rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                if (roomBelow && roomLeft && roomAbove) generatedOutlines.Add (Instantiate (rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                if (roomLeft && roomAbove && roomRight) generatedOutlines.Add (Instantiate (rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                break;
            case 4:
                generatedOutlines.Add (Instantiate (rooms.fourway, roomPosition, transform.rotation));
                break;
        }
    }

    void CreateGridGraph (Transform roomCenterTransform) {
        //Holds all graph data
        AstarData data = AstarPath.active.data;
        //Creates a Grid Graph
        GridGraph gridGraph = data.AddGraph (typeof (GridGraph)) as GridGraph;
        //Switch to 2D mode
        gridGraph.rotation = new Vector3 (-90, 270, 90);
        //Setup a grid graph with some values
        int width = 60;
        int depth = 32;
        float nodeSize = 0.5f;
        gridGraph.SetDimensions (width, depth, nodeSize);
        //Move Grid Graph
        gridGraph.center = roomCenterTransform.position;

        //Use 2D physics
        gridGraph.collision.use2D = true;
        //Diameter
        gridGraph.collision.diameter = 1.5f;
        //Setting obstacle layer mask
        gridGraph.collision.mask = LayerMask.GetMask ("Obstacle") |  LayerMask.GetMask ("IgnoreBullets");

        AstarPath.active.Scan ();
    }
}

[System.Serializable]
public class RoomPrefabs {
    public GameObject singleUp, singleRight, singleDown, singleLeft,
    doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleLeftUp, doubleLeftDown,
    tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
    fourway;
}