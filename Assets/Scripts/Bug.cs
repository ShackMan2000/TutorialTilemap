using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{

    private MapManager mapManager;

    [SerializeField]
    private float moveTime;

    [SerializeField]
    private float baseSpeed;

    private float moveCounter;


    [SerializeField]
    private float stinkAmount, spreadIntervall = 1f;

    [SerializeField]
    private int stinkRadius;


    private StinkManagerSketch stinkManager;


    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        stinkManager = FindObjectOfType<StinkManagerSketch>();
       // StartCoroutine(SpreadStink());
    }


    private void Update()
    {
        moveCounter -= Time.deltaTime;

        if(moveCounter <= 0)
        {
            moveCounter = moveTime;
            float newRotation = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, 0f, newRotation);
        }


       // float adjustedSpeed = mapManager.GetTileWalkingSpeed(transform.position) * baseSpeed;

        transform.position += transform.up * Time.deltaTime * baseSpeed;

    }



    //private IEnumerator SpreadStink()
    //{

    //    while(true)
    //    {

    //        stinkManager.NewSpreadStink(transform.position, stinkAmount, stinkRadius);

    //        yield return new WaitForSeconds(spreadIntervall);



    //    }


    //}


}
