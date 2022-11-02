using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jumpscare : MonoBehaviour
{
    private GameObject enemy;
    private Vector3[] positionsScare1 =
    {
        new Vector3(25, 10, -41),
        new Vector3(32.5f, 10, -50),
        new Vector3(45, 10, -50),
    };
    private Vector3[] positionsScare2 =
    {
        new Vector3(25, 10, -59),
        new Vector3(21.5f, 7, -53),
        new Vector3(4, 3, -50),
    };
    public bool triggerable;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        triggerable = name == "Jumpscare_1" ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jumpscare triggers should only trigger once
        if (other.tag == "Player" && triggerable)
        {
            triggerable = false;
            switch (name)
            {
                case "Jumpscare_1":
                    StartCoroutine(Jumpscare1Sequence());
                    GameObject.Find("Enable_Jumpscare_2").GetComponent<Enemy_Jumpscare>().triggerable = true;
                    break;
                case "Enable_Jumpscare_2":
                    enemy.transform.position = positionsScare2[0];
                    GameObject.Find("Jumpscare_2").GetComponent<Enemy_Jumpscare>().triggerable = true;
                    break;
                case "Jumpscare_2":
                    StartCoroutine(Jumpscare2Sequence());
                    break;
            }
        }
    }

    IEnumerator Jumpscare1Sequence()
    {
        yield return new WaitForSeconds(0.25f); // delay 1/4 second before moving monster

        // P[t] = P[0] + t(P[1] - P[0])

        // Pos 0 (initial) to Pos 1
        for (float time = 0; time < 1; time += Time.deltaTime * 2)
        {
            enemy.transform.position = positionsScare1[0] + time * (positionsScare1[1] - positionsScare1[0]);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Pos 1 to Pos 2
        for (float time = 0; time < 1; time += Time.deltaTime * 2)
        {
            enemy.transform.position = positionsScare1[1] + time * (positionsScare1[2] - positionsScare1[1]);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        // move it to where it is unseeable
        enemy.transform.position = new Vector3(70, 8, -50);
    }

    IEnumerator Jumpscare2Sequence()
    {
        yield return new WaitForSeconds(0.25f); // delay 1/4 second before moving monster

        // Pos 0 (initial) to Pos 1
        for (float time = 0; time < 1; time += Time.deltaTime * 2)
        {
            enemy.transform.position = positionsScare2[0] + time * (positionsScare2[1] - positionsScare2[0]);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Pos 1 to Pos 2
        for (float time = 0; time < 1; time += Time.deltaTime * 2)
        {
            enemy.transform.position = positionsScare2[1] + time * (positionsScare2[2] - positionsScare2[1]);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        // move it to where it is unseeable
        enemy.transform.position = new Vector3(70, 8, -50);
    }
}
