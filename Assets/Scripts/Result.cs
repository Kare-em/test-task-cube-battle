using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Result : MonoBehaviour
{
    private List<List<string>> GetStringStats(List<UnitSerialize> team)
    {

        List<List<string>> results = new List<List<string>>();

        for (int i = 0; i < team.Count; i++)
        {
            results.Add(team[i].GetStringProperties(i));
        }
        return results;
    }

    private List<UnitSerialize> SortTeam(List<UnitSerialize> team)
    {
        var sortedTeam = from u in team
                         orderby u.Experience descending
                         select u;


        return sortedTeam.ToList();
    }
    public void AddFullStats()
    {

        List<List<GameObject>> blocks = new List<List<GameObject>>();

        blocks.Add(GameObject.FindGameObjectsWithTag("ResultColumnRed").ToList());
        blocks.Add(GameObject.FindGameObjectsWithTag("ResultColumnBlue").ToList());

        for (int i = 0; i < blocks.Count; i++)//блок команды
        {
            var sortedTeam = SortTeam(UnitManager.SharedInstance.SerializedTeams[i]);
            var resultsTeam = GetStringStats(sortedTeam);
            for (int j = 0; j < blocks[i].Count; j++)// столбец свойства
            {
                var tempText = blocks[i][j].GetComponent<Text>();

                for (int k = 0; k < resultsTeam.Count; k++)// свойство игрока
                {
                    tempText.text += resultsTeam[k][j] + '\n';
                }
            }
        }
    }
}