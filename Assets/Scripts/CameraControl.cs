using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraControl : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCam;
    private GameObject targetunit = null;
    // Start is called before the first frame update
    private void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void SetFollowUnit()
    {
        var followUnit = targetunit.transform;
        virtualCam.Follow = virtualCam.LookAt = followUnit;
    }

    public void FindRandomUnit()
    {
        var teams = UnitManager.SharedInstance.Teams;
        var indexTeam = Random.Range(0, teams.Count);
        var indexUnit = Random.Range(0, teams[indexTeam].Count);
        targetunit = teams[indexTeam][indexUnit].gameObject;
        SetFollowUnit();
    }
    private void FixedUpdate()
    {
        if ((GameControl.GameIsStarted) && (!targetunit.activeInHierarchy))
        {
            FindRandomUnit();
        }
    }
}
