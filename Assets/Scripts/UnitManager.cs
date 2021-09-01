using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject unitRed;
    [SerializeField] private GameObject unitBlue;
    [SerializeField] private float widthSpawn;
    [SerializeField] private int playerCount;
    [SerializeField] private int NumberOfBulletsForEach;

    private CameraControl cameraCtrl;

    public static UnitManager SharedInstance;
    public List<List<Unit>> TempTeams;
    public List<List<Unit>> Teams;
    public List<List<UnitSerialize>> SerializedTeams;

    public int PlayerCount { get => playerCount; set => playerCount = value; }

    private void Awake()
    {
        SharedInstance = this;

        SerializedTeams = new List<List<UnitSerialize>>();
        SerializedTeams.Add(new List<UnitSerialize>());
        SerializedTeams.Add(new List<UnitSerialize>());

        TempTeams = new List<List<Unit>>();
        Teams = new List<List<Unit>>();

        cameraCtrl = FindObjectOfType<CameraControl>();

    }
    public void SpawnUnits()
    {
        SpawnTeam(unitRed);
        SpawnTeam(unitBlue);
        SignUpAllUnits();

        GameControl.GameIsStarted = true;
        cameraCtrl.FindRandomUnit();
    }
    private void SpawnTeam(GameObject unit)
    {
        List<Unit> team = new List<Unit>();
        for (int i = 0; i < playerCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(0, widthSpawn), 3, Random.Range(0, widthSpawn));
            GameObject tempUnit = Instantiate(unit, position, Quaternion.identity);
            team.Add(tempUnit.GetComponent<Unit>());
        }
        Teams.Add(team);
    }
    private void AddUnitsInList(string tag)
    {

        List<Unit> team = new List<Unit>();
        foreach (var unit in GameObject.FindGameObjectsWithTag(tag))
        {
            team.Add(unit.GetComponent<Unit>());
        }
        TempTeams.Add(team);
    }
    private void SignUpAllUnits()
    {
        AddUnitsInList("Red");
        AddUnitsInList("Blue");
    }


    public void SerializeAll()
    {
        foreach (var team in TempTeams)
        {
            foreach (var unit in team)
            {
                UnitSerialize unitSerialize = new UnitSerialize();
                unitSerialize.Serialize(unit);
                SerializedTeams[unit.NumberOfTeam].Add(unitSerialize);
            }
        }
    }

    public List<Unit> FindInTheDistance(Unit finder)
    {
        List<Unit> theNearest = new List<Unit>();
        var position = finder.transform.position;
        var explosionDistance = finder.ExplosionDistance;
        var indexOfEnemyTeam = IndexOfEnemyTeam(finder);
        foreach (var unit in Teams[indexOfEnemyTeam])
        {
            if (!(unit is null))
            {

                float currentDistance = CalculateDistance(position, unit);
                if (currentDistance < explosionDistance)
                {
                    theNearest.Add(unit);
                }
            }
        }
        return theNearest;
    }

    public float CalculateDistance(Vector3 position, Unit unit)
    {
        Vector3 differenceVector = unit.transform.position - position;
        return differenceVector.magnitude;
    }
    private static int IndexOfEnemyTeam(Unit finder)
    {
        if (finder.NumberOfTeam == 0)
            return 1;
        else
            return 0;
    }

    public Unit FindClosestEnemy(Unit finder)
    {
        Vector3 position = finder.transform.position;
        Unit closest = null;
        float distance = Mathf.Infinity;
        int indexOfEnemyTeam = IndexOfEnemyTeam(finder);
        foreach (var unit in Teams[indexOfEnemyTeam])
        {
            if (!(unit is null))
            {
                float currentDistance = CalculateDistance(position, unit);
                if (currentDistance < distance)
                {
                    closest = unit;
                    distance = currentDistance;
                }
            }
        }
        return closest;
    }



    private void FixedUpdate()
    {
        if (!GameControl.GameIsOver)
        {
            bool gameIsOver = false;
            foreach (var team in Teams)
                if (team.Count < 1)
                    gameIsOver = true;

            if (GameControl.GameIsStarted && gameIsOver)
                GameControl.SharedInstance.GameOver();
        }
    }




}
