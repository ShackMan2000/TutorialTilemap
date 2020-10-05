using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FireManager : MonoBehaviour
{


    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private TileBase ashTile;


    [SerializeField]
    private MapManager mapManager;

    [SerializeField]
    private Fire firePrefab;


    private List<Vector3Int> activeFires = new List<Vector3Int>();




    public void FinishedBurning(Vector3Int position)
    {
        map.SetTile(position, ashTile);
        activeFires.Remove(position);
    }

    public void TryToSpread(Vector3Int position, float spreadChance)
    {
        for (int x = position.x -1; x < position.x + 2 ; x++)
        {
            for (int y = position.y - 1; y < position.y + 2; y++)
            {
                TryToBurnTile(new Vector3Int(x, y, 0));
            }
        }


        void TryToBurnTile(Vector3Int tilePosition)
        {
            if (activeFires.Contains(tilePosition)) return;

            TileData data = mapManager.GetTileData(tilePosition);

            if(data != null && data.canBurn)
            {
                if (UnityEngine.Random.Range(0f, 100f) <= data.spreadChance)
                    SetTileOnFire(tilePosition, data);

            }

        }

    }

    private void SetTileOnFire(Vector3Int tilePosition, TileData data)
    {
        Fire newFire = Instantiate(firePrefab);
        newFire.transform.position = map.GetCellCenterWorld(tilePosition);
        newFire.StartBurning(tilePosition, data, this);

        activeFires.Add(tilePosition);

    }




    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileData data = mapManager.GetTileData(gridPosition);

            SetTileOnFire(gridPosition, data);


        }
    }


}
