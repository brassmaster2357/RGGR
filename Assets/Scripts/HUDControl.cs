using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDControl : MonoBehaviour
{
    public Image healthBar;
    public Image healthBarEmpty;
    public TextMeshProUGUI moneyText;
    public PlayerController pc;

    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        
    }
}
