using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomScript : MonoBehaviour
{
    public GameObject boss;
    public GameObject room;
    public Color color;
    public GameObject replacePrefab;
    public CameraController cameraController;
	
	bool setup = true;

    void Update()
    {
        if (boss == null && setup)
        {
			setup = false;
            GameObject newRoomEpic = Instantiate(replacePrefab);
            newRoomEpic.transform.SetPositionAndRotation(new(0,0,0), Quaternion.identity);
            newRoomEpic.GetComponent<SpriteRenderer>().color = color;
            cameraController.shakeFalloff = 0.95f;
            cameraController.InitiateCameraShake(5);
            Destroy(room);
        }
        if (!setup && room != null)
        {
            Destroy(room);
        }
    }
}
