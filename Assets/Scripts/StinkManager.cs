using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class StinkManager : MonoBehaviour
{

    [SerializeField]
    private Tilemap stinkMap;

    [SerializeField]
    private float maxStink;



    [SerializeField]
    private Color maxStinkColor, minStinkColor, clearColor;



    private Dictionary<Vector3Int, float> stinkingTiles = new Dictionary<Vector3Int, float>();

    [SerializeField]
    private float testAddStinkAmount;

    [SerializeField]
    private float stinkFallOff;


    [SerializeField]
    private int testRadius;



    [SerializeField]
    private float reduceAmount, reduceIntervall = 1f;







    private void Start()
    {
        StartCoroutine(ReduceStinkRoutine());
    }


    public void AddStink(Vector2 worldPosition, float stinkAmount, int radius)
    {
        Vector3Int gridPosition = stinkMap.WorldToCell(worldPosition);


        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                float distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);
                if(distanceFromCenter <= radius)
                {
                    Vector3Int nextTilePosition = new Vector3Int(gridPosition.x + x, gridPosition.y + y, 0);
                    ChangeStink(nextTilePosition, stinkAmount - (distanceFromCenter * stinkFallOff * stinkAmount));

                }


            }
        }

        ChangeStink(gridPosition, stinkAmount);

        VisualizeStink();


    }







    private void ChangeStink(Vector3Int gridPosition, float changeBy)
    {

        if (!stinkingTiles.ContainsKey(gridPosition))
            stinkingTiles.Add(gridPosition, 0f);


        float newValue = stinkingTiles[gridPosition] + changeBy;


        if (newValue <= 0f)
        {
            stinkingTiles.Remove(gridPosition);

            stinkMap.SetTileFlags(gridPosition, TileFlags.None);
            stinkMap.SetColor(gridPosition, clearColor);
            stinkMap.SetTileFlags(gridPosition, TileFlags.LockColor);


        }
        else
            stinkingTiles[gridPosition] = Mathf.Clamp(newValue, 0f, maxStink);


    }



    private IEnumerator ReduceStinkRoutine()
    {
        while (true)
        {


            Dictionary<Vector3Int, float> stinkingTilesCopy = new Dictionary<Vector3Int, float>(stinkingTiles);

            foreach (var entry in stinkingTilesCopy)
            {
                ChangeStink(entry.Key, reduceAmount);
            }

        VisualizeStink();

            yield return new WaitForSeconds(reduceIntervall);

        }



    }




    private void VisualizeStink()
    {
        foreach (var entry in stinkingTiles)
        {
            float stinkPercent = entry.Value / maxStink;

            Color newTileColor = maxStinkColor * stinkPercent + minStinkColor * (1f - stinkPercent);


            stinkMap.SetTileFlags(entry.Key, TileFlags.None);
            stinkMap.SetColor(entry.Key, newTileColor);
            stinkMap.SetTileFlags(entry.Key, TileFlags.LockColor);



        }



    }

    




    public Vector2 GetStinkiestTile(Vector2 worldPosition, int radius)
    {

        Vector3Int gridPosition = stinkMap.WorldToCell(worldPosition);

        Vector3Int goal = new Vector3Int(gridPosition.x + Random.Range(-radius, radius + 1), gridPosition.y + Random.Range(-radius, radius + 1), 0);

        float highestStink = GetStinkValue(goal);


        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                float distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);
                if (distanceFromCenter <= radius)
                {
                    Vector3Int possibleGoal = new Vector3Int(gridPosition.x + x, gridPosition.y + y, 0);

                    if (GetStinkValue(possibleGoal) > GetStinkValue(goal))
                        goal = possibleGoal;

                }


            }
        }


        return stinkMap.CellToWorld(goal);



    }
    
    
    
    

    private float GetStinkValue(Vector3Int gridPosition)
    {
        if (stinkingTiles.ContainsKey(gridPosition))
            return stinkingTiles[gridPosition];
        else
            return 0f;

    }

    


}
