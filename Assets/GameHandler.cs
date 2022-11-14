using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] GameObject mob;
    [SerializeField] Transform mobHolder;

    void Start()
    {
        //spawn mobs
        for(int i = 0; i < 100; i++)
        {
            var pos = new Vector3(Random.Range(1, 50), 3, Random.Range(1, 50));
            GameObject mobGO = Instantiate(mob, pos, Quaternion.identity);
            mobGO.transform.parent = mobHolder;
            mobGO.transform.Rotate(new Vector3(0, 0, 90));
        }
    }

    void Update()
    {
        
    }
}