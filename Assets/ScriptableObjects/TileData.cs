using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class TileData : ScriptableObject
{

    public TileBase[] tiles;


    public float walkingSpeed, poisonous;


    public bool canBurn;


    public float spreadChance, spreadIntervall, burnTime;


}
