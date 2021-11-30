using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnCubeController : MonoBehaviour
{
    public Color DefaultColor = Color.green;
    public Color ChangedColor = Color.cyan;
    public bool isAttachedToPlayerCube = false;
    Renderer rend;
    public void Start()
    {
        rend = GetComponent<Renderer>();
    }
    private void Update()  // optimise this
    {
        if (isAttachedToPlayerCube)
        {
            ChangeColor();
        }
        else
        {
            ResetColor();
        }
    }
    private void ChangeColor()
    {
        rend.material.color = ChangedColor;
    }
    private void ResetColor()
    {
        rend.material.color = DefaultColor;
    }
    public void AttachCube()
    {
        ModifyCollider();
        isAttachedToPlayerCube = true;
        
    }
    public void DetachCube()
    {
        ResetCollider();
        isAttachedToPlayerCube = false;
    }
    private void ModifyCollider()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void ResetCollider()
    {
        this.GetComponent<BoxCollider>().enabled = true;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
   
}
