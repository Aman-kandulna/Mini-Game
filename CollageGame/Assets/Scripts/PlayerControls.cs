using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControls : MonoBehaviour
{
    private Transform target;
    public float movespeed;
    public bool useWallMotion = true;
    private Vector3 dir;
    private bool isPawnCubeAttached;
    private Vector3 pawnCubeDirectionWithRespectToPlayerCube;
    public  BoxCollider bx;
    private GameObject Pawncube = null;
    private bool wasMoving;
    private bool isHitting;
    RaycastHit hitinfo;
    private float ColliderCorrectionValue = 0.1f;
    public GameManager gameManager;
    private LayerMask layermask;
    private Vector3 lastStaticPosition;
    private Vector3 lastTransform;
   
    public void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.playerReachedEndpoint.AddListener(OnreachedEndpoint);

    }
    private void OnDisable()
    {
        gameManager.playerReachedEndpoint.RemoveListener(OnreachedEndpoint);
    }


    void Start()
    {
        bx = transform.GetComponent<BoxCollider>();
        layermask = LayerMask.GetMask("Walls") | LayerMask.GetMask("PawnCube");
        SetPlayerProperties();
        
    }
    void SetPlayerProperties()
    {
        target = null;
        lastStaticPosition = transform.position + bx.center;
        lastTransform = transform.position;
        wasMoving = false;
        isPawnCubeAttached = false;
        pawnCubeDirectionWithRespectToPlayerCube = Vector3.zero;
        gameManager.PlayerHasReachedTarget = true;
        GameManager.hasReachedEndPoint = false;
        
    }
    private void Update()
    {
      
        if (target != null)
        {

            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * movespeed);
            gameManager.PlayerHasReachedTarget = false;
            wasMoving = true;

            if (Vector3.Distance(transform.position, target.position) == 0f)
            {
                WaypointPooler.instance.returnWaypointToPool(target);
                target = null;
                gameManager.PlayerHasReachedTarget = true;
            }

        }
        if (gameManager.PlayerHasReachedTarget && !GameManager.hasReachedEndPoint)
        {
            if (isPawnCubeAttached && Input.GetKeyDown(KeyCode.E))
            {

                DetachPawnCube();
                DetachPlayerCube();

            }
            if (!isPawnCubeAttached && wasMoving)
            {
                DeterminePawnCubeLocation();
            }
         
            wasMoving = false;
            TakeInput();

            if(dir != Vector3.zero)
            SetWaypointInInputDirection();

            CheckIfPlayerMoved();
            lastStaticPosition = transform.position + bx.center;
            lastTransform = transform.position;

        }

    }
    public void CheckIfPlayerMoved()
    {
        if(transform.position != lastTransform)
        {
            gameManager.IncreaseMoveCounter(); // increases moveCount on player movement
        }
    }
    
    public void OnreachedEndpoint()
    {
       
        if (isPawnCubeAttached)
        {
            DetachPawnCube();
            DetachPlayerCube();
        }
        GameManager.hasReachedEndPoint = true;
        gameManager.PlayerHasReachedTarget = true;
        Debug.Log("EndPoint Reached");
    }
    private void DeterminePawnCubeLocation()
    {
     
        Ray directedray = new Ray(transform.position, dir);
        
        RaycastHit hitinfo;
        if (Physics.Raycast(directedray, out hitinfo, 1f))
        {
            if (hitinfo.collider.CompareTag("PawnCube"))
            {
                //pawn is in front of the cube
                setPawnCubePositionWithRelativeToPlayerCube(directedray.direction);
                AttachCube(hitinfo.collider.gameObject);
            }

        }        
    }  
    public void TakeInput()
    {
        dir = Vector3.zero;
    
        if (Input.GetKeyDown(KeyCode.W))
        {
            dir = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            dir = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            dir = Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dir = Vector3.right;
        }
        
       
    }
    public void SetWaypointInInputDirection()
    {  
        Setwaypoint();
    }
    
    public void Setwaypoint()
    {

        ColliderCorrectionValue = 0.1f;
        isHitting = Physics.BoxCast(bx.bounds.center, bx.bounds.size / 2.5f, dir, out hitinfo,Quaternion.identity,50f,layermask,QueryTriggerInteraction.Ignore);
        if (isHitting)
        {
            if (useWallMotion)
            {
                
                    Transform waypointT = WaypointPooler.instance.getWaypointFromPool();
                    if (dir == pawnCubeDirectionWithRespectToPlayerCube || dir == -pawnCubeDirectionWithRespectToPlayerCube)
                    {
                        ColliderCorrectionValue = 0.2f;
                    }
                    waypointT.position = transform.position + (dir * hitinfo.distance) + (-dir * ColliderCorrectionValue);
                    target = waypointT.transform; 
            }
        }
        else
        {
            target = null;
        }
    }
    public void setPawnCubePositionWithRelativeToPlayerCube(Vector3 dir)
    {
          pawnCubeDirectionWithRespectToPlayerCube = dir;

    }
    public void AttachCube(GameObject cube)
    {
        Pawncube = cube;
        Pawncube.transform.SetParent(this.gameObject.transform);
        Pawncube.GetComponent<PawnCubeController>().AttachCube();
        isPawnCubeAttached = true;
        gameManager.IncreaseMoveCounter(); //increases moveCount on attaching cube
        ModifyCollider();

    }
    private void OnMouseDown()
    {
        Detach();
    }
    public void Detach()
    {
        if (isPawnCubeAttached)
        {

            DetachPawnCube();
            DetachPlayerCube();

        }
    }
    private void DetachPlayerCube()
    {
        pawnCubeDirectionWithRespectToPlayerCube = Vector3.zero;
        isPawnCubeAttached = false;
        ResetCollider();
        gameManager.IncreaseMoveCounter(); // increases moveCount on Detaching the player
    }
    private void DetachPawnCube()
    {

        Pawncube.transform.parent = null; 
        Pawncube.GetComponent<PawnCubeController>().DetachCube();
        Pawncube = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if(target!=null)
        {
            Gizmos.DrawSphere(target.position, 0.25f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(lastStaticPosition + dir * hitinfo.distance, bx.size);
        
        Gizmos.DrawRay(transform.position, dir * (hitinfo.distance - Vector3.Distance(lastStaticPosition, transform.position)));

    }
    private void ModifyCollider()
    {
        if(pawnCubeDirectionWithRespectToPlayerCube==transform.forward)
        {
            bx.center = new Vector3(bx.center.x, bx.center.y, 0.5f);
            bx.size = new Vector3(bx.size.x, bx.size.y, 2f);
        }
        else if (pawnCubeDirectionWithRespectToPlayerCube == -transform.forward)
        {
            bx.center = new Vector3(bx.center.x, bx.center.y, -0.5f);
            bx.size = new Vector3(bx.size.x, bx.size.y, 2f);
        }
        else if (pawnCubeDirectionWithRespectToPlayerCube == transform.right)
        {
            bx.center = new Vector3(0.5f, bx.center.y, bx.center.z);
            bx.size = new Vector3(2f, bx.size.y,bx.size.z);
        }
        else if (pawnCubeDirectionWithRespectToPlayerCube == -transform.right)
        {
            bx.center = new Vector3(-0.5f, bx.center.y,bx.center.z);
            bx.size = new Vector3(2f, bx.size.y,bx.size.z);
        }

    }
     private void ResetCollider()
    {
        bx.center = new Vector3(0, 0, 0);
        bx.size = new Vector3(1, 1, 1);
        
    }
    

}
