using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadStink : MonoBehaviour
{

    [SerializeField]
    private float spreadAmount, spreadIntervall = 1f;


    [SerializeField]
    private int radius;


    [SerializeField]
    private StinkManager stinkManager;



    private void Awake()
    {
        stinkManager = FindObjectOfType<StinkManager>();
    }


    private void Start()
    {
        StartCoroutine(SpreadRoutine());
    }


    private IEnumerator SpreadRoutine()
    {

        while(true)
        {
            stinkManager.AddStink(transform.position, spreadAmount, radius);

            yield return new WaitForSeconds(spreadIntervall);



        }



    }




}
