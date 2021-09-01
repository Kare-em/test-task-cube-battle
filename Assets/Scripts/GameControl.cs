using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [SerializeField] private float preStartTime;
    [SerializeField] private float battleTime;
    private GameObject table;

    public static bool GameIsOver;
    public static bool GameIsStarted;
    public static GameControl SharedInstance;

    // Start is called before the first frame update
    private void Awake()
    {
        SharedInstance = this;

        table = GameObject.FindGameObjectWithTag("ResultTable");
        table.SetActive(false);

        GameIsStarted = false;
        GameIsOver = false;

    }
    private void Start()
    {
        StartCoroutine(WaitPreSpawnTime());
    }
    private IEnumerator WaitPreSpawnTime()
    {
        yield return new WaitForSeconds(preStartTime);
        UnitManager.SharedInstance.SpawnUnits();
        StartCoroutine(WaitForTimeOut());
    }
    private IEnumerator WaitForTimeOut()
    {
        yield return new WaitForSeconds(battleTime);
        GameOver();
    }

    public void GameOver()
    {
        StopAllCoroutines();

        UnitManager.SharedInstance.SerializeAll();

        GameIsOver = true;
        Time.timeScale = 0f;

        ShowTable();
    }
    private void SetWinner()
    {
        var teams = UnitManager.SharedInstance.Teams;
        float[] sumExp = new float[teams.Count];
        for (int i = 0; i < teams.Count; i++)
        {
            foreach (var unit in teams[i])
            {
                sumExp[i] += unit.Experience;
            }
        }
        var textWinner = GameObject.FindGameObjectWithTag("WhoWinner").GetComponent<Text>();
        if (sumExp[0] > sumExp[1])
            textWinner.alignment = TextAnchor.MiddleLeft;
        else textWinner.alignment = TextAnchor.MiddleRight;
    }
    private void ShowTable()
    {
        table.SetActive(true);

        SetWinner();
        table.GetComponentInChildren<Result>().AddFullStats();
    }
}
