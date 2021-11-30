using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject STARTPOINT;
    public GameObject player;
    public static bool hasReachedEndPoint = false;
    
   
   

    private void OnEnable()
    {
        EndPointScript.instance.playerReachedEndpoint.AddListener(OnReachEndPoint);
        
    }
    void Start()
    {
        ResetPlayerPosition();
        
    }
   
    public void OnReachEndPoint()
    {
        hasReachedEndPoint = true;
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = STARTPOINT.transform.position;
    }
    private void OnDisable()
    {
        EndPointScript.instance.playerReachedEndpoint.RemoveListener(OnReachEndPoint);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
