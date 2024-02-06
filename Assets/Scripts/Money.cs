using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public float scale;
    private void Awake()
    {
        scale = Random.Range(2.5f, 5);
        float money = scale * 20;
        transform.localScale = new(scale, scale, 1);
    }
}
