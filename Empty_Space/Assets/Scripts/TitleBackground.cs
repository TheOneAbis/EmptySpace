using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackground : MonoBehaviour
{
    private Vector2 current;
    private Vector2 reset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Awake
    private void Awake()
    {
        reset = new Vector2(1393.5f, 163.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current = gameObject.GetComponent<Transform>().transform.position;
        if (current.x <= -900)
        {
            gameObject.GetComponent<Transform>().transform.position = reset;
        }
        else
        {
            current.x = current.x - 2.0f;
            gameObject.GetComponent<Transform>().transform.position = current;
        }
    }
}
