using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraShake;
    private float decayRate;
    private Vector2 posFromPlayer;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        posFromPlayer = new(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
    }
}
