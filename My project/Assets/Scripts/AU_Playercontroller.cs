using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AU_Playercontroller : MonoBehaviour
{
    //Components
    Rigidbody myRB;
    Transform myAvatar;
    //Player movement
    [SerializeField] InputAction ZQSD;
    Vector2 movementInput;
    [SerializeField] float movementSpeed;


    private void OnEnable()
    {
        ZQSD.Enable();

    }

    private void OnDisable()
    {
        ZQSD.Disable();

    }


    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();

        myAvatar = transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        movementInput = ZQSD.ReadValue<Vector2>();

        if (movementInput.x != 0)
        {
            myAvatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
        }
    }

    private void FixedUpdate()
    {
        myRB.velocity = movementInput * movementSpeed;
    }

}