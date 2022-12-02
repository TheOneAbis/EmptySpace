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
            StartCoroutine(DelayRespawn());
            Debug.Log("Chase failed. Restarting.");
        }
    }

    IEnumerator DelayRespawn()
    {
        transform.parent.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        yield return new WaitForSeconds(1);
        transform.parent.position = new Vector3(0, 500, 0);
        yield return new WaitForSeconds(5);
        parentController.ResetWithRandomCP();
        transform.parent.GetComponent<ParticleSystem>().Play();
    }
}
