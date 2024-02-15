using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomScript : MonoBehaviour
{
    public GameObject boss;
    public GameObject room;
    public GameObject replacePrefab;
    public CameraController cameraController;

    void Update()
    {
        if (boss == null)
        {
            GameObject newRoomEpic = Instantiate(replacePrefab);
            newRoomEpic.transform.SetPositionAndRotation(room.transform.position, room.transform.rotation);
            cameraController.InitiateCameraShake(5);
            Destroy(room);
            Destroy(this.gameObject);
        }
    }
}
