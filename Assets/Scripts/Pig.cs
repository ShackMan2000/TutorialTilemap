using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{

    [SerializeField]
    private float speed;


    private Vector2 goal;

    private StinkManager stinkManager;

    [SerializeField]
    private int smellRadius;

    private void Awake()
    {
        stinkManager = FindObjectOfType<StinkManager>();
        goal = stinkManager.GetStinkiestTile(transform.position, smellRadius);
        RotateToGoal();
    }




    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, goal, speed * Time.deltaTime);


        if ((Vector2)transform.position == goal)
        {

            goal = stinkManager.GetStinkiestTile(transform.position, smellRadius); ;
            RotateToGoal();

        }
    }

    private void RotateToGoal()
    {
        Vector2 direction = goal - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
