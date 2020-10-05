using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PigBugSketch : MonoBehaviour
{

    //get strongest smelling tile, go there



    [SerializeField]
    private float baseSpeed;

private StinkManager stinkManager;

    private Vector2 currentGoal;

    [SerializeField]
    private int smellRadius;


    private void Awake()
    {
        stinkManager = FindObjectOfType<StinkManager>();
        currentGoal = stinkManager.GetStinkiestTile(transform.position, smellRadius);
    }


    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentGoal, baseSpeed * Time.deltaTime);
        Vector2 direction = currentGoal - (Vector2)transform.position;
        

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        if ((Vector2)transform.position == currentGoal)
            currentGoal = stinkManager.GetStinkiestTile(transform.position, smellRadius);


    }







}
