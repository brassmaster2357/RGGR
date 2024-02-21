using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraShake;
    private Vector3 position;
    private Vector3 goalPosition;

    private float shakeIntensity;
    public float shakeFalloff = 0.9f;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        position.z = -10;
        goalPosition.z = -10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Follow the player in intervals of 32x20 because Steam Deck
        goalPosition.x = Mathf.Floor((player.transform.position.x + 16) / 32) * 32;
        goalPosition.y = Mathf.Floor((player.transform.position.y + 10) / 20) * 20;
        if ((goalPosition - position).magnitude > 30)
        {
            player.GetComponent<PlayerController>().invul = 0.5f; // Half-second grace period when entering room
        }
        position = (position * 6 + goalPosition) / 7; // *slowly* go to the real position
        gameObject.transform.position = new(position.x + Random.Range(-shakeIntensity, shakeIntensity), position.y + Random.Range(-shakeIntensity, shakeIntensity), -10);
        shakeIntensity *= shakeFalloff;
    }

    public void InitiateCameraShake(int damage)
    {
        shakeIntensity = 0.25f * damage;
    }
}
