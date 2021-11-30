using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndPointScript : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent playerReachedEndpoint;
    public static EndPointScript instance;
    public GameObject winscreen;
    
    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if(playerReachedEndpoint == null)
        {
            playerReachedEndpoint = new UnityEvent();
        }
       
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && PlayerControls.instance.reachedTarget == true)
        {
            playerReachedEndpoint.Invoke();
            winscreen.SetActive(true);
            
        }
    }
    
   


}