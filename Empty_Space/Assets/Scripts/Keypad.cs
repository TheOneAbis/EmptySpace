using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    private GameObject player;
    private float distanceSquared;

    public GameObject exitDoor;
    public float interactDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        distanceSquared = (player.transform.position - transform.position).sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        distanceSquared = (player.transform.position - transform.position).sqrMagnitude;

        if (distanceSquared < Mathf.Pow(interactDistance, 2))
        {
            
        }
    }
}
