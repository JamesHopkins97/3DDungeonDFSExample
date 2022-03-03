using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls; // 0 - down, 1 - up, 2 - left, 3 - right
    public GameObject[] doors;

    public void UpdateRoom(bool[] status)
    {
        for(int i = 0; i< status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}
