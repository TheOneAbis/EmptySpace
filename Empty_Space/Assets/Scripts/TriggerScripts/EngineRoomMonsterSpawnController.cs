using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EngineRoomMonsterSpawnController : MonoBehaviour
{
    private GameObject player;
    private GameObject monster;

    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        monster = GameObject.FindGameObjectWithTag("Enemy");
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn the monster and begin its patrol
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.gameObject == player)
        {
            triggered = true;
            monster.GetComponent<MonsterChaseController>().InitPatrolMode();
        }
    }
}
