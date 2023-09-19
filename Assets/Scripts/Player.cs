using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public float movementSpeed = 50f;
    public float rotationSpeed = 130f;
    public NetworkVariable<Color> playerColorNetVar = new NetworkVariable<Color>(Color.red);

    private Camera playerCamera;
    private GameObject playerBody;

    public float xBoundry = 5f;
    public float xNegBoundry = 5f;
    public float zBoundry = 5f;
    public float zNegBoundry = 5f;


    private void Start()
    {
       playerCamera = transform.Find("Camera").GetComponent<Camera>();
       playerCamera.enabled = IsOwner;
       playerCamera.GetComponent<AudioListener>().enabled = IsOwner;

        playerBody = transform.Find("Body").gameObject;
        ApplyColor();
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (IsClient)
            {
                PlayerBoundries();
            }
            OwnerHandleInput();
        }
        
        
    }

    public void PlayerBoundries()
    {
        if(transform.position.x < xBoundry)
        {
            transform.position = new Vector3(xBoundry, transform.position.y, 0);
        }else if(transform.position.x <= xNegBoundry){
            transform.position = new Vector3(xNegBoundry, transform.position.y, 0);
        }

        if (transform.position.z < zBoundry)
        {
            transform.position = new Vector3(zBoundry, transform.position.y, 0);
        }
        else if (transform.position.z <= zNegBoundry)
        {
            transform.position = new Vector3(zNegBoundry, transform.position.y, 0);
        }
    }

    private void OwnerHandleInput()
    {
        Vector3 movement = CalcMovement();
        Vector3 rotation = CalcRotation();

        if(movement != Vector3.zero || rotation != Vector3.zero)
        {
            MoveServerRPC(movement, rotation);
        }
    }

    public void ApplyColor()
    {
        playerBody.GetComponent<MeshRenderer>().material.color = playerColorNetVar.Value;
    }

    [ServerRpc]
    public void MoveServerRPC(Vector3 movement, Vector3 rotation)
    {
        transform.Translate(movement);
        transform.Rotate(rotation);
    }

    // Rotate around the y axis when shift is not pressed
    private Vector3 CalcRotation()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        Vector3 rotVect = Vector3.zero;
        if (!isShiftKeyDown)
        {
            rotVect = new Vector3(0, Input.GetAxis("Horizontal"), 0);
            rotVect *= rotationSpeed * Time.deltaTime;
        }
        return rotVect;
    }


    // Move up and back, and strafe when shift is pressed
    private Vector3 CalcMovement()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float x_move = 0.0f;
        float z_move = Input.GetAxis("Vertical");

        if (isShiftKeyDown)
        {
            x_move = Input.GetAxis("Horizontal");
        }

        Vector3 moveVect = new Vector3(x_move, 0, z_move);
        moveVect *= movementSpeed * Time.deltaTime;

        return moveVect;
    }
}
