using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;




[ExecuteInEditMode]
public class MapVariation : MonoBehaviour
{

    public List<VariationPack> packs;


    [SerializeField]
    private List<VariationPack> packsWithFrequencies;

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private Vector3Int bottomLeft, topRight;



    public Tilemap map;

    public TileBase newTile;

    private void Start()
    {
        print(map.origin);
        print("size" + map.size);

        GoThroughMap();
    }


    public void GoThroughMap()
    {
        map.CompressBounds();
        Vector3Int origin = map.origin;
        Vector3Int size = map.size;

        for (int x = origin.x; x < origin.x + size.x; x++)
        {
            Vector3Int newVec = new Vector3Int(x, map.origin.y, map.origin.z);
            map.SetTile(newVec, newTile);
        }


    }




    public void ChangeMap()
    {
        CreatePacksWithFrequencies();       

        for (int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y <= topRight.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, bottomLeft.z);

                TileBase tile = map.GetTile(tilePosition);

                int packID = GetVariationPackID(tile);

                if (packID > -1)
                {

                    int randomTile = Random.Range(0, packsWithFrequencies[packID].tiles.Count);
                    TileBase replacementTile = packsWithFrequencies[packID].tiles[randomTile];

                    map.SetTile(tilePosition, replacementTile);
                }

            }
        }





    }


    private void CreatePacksWithFrequencies()
    {
        //go through packs and make a copy
        //inside go through all tiles of the pack and add to copy with frequency

        packsWithFrequencies = new List<VariationPack>();

        for (int i = 0; i < packs.Count; i++)
        {
            VariationPack newPack = new VariationPack();

            newPack.tiles = new List<TileBase>();

            for (int j = 0; j < packs[i].tiles.Count; j++)
            {

                for (int k = 0; k < packs[i].frequency[j]; k++)
                {
                    
                    newPack.tiles.Add(packs[i].tiles[j]);

                }

            }

            packsWithFrequencies.Add(newPack);

        }




    }


    //apply the frequencies

    //Go through tilemap and get tileType of every tile






    //check if that tile is in a variaiton Pack
    private int GetVariationPackID(TileBase tileToCheck)
    {
        int id = -1;

        for (int i = 0; i < packs.Count; i++)
        {
            foreach (var tile in packs[i].tiles)
            {
                if (tile == tileToCheck)
                    return i;
            }


        }

        return id;
    }








}
