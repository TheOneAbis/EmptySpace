using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseInitController : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;
    private GameObject crosshairUI;

    // Instance Editable
    public GameObject doorToCheck;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        crosshairUI = GameObject.Find("Crosshair");
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
            if (doorToCheck.GetComponent<DoorController>().jammed == false && !triggered)
            {
                triggered = true;
                StartCoroutine(InitEnemyChaseSequence());
            }
        }
    }

    IEnumerator InitEnemyChaseSequence()
    {
        // disable player controls for cutscene
        crosshairUI.SetActive(false);
        player.GetComponent<FirstPersonController>().enabled = false;

        // Cutscene
        yield return new WaitForSeconds(0.5f);
        RenderSettings.ambientIntensity = 0.0f;
        yield return new WaitForSeconds(1.0f);
        enemy.transform.position = new Vector3(-18, 3, -66); // enemy starting position
        enemy.transform.Rotate(0, -90, 0);
        enemy.GetComponent<Light>().type = LightType.Spot;
        enemy.GetComponent<Light>().intensity = 50;
        enemy.GetComponent<Light>().range = 40;
        enemy.GetComponent<Light>().innerSpotAngle = 50;
        enemy.GetComponent<Light>().spotAngle = 90;
        yield return new WaitForSeconds(0.75f);

        Quaternion startRotation = player.transform.rotation;
        Vector3 lookDirection = (enemy.transform.position - player.transform.position);
        lookDirection.y = 0;
        
        for (float i = 0; i < 1.0f; i += Time.deltaTime)
        {
            player.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(lookDirection.normalized, player.transform.up), i);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(2.5f);
        GameObject.Find("UIManager").GetComponent<UIManagement>().DisplayTooltip(Tooltip.Sprint, 5.0f);
        // re-enable player controls
        crosshairUI.SetActive(true);
        player.GetComponent<FirstPersonController>().enabled = true;
    }
}
