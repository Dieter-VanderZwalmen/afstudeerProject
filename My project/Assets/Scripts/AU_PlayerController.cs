using Photon.Pun;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class AU_PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] bool hasControl;
    public static AU_PlayerController localPlayer;

    //Components
    Rigidbody myRB;
    Animator myAnim;
    Transform myAvatar;

    //Player movement
    [SerializeField] InputAction WASD;
    Vector2 movementInput;
    [SerializeField] float movementSpeed;

    float direction = 1;
    //Player Color
    static Color myColor;
    SpriteRenderer myAvatarSprite;

    //Role
    [SerializeField] bool isImposter;
    [SerializeField] InputAction KILL;
    float killInput;

    List<AU_PlayerController> targets;
    [SerializeField] Collider myCollider;

    bool isDead;

    [SerializeField] GameObject bodyPrefab;

    public static List<Transform> allBodies;

    List<Transform> bodiesFound;

    [SerializeField] InputAction REPORT;
    [SerializeField] LayerMask ignoreForBody;

    //Interaction
    AU_Interactable tempInteractable;
    [SerializeField] InputAction MOUSE;
    Vector2 mousePositionInput;
    Camera myCamera;
    [SerializeField] InputAction INTERACTION;
    [SerializeField] LayerMask interactLayer;

    //Networking
    PhotonView myPV;
    [SerializeField] GameObject lightMask;
    [SerializeField] lightcaster myLightCaster;

    [SerializeField] private TextMeshProUGUI nameText;
    public string myNickname;

    private void Awake()
    {
        KILL.performed += KillTarget;
        REPORT.performed += ReportBody;
        INTERACTION.performed += Interact;
    }

    private void OnEnable()
    {
        WASD.Enable();
        KILL.Enable();
        REPORT.Enable();
        MOUSE.Enable();
        INTERACTION.Enable();
    }

    private void OnDisable()
    {
        WASD.Disable();
        KILL.Disable();
        REPORT.Disable();
        MOUSE.Disable();
        INTERACTION.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        myPV = GetComponent<PhotonView>();

        if(myPV.IsMine)
        {
            localPlayer = this;
        }
        myCamera = transform.GetChild(1).GetComponent<Camera>();
        targets = new List<AU_PlayerController>();
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        myAvatar = transform.GetChild(0);
        myAvatarSprite = myAvatar.GetComponent<SpriteRenderer>();

        //set Color to white
        
        SetColor(Color.white);
        SetColorAsNickname("White");
        SetName();

        if (!myPV.IsMine)
        {
            //disable child object named fov
            transform.GetChild(3).gameObject.SetActive(false);
            //set the layer to behindMask
            //AU_Player (TransparentFX)
            //Player
            //Canvas_NameTag
            // -> Text_NameTag
            int behindMaskLayerNumber = LayerMask.NameToLayer("BehindMask");
            int TransparentFXLayerNumber = LayerMask.NameToLayer("TransparentFX");

            myPV.gameObject.layer = TransparentFXLayerNumber;
            myPV.gameObject.transform.GetChild(0).gameObject.layer = behindMaskLayerNumber;
            myPV.gameObject.transform.GetChild(2).gameObject.layer = behindMaskLayerNumber;
            myPV.gameObject.transform.GetChild(2).GetChild(0).gameObject.layer = behindMaskLayerNumber;

            // myPV.gameObject.transform.GetChild(0).GetChild(0).gameObject.layer = behindMaskLayerNumber;

            myCamera.gameObject.SetActive(false);
            //lightMask.SetActive(false);
            //myLightCaster.enabled = false;
            return;
        }
        if (myColor == Color.clear)
            myColor = Color.white;

        myAvatarSprite.color = myColor;


        if (sceneName == "StartGame")
        {
            myCamera.gameObject.SetActive(false);
            
        }

        

        allBodies = new List<Transform>();

        bodiesFound = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myAvatar.localScale = new Vector2(direction, 1);

        if (!myPV.IsMine)
            return;

        movementInput = WASD.ReadValue<Vector2>();
        myAnim.SetFloat("Speed", movementInput.magnitude);
        if (movementInput.x != 0)
        {
            //Draai enkel het object Player.
            //Blijf ook gedraaid staan.
            
            direction = Mathf.Sign(movementInput.x);
            Transform player = transform.Find("Player");
            if (player != null)
            {
                player.localScale = new Vector3(direction, 1, 1);
            }


        }

        /*if (allBodies.Count > 0)
        {
            BodySearch();
        }*/

        mousePositionInput = MOUSE.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!myPV.IsMine)
            return;
        myRB.velocity = movementInput * movementSpeed;
    }

    private void SetName(){
       nameText.text =  PhotonNetwork.LocalPlayer.NickName ;
    }

    public void SetColorAsNickname(string nickname){
        PhotonNetwork.LocalPlayer.NickName = nickname;
        nameText.text =  PhotonNetwork.LocalPlayer.NickName ;

        Debug.Log("My Nickname: " + PhotonNetwork.LocalPlayer.NickName);
    }

    public void SetColor(Color newColor)
    {
        myColor = newColor;
        if (myAvatarSprite != null)
        {
            myAvatarSprite.color = myColor;
        }
    }

    public void SetRole(bool newRole)
    {
        isImposter = newRole;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AU_PlayerController tempTarget = other.GetComponent<AU_PlayerController>();
            if (isImposter)
            {
                if (tempTarget.isImposter)
                    return;
                else
                {
                    targets.Add(tempTarget);
                }
            }
        }
        if (other.tag == "Interactable")
        {
            tempInteractable = other.GetComponent<AU_Interactable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            AU_PlayerController tempTarget = other.GetComponent<AU_PlayerController>();
            if (targets.Contains(tempTarget))
            {
                    targets.Remove(tempTarget);
            }
        }
        if (other.tag == "Interactable")
        {
            tempInteractable = null;
        }
    }

    public void ReduceVision(){
        //get component Fov
        Debug.Log("AU_PlayerController: ReduceVision");
       
        //GetGameObject<Fov>().ReduceVision();
        //get the child object named fov and call the function ReduceVision
        myPV.gameObject.transform.GetChild(3).gameObject.GetComponent<Fov>().ReduceVision();
    }
    private void KillTarget(InputAction.CallbackContext context)
    {
        if (!myPV.IsMine)
            return;
        if (!isImposter)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            //Debug.Log(targets.Count);
            if (targets.Count == 0)
                return;
            else
            {
                if (targets[targets.Count - 1].isDead)
                    return;

                transform.position = targets[targets.Count - 1].transform.position;
                //targets[targets.Count - 1].Die();  --> non multiplayer
                targets[targets.Count - 1].myPV.RPC("RPC_Kill", RpcTarget.All);
                targets.RemoveAt(targets.Count - 1);
            }
        }
    }

    public void KillTarget()
    {
        if (!myPV.IsMine)
            return;
        if (!isImposter)
            return;

            //Debug.Log(targets.Count);
            if (targets.Count == 0)
                return;
            else
            {
                if (targets[targets.Count - 1].isDead)
                    return;

                transform.position = targets[targets.Count - 1].transform.position;
                //targets[targets.Count - 1].Die();  --> non multiplayer
                targets[targets.Count - 1].myPV.RPC("RPC_Kill", RpcTarget.All);
                targets.RemoveAt(targets.Count - 1);
            
        }
    }


    [PunRPC]
    void RPC_Kill()
    {
        Die();
    }

    public void Die()
    {
        if (!myPV.IsMine)
            return;

        //AU_Body tempBody = Instantiate(bodyPrefab, transform.position, transform.rotation).GetComponent<AU_Body>();
        AU_Body tempBody = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AU_Body"), transform.position, transform.rotation).GetComponent<AU_Body>();
        tempBody.SetColor(myAvatarSprite.color);

        isDead = true;

        myAnim.SetBool("IsDead", isDead);
        gameObject.layer = 9;
        myCollider.enabled = false;
    }

    void BodySearch()
    {
        foreach(Transform body in allBodies)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, body.position - transform.position);
            Debug.DrawRay(transform.position, body.position - transform.position, Color.cyan);
            if(Physics.Raycast(ray, out hit, 1000f, ~ignoreForBody))
            {
                
                if (hit.transform == body)
                {
                    //Debug.Log(hit.transform.name);
                    //Debug.Log(bodiesFound.Count);
                    if (bodiesFound.Contains(body.transform))
                        return;
                    bodiesFound.Add(body.transform);
                }
                else
                {                   
                    bodiesFound.Remove(body.transform);
                }
            }
        }
    }

    private void ReportBody(InputAction.CallbackContext obj)
    {
        if (bodiesFound == null)
            return;
        if (bodiesFound.Count == 0)
            return;
        Transform tempBody = bodiesFound[bodiesFound.Count - 1];
        allBodies.Remove(tempBody);
        bodiesFound.Remove(tempBody);
        tempBody.GetComponent<AU_Body>().Report();
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            RaycastHit hit;
            Ray ray = myCamera.ScreenPointToRay(mousePositionInput);
            if (Physics.Raycast(ray, out hit,interactLayer))
            {
                if (hit.transform.tag == "Interactable")
                {
                    Debug.Log("Interactable");
                    AU_Interactable temp = hit.transform.GetComponent<AU_Interactable>();
                    temp.PlayMiniGame();
                }
            }
        } 
    }

    public void ReportBody()
    {
        if (bodiesFound == null)
            return;
        if (bodiesFound.Count == 0)
            return;
        Transform tempBody = bodiesFound[bodiesFound.Count - 1];
        allBodies.Remove(tempBody);
        bodiesFound.Remove(tempBody);
        tempBody.GetComponent<AU_Body>().Report();
    }

    public void Interact()
    {
        if (tempInteractable != null)
            {
                tempInteractable.PlayMiniGame();
            }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(direction);
        }
        else
        {
            direction = (float)stream.ReceiveNext();
        }
    }


   
}