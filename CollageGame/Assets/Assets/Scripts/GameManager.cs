using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject StartPoint;
    [HideInInspector]
    public GameObject player;
    public static bool hasReachedEndPoint = false;
    
   
   

    private void OnEnable()
    {
        EndPointScript.instance.playerReachedEndpoint.AddListener(OnReachEndPoint);
        
    }
    private void Awake()
    {
        StartPoint = GameObject.FindGameObjectWithTag("StartPoint");
        if(StartPoint == null)
        {
            Debug.LogWarning("No StartPoint exists in the scene. Place a StartPoint in the scene");
        }
        player = GameObject.FindGameObjectWithTag("Player");

        if(player == null )
        {
            Debug.LogWarning("No player gameobject found in the scene. Place a player gameobject in the scene");
        }
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
        if(StartPoint != null && player !=null)
        player.transform.position = StartPoint.transform.position;
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
