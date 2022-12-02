using Mono.Cecil.Cil;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Checkpoint
{
    Start,
    Hallway,
    EngineRoom
}

public class CheckpointController : MonoBehaviour
{
    private Vector3 respawnPoint;
    private Vector3 respawnRotation;

    private GameObject player;
    private GameObject enemy;

    public Checkpoint currentCP;

    // Set in editor
    public GameObject[] hallwayJammedDoors;
    public GameObject[] endDoors;
    public GameObject chaseInitTrigger;

    public GameObject[] batteriesToRespawn;
    public GameObject[] puzzlesToReset;

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = new Vector3(-2, 9, 65); // start of game
        respawnRotation = Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        currentCP = Checkpoint.Start;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetCP(Checkpoint checkpoint)
    {
        if (checkpoint != currentCP)
        {
            currentCP = checkpoint;

            switch (currentCP)
            {
                case Checkpoint.Hallway:
                    respawnPoint = new Vector3(6, 2, -66); // beginning of hallway
                    respawnRotation = new Vector3(0, -90, 0);
                    break;
                case Checkpoint.EngineRoom:
                    respawnPoint = new Vector3(-106, 3, -66); // beginning of hallway
                    respawnRotation = new Vector3(0, -90, 0);
                    break;
            }
        }
    }

    public void Respawn()
    {
        player.transform.position = respawnPoint;
        player.transform.rotation = Quaternion.Euler(respawnRotation);

        switch (currentCP)
        {
            case Checkpoint.Hallway:
                RenderSettings.ambientIntensity = 1.0f; // reset environment lighting

                foreach (GameObject door in hallwayJammedDoors)
                    door.GetComponent<DoorController>().ResetState(true, true, false, false);

                foreach (GameObject door in endDoors)
                    door.GetComponent<DoorController>().ResetState(true, false, true, true);

                enemy.transform.position += new Vector3(0, 100, 0); // move monster out of view
                chaseInitTrigger.GetComponent<MonsterChaseInitController>().ResetTrigger();

                break;
            case Checkpoint.EngineRoom:
                foreach (GameObject battery in batteriesToRespawn)
                {
                    player.GetComponent<PlayerInventoryManager>().Remove(battery);
                    battery.GetComponent<Collectible>().collected = false;
                }
                //foreach (GameObject puzzle in puzzlesToReset)
                //{
                //    puzzle.GetComponent<PipeManager>().
                //}

                enemy.transform.position += new Vector3(0, 500, 0);
                enemy.GetComponent<MonsterChaseController>().mode = MonsterMode.Hallway;

                player.GetComponent<FirstPersonController>().Gravity = -15;
                player.GetComponent<FirstPersonController>().zeroG = false;

                GameObject.Find("EngineRoom_MonsterSpawnTrigger").GetComponent<EngineRoomMonsterSpawnController>().triggered = false;

                break;
        }

        StartCoroutine(DelayMovementControls());
    }

    IEnumerator DelayMovementControls()
    {
        yield return new WaitForSeconds(Time.deltaTime * 5);
        player.GetComponent<FirstPersonController>().enabled = true;
    }
}
