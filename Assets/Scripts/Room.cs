using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered;
    [SerializeField] GameObject[] doors;

    [HideInInspector] public bool roomActive;

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //CameraController.instance.ChangeTarget(gameObject.transform);

            if (closeWhenEntered)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            roomActive = false;
        }
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
            closeWhenEntered = false;
        }
    }
}
