using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Put enemies into the object you put into "epicreveal"

    public GameObject epicreveal;
    private void Start()
    {
        Debug.Log("set unactive");
        epicreveal.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("set active");
        epicreveal.SetActive(true);
    }
}
