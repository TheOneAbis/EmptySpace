using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObstacleCollisionHandler : MonoBehaviour
{
    private MonsterChaseController parentController;
    // Start is called before the first frame update
    void Start()
    {
        parentController = GetComponentInParent<MonsterChaseController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("MonsterCP") && parentController.mode == MonsterMode.Chase)
        {
            parentController.Stop();
            parentController.ResetWithRandomCP();
            Debug.Log("Chase failed. Restarting.");
        }
    }
}