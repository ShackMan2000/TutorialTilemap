using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private Vector3Int position;

    private TileData data;

    private FireManager fireManager;


    private float burnTimeCounter, spreadIntervallCounter;




    public void StartBurning(Vector3Int position, TileData data, FireManager fm)
    {
        this.position = position;
        this.data = data;
        fireManager = fm;

        burnTimeCounter = data.burnTime;
        spreadIntervallCounter = data.spreadIntervall;
    }



    private void Update()
    {
        burnTimeCounter -= Time.deltaTime;
        if(burnTimeCounter <=0)
        {
            fireManager.FinishedBurning(position);
            Destroy(gameObject);
        }

        spreadIntervallCounter -= Time.deltaTime;
        if(spreadIntervallCounter <=0)
        {
            spreadIntervallCounter = data.spreadIntervall;
            fireManager.TryToSpread(position, data.spreadChance);
        }
        
    }







}
