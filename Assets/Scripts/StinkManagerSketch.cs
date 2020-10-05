using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StinkManagerSketch : MonoBehaviour
{


    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private Color colorNoStink, colorFullStink;

    [SerializeField]
    private float stinkFallOff;


    [SerializeField]
    private float maxStinkPerTile;



    [SerializeField]
    private float reduceStinkIntervall, reduceStinkAmount;


    public int testRadius;


    //keep track of all tiles with stinkValue > 0
    public Dictionary<Vector3Int, float> stinkingTiles = new Dictionary<Vector3Int, float>();


    private void Start()
    {
        StartCoroutine(ReduceStinkRoutine());
    }


    //public void SpreadStink()
    //{
    //    Dictionary<Vector3Int, float> stinkTilesCopy = new Dictionary<Vector3Int, float>(stinkingTiles);

    //    foreach (var entry in stinkTilesCopy)
    //    {
    //        Vector3Int position = entry.Key;

    //        // int tilesSpread = 0;



    //        if (entry.Value > spreadThreshhold)
    //        {

    //            stinkingTiles[entry.Key] = entry.Value - (entry.Value * stinkFallOff * 4f);

    //            for (int x = -1; x < 2; x++)
    //            {
    //                for (int y = -1; y < 2; y++)
    //                {
    //                    if (Mathf.Abs(x) + Mathf.Abs(y) == 1)
    //                    {
    //                        Vector3Int neighbourPosition = new Vector3Int(position.x + x, position.y + y, 0);
    //                        //   if (!stinkingTiles.ContainsKey(neighbourPosition) || stinkingTiles[neighbourPosition].stinkValue <)
    //                        ChangeStink(neighbourPosition, entry.Value * stinkFallOff);
    //                    }
    //                }
    //            }

    //        }


    //    }




    //    //go through all stinkingTiles(structs) and spread stink to neighbourtiles
    //    //only if neighbour is less stinky


    //}

    public void ReduceStink()
    {

        Dictionary<Vector3Int, float> stinkingTilesCopy = new Dictionary<Vector3Int, float>(stinkingTiles);

        foreach (var entry in stinkingTilesCopy)
        {
            ChangeStink(entry.Key, reduceStinkAmount);
        }


    }

    public void NewSpreadStink(Vector2 worldPosition, float stinkAmount, int radius)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);


        for (int x = -radius; x <= radius ; x++)
        {
            for (int y = -radius; y < radius + 1; y++)
            {
                int distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);
                if (distanceFromCenter < radius + 1)
                {
                    Vector3Int neighbourPosition = new Vector3Int(gridPosition.x + x, gridPosition.y + y, 0);
                    //   if (!stinkingTiles.ContainsKey(neighbourPosition) || stinkingTiles[neighbourPosition].stinkValue <)
                    ChangeStink(neighbourPosition, stinkAmount - (distanceFromCenter * stinkFallOff * stinkAmount));
                }
            }
        }
    }

    public void VisualizeStink()
    {
        foreach (var item in stinkingTiles)
        {
            float stinkPercent = item.Value / maxStinkPerTile;
            Color newStinkColor = (colorNoStink * (1f-stinkPercent) + colorFullStink * stinkPercent);
            


            map.SetTileFlags(item.Key, TileFlags.None);
            map.SetColor(item.Key, newStinkColor);
            map.SetTileFlags(item.Key, TileFlags.LockColor);

        }


    }



    public void ChangeStink(Vector3Int position, float changeBy)
    {              

        if (!stinkingTiles.ContainsKey(position))
            stinkingTiles.Add(position, 0f);

        float newAmount = stinkingTiles[position] + changeBy;

        if (newAmount <= 0f)
        {
            stinkingTiles.Remove(position);
            map.SetTileFlags(position, TileFlags.None);
            map.SetColor(position, colorNoStink);
            map.SetTileFlags(position, TileFlags.LockColor);

        }
        else
            stinkingTiles[position] = Mathf.Clamp(newAmount, 0f, maxStinkPerTile);

    }




    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            NewSpreadStink(mousePosition, 10f, testRadius);          

        }
    }


    private IEnumerator ReduceStinkRoutine()
    {
        while (true)
        {         
            ReduceStink();         
            VisualizeStink();
            print(stinkingTiles.Count);

            yield return new WaitForSeconds(reduceStinkIntervall);
        }


    }


    public Vector2 PositionOfStinkiestTile(Vector2 worldPosition, int radius)
    {
        //convert to gridPosition
        //go through list to get smelliest gridPosition
        //should return random if nothing smells at all

        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        Vector3Int goal = new Vector3Int(gridPosition.x + Random.Range(-radius, radius + 1), gridPosition.y + Random.Range(-radius, radius + 1), 0);

        float highestStink = GetStinkValue(goal);


        for (int x = -radius; x < radius + 1; x++)
        {
            for (int y = -radius; y < radius + 1; y++)
            {
                int distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);
                if (distanceFromCenter < radius + 1)
                {
                    Vector3Int possibleGoal = new Vector3Int(gridPosition.x + x, gridPosition.y + y, 0);

                    if (GetStinkValue(possibleGoal) > GetStinkValue(goal))
                        goal = possibleGoal;
                  
                }
            }
        }


        return map.CellToWorld(goal);

    }



    private float GetStinkValue(Vector3Int gridPosition)
    {
        if (stinkingTiles.ContainsKey(gridPosition))
            return stinkingTiles[gridPosition];
        else return 0f;
    }


}
