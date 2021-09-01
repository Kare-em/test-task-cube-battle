using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSerialize
{
    private string nickName;
    private string damage;
    private string murders;
    private int experience;

    public string NickName { get => nickName; set => nickName = value; }
    public string Damage { get => damage; set => damage = value; }
    public string Murders { get => murders; set => murders = value; }
    public int Experience { get => experience; set => experience = value; }
    public UnitSerialize()
    {

    }

    public void Serialize(Unit unit)
    {
        int damage = Mathf.RoundToInt(unit.DamageScore);
        int experience = Mathf.RoundToInt(unit.Experience);
        NickName = unit.Nickname;
        Damage = damage.ToString();
        Murders = unit.Murders.ToString();
        Experience = experience;

    }

    public List<string> GetStringProperties(int i)
    {
        List<string> array = new List<string>() { (i + 1).ToString(), NickName, Damage, Murders, Experience.ToString() };
        return array;
    }
}
