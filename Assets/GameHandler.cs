using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] GameObject mob;
    [SerializeField] Transform mobHolder;
    Grid grid;
    List<Mob> mobList = new List<Mob>();

    void Start()
    {
        grid = FindObjectOfType<Grid>();

        //spawn mobs
        for (int i = 0; i < 1000; i++)
        {
            var pos = new Vector3(Random.Range(1, 50), 3, Random.Range(1, 50));
            GameObject mobGO = Instantiate(mob, pos, Quaternion.identity);
            mobGO.transform.parent = mobHolder;
            mobGO.transform.Rotate(new Vector3(0, 0, 90));
            var mobScript = mobGO.GetComponent<Mob>();
            mobScript.grid = grid;
            mobList.Add(mobScript);
        }
    }

    void Update()
    {
        
    }
    public void UpdateMobMoveTargets()
    {
        mobList.ForEach(mob => mob.AdditionalMovementAdder());
    }
}
