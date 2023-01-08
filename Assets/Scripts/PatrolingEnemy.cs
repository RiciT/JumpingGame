using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PatrolingEnemy : Enemy
{
    public float speed = 5;
    public Transform[] patrolPoints;

    int index = 0;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[index].position, speed * Time.deltaTime);
        if (transform.position == patrolPoints[index].position)
            index++;
        if (index == patrolPoints.Length)
            index = 0;
    }
}
