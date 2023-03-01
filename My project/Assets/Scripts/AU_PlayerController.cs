using Photon.Pun;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AU_PlayerController : MonoBehaviour, IPunObservable
{
    [SerializeField] bool hasControl;
    public static AU_PlayerController localPlayer;
    public string nickName;
    public int actorNumber;

    private static VotingManager votingManager = new VotingManager();
    private static AU_GameController gameController = new AU_GameController();

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

    public bool isDead;

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
    public PhotonView myPV; 
    [SerializeField] GameObject lightMask;
    [SerializeField] lightcaster myLightCaster;

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
            this.actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Debug.Log(AU_GameController.allPlayers);
        }
        myCamera = transform.GetChild(2).GetComponent<Camera>();
        Debug.Log(myCamera);
        targets = new List<AU_PlayerController>();
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        myAvatar = transform.GetChild(0);
        myAvatarSprite = myAvatar.GetComponent<SpriteRenderer>();
        if (!myPV.IsMine)
        {
            myCamera.gameObject.SetActive(false);
            lightMask.SetActive(false);
            myLightCaster.enabled = false;
            return;
        }
        if (myColor == Color.clear)
            myColor = Color.white;
        myAvatarSprite.color = myColor;

        if (sceneName == "StartGame")
        {
            myCamera.gameObject.SetActive(false);
            lightMask.SetActive(false);
            myLightCaster.enabled = false;
        }

        allBodies = new List<Transform>();

        bodiesFound = new List<Transform>();

        gameController.AddToPlayerList(this);
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
            direction = Mathf.Sign(movementInput.x);
        }

        if(allBodies.Count > 0)
        {
            BodySearch();
        }

        mousePositionInput = MOUSE.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!myPV.IsMine)
            return;
        myRB.velocity = movementInput * movementSpeed;
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
        {
            return;
        }
        if (!isImposter)
        {
            return;
        }
        //Debug.Log(targets.Count);
        if (targets.Count == 0)
        {
            return;
        }
        else
        {
            if (targets[targets.Count - 1].isDead)
            {
                return;
            }
            transform.position = targets[targets.Count - 1].transform.position;
            //targets[targets.Count - 1].Die();  --> non multiplayer
            targets[targets.Count - 1].myPV.RPC("RPC_Kill", RpcTarget.All, targets[targets.Count - 1].myPV.Owner.ActorNumber);
            targets.RemoveAt(targets.Count - 1);
        }
    }
        
    [PunRPC]
    void RPC_Kill(int actorNumber)
    {
        gameController.AddToBodiesFoundActorNumber(actorNumber);
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
            if (Vector2.Distance(transform.position, body.position) < 5f)
            {
                if (!bodiesFound.Contains(body))
                {
                    bodiesFound.Add(body);
                }
            }
            else
            {
                if (bodiesFound.Contains(body))
                {
                    bodiesFound.Remove(body);
                }
            }
        }
    }

    public void ReportBody(InputAction.CallbackContext obj)
    {
        if (gameController.bodiesFoundActorNumber == null)
        {    
            Debug.Log("bodiesfound is null");
            return;
        }
        if (gameController.bodiesFoundActorNumber.Count == 0)
        {
            Debug.Log("bodiesfound is 0");
            return;
        }
        Transform tempBody = bodiesFound[bodiesFound.Count - 1];
        allBodies.Remove(tempBody);
        bodiesFound.Remove(tempBody);
        tempBody.GetComponent<AU_Body>().Report();
        votingManager.DeadBodyReported(myPV, gameController.bodiesFoundActorNumber[gameController.bodiesFoundActorNumber.Count - 1]);
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
        if (gameController.bodiesFoundActorNumber == null)
        {    
            Debug.Log("bodiesfound is null");
            return;
        }
        if (gameController.bodiesFoundActorNumber.Count == 0)
        {
            Debug.Log("bodiesfound is 0");
            return;
        }
        Transform tempBody = bodiesFound[bodiesFound.Count - 1];
        allBodies.Remove(tempBody);
        bodiesFound.Remove(tempBody);
        tempBody.GetComponent<AU_Body>().Report();
        votingManager.DeadBodyReported(myPV, gameController.bodiesFoundActorNumber[gameController.bodiesFoundActorNumber.Count - 1]);
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
            stream.SendNext(isImposter);
        }
        else
        {
            direction = (float)stream.ReceiveNext();
            this.isImposter = (bool)stream.ReceiveNext();
        }
    }

    public void BecomeImposter(int ImposterNumber)
    {
        if(PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[ImposterNumber])
        {
            isImposter = true;
        }
    }

    [PunRPC]
    void RPC_DeadBodyReported(int actorNumber)
    {
        //load the votingscreen for all players through the photonnetwork
        PhotonNetwork.LoadLevel("VotingScreen");
    }
}