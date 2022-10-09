using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cryo_Pod_Script : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.Play("Scene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
