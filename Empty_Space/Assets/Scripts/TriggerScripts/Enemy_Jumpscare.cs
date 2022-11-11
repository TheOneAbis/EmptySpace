using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jumpscare : MonoBehaviour
{
    private GameObject enemy;
    private AudioSource playerAudio;
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
    public AudioClip newAudio;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
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
                case "StopAudio":
                    // Stop and set to new creepy music
                    StartCoroutine(FadeOutAudio());
                    break;
                case "Jumpscare_1":
                    StartCoroutine(FadeInAudio());
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

    IEnumerator FadeOutAudio()
    {
        for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime * 2)
        {
            playerAudio.volume = i / 1.0f;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // After fading, stop and set to new audio
        playerAudio.Stop();
        playerAudio.clip = newAudio;
    }

    IEnumerator FadeInAudio()
    {
        // Begin and fade in new audio
        playerAudio.Play();
        for (float i = 0; i <= 1.0f; i += Time.deltaTime)
        {
            playerAudio.volume = i / 1.0f;
            yield return new WaitForSeconds(Time.deltaTime);
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

        // play new creepy music
        playerAudio.Play();
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
