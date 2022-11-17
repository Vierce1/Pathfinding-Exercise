using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] GameObject mob;
    [SerializeField] Transform mobHolder;
    Grid grid;
    [SerializeField] GameObject heroPrefab;
    Hero hero;
    List<Mob> mobList = new List<Mob>();
    public int currentLevel = 1;
    public bool draggingRepulsor = false;
    public bool levelPlaying = false;
    public bool beatLevel = false;

    //level specific
    [SerializeField] GameObject[] levelObjects;
   

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        System.Array.ForEach(levelObjects, level => level.SetActive(false));
        SetUpLevel(currentLevel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            levelPlaying = true;
        }

        if (Application.isEditor && Input.GetKeyDown(KeyCode.N))
        {
            beatLevel = true;
            LoadNextLevel();
        }
    }
    public void UpdateMobMoveTargets()
    {
        mobList.ForEach(mob => mob.AdditionalMovementAdder());
    }
    public void LoadNextLevel()
    {
        levelObjects[currentLevel - 1].SetActive(false);
        currentLevel++;
        beatLevel = false;
        //Destroy all objects / Set inactive       
        grid.DestroyGrid();
        mobList.ForEach(x => Destroy(x.gameObject));
        mobList.Clear();
        Destroy(hero.gameObject);
        SetUpLevel(currentLevel);
    }
    void SetUpLevel(int level)
    {
        levelObjects[level - 1].SetActive(true);        
        var camPos = GetCamStartPos(level);
        Camera.main.transform.position = camPos.pos;
        Camera.main.transform.rotation = camPos.rot;
        var heroPos = GetHeroStart(level);
        GameObject heroGO = Instantiate(heroPrefab, heroPos.pos, heroPos.rot);
        hero = heroGO.GetComponent<Hero>();

        var gridSize = GetGridSize(currentLevel);
        grid.xCellCount = gridSize.x;
        grid.zCellCount = gridSize.z;
        Debug.Log("Buidling grid " + level);
        grid.BuildGrid();

        // Mobs
        PlaceMobs(level);
    }
    (int x, int z) GetGridSize(int level)
    {
        switch (level)
        {
            case 1: return (60, 18);
            case 2: return (60, 18);
        }
        return (0, 0);
    }
    (Vector3 pos, Quaternion rot) GetCamStartPos(int level)
    {
        switch (level)
        {
            case 1: return
                    (new Vector3(70, 5.5f, 26f), Quaternion.Euler(4.24f, 176, 0));
            case 2: return (new Vector3(70, 5.5f, 26f), Quaternion.Euler(4.24f, 176, 0));

        }
        return (Vector3.zero, Quaternion.identity);
    }
    (Vector3 pos, Quaternion rot) GetHeroStart(int level)
    {
        switch (level)
        {
            case 1: return (new Vector3(77.3f, 2.628f, 4.54f), Quaternion.Euler(0, 270, 0));
            case 2: return (new Vector3(77.3f, 2.628f, 4.54f), Quaternion.Euler(0, 270, 0));

        }
        return (Vector3.zero, Quaternion.identity);
    }
    void PlaceMobs(int level)
    {
        var mobCount = 100;
        var mobX = (0, 60);
        var mobZ = (0, 10);
        switch (level)
        {
            case 1:
                mobCount = 100;
                mobX = (0, 60);
                mobZ = (0, 10);
                break;
            case 2: //Same level but with target can cut down
                mobCount = 800;
                mobX = (0, 60);
                mobZ = (0, 10);
                break;
        }

        //spawn mobs
        for (int i = 0; i < mobCount; i++)
        {
            var x = Random.Range(mobX.Item1, mobX.Item2);
            var z = Random.Range(mobZ.Item1, mobZ.Item2);
            var mobPos = new Vector3(x, 2.6f, z);
            GameObject mobGO = Instantiate(mob, mobPos, Quaternion.identity);
            mobGO.transform.parent = mobHolder;
            var mobScript = mobGO.GetComponent<Mob>();
            mobScript.grid = grid;
            mobList.Add(mobScript);
        }
    }
}
