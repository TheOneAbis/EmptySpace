using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseController : MonoBehaviour
{
    private Vector3 goal;
    public float moveSpeed;
    private bool shouldMove;

    // Start is called before the first frame update
    void Start()
    {
        goal = transform.position;
        shouldMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the monster if it is set to move toward the goal
        if (shouldMove)
        {
            if ((goal - transform.position).magnitude > 0.1f)
                transform.position += (goal - transform.position).normalized * moveSpeed * Time.deltaTime;
            else
                shouldMove = false;
        }
    }

    public void SetGoal(Vector3 goalPosition)
    {
        goal = goalPosition;
    }

    public void MoveToGoal()
    {
        shouldMove = true;
    }
}
