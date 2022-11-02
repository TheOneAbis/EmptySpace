using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jumpscare : MonoBehaviour
{
    private GameObject enemy;
    private Vector3[] positions =
    {
        new Vector3(25, 10, -41),
        new Vector3(32.5f, 10, -50),
        new Vector3(45, 10, -50),
    };

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(JumpscareSequence());
        }
    }

    IEnumerator JumpscareSequence()
    {
        // P[t] = P[0] + t(P[1] - P[0])

        // Pos 0 (initial) to Pos 1
        for (float time = 0; time < 1; time += Time.deltaTime * 2)
        {
            enemy.transform.position = positions[0] + time * (positions[1] - positions[0]);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Pos 1 to Pos 2
        for (float time = 0; time < 1; time += Time.deltaTime * 2)
        {
            enemy.transform.position = positions[1] + time * (positions[2] - positions[1]);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        // move it to where it is unseeable
        enemy.transform.position += new Vector3(50, 0, 0);
    }
}
