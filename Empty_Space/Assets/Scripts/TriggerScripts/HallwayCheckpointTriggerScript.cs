using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayCheckpointTriggerScript : MonoBehaviour
{
    private CheckpointController cpController;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        cpController = GameObject.Find("CheckpointManager").GetComponent<CheckpointController>();
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.gameObject.tag == "Player")
        {
            triggered = true;
            switch (cpController.currentCP)
            {
                case Checkpoint.Start:
                    cpController.SetCP(Checkpoint.Hallway);
                    break;
                case Checkpoint.Hallway:
                    cpController.SetCP(Checkpoint.EngineRoom);
                    break;
            }
        }
    }
}
