using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseInitController : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;

    // Instance Editable
    public GameObject doorToCheck;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {

            if (doorToCheck.GetComponent<DoorController>().locked == false)
            {
                triggered = true;
                StartCoroutine(InitEnemyChaseSequence());
            }
        }
    }

    IEnumerator InitEnemyChaseSequence()
    {
        yield return new WaitForSeconds(0.5f);
        enemy.transform.position = new Vector3(-18, 3, -66); // enemy starting position


    }
}
