using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayCheckpointTriggerScript : MonoBehaviour
{
    private CheckpointController cpController;

    // Start is called before the first frame update
    void Start()
    {
        cpController = GameObject.Find("CheckpointManager").GetComponent<CheckpointController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            cpController.SetCP(Checkpoint.Hallway);
        }
    }
}
