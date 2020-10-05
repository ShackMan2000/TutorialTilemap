using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BugSpawner : MonoBehaviour
{

    [SerializeField]
    private Bug bugPrefab;




    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Bug newBug = Instantiate(bugPrefab);

            newBug.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
    }





}



