using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;

public class PlayerScript : NetworkBehaviour
{
    public Animator PlayerAnimator;
    float turnSpeed= 220f;
    public float acceleration = 0.1f;
    private float current_speed = 2;
    public AudioSource src;
    public AudioClip shooting;
    


    private float currentHorizontalValue;
    private float currentVerticalValue;
    [SerializeField] float valueSmoothing;
    // Start is called before the first frame update
    public float zoomFOV = 30f;
    private bool isZoomed = false;
    public Transform playerCamera;
    public float normalFOV = 60f;
    [SerializeField] TwoBoneIKConstraint secondHand;
    NetworkVariable<int> rigWeight = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    void Start()
    {
        rigWeight.OnValueChanged += (oldval, newval) =>
        {
            secondHand.weight = newval;
        };
        PlayerAnimator = GetComponent<Animator>();
        if (!IsOwner)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            rigWeight.Value = secondHand.weight == 0 ? 1 : 0;
        }
        float targetSpeedVertical = Input.GetAxis("Vertical");
        float targetSpeedHorizontal = Input.GetAxis("Horizontal");

        PlayerAnimator.SetBool("Aim", false);
        PlayerAnimator.SetBool("Shoot", false);

        bool forward = Input.GetKey("w");
        bool back = Input.GetKey("s");
        bool left = Input.GetKey("a");
        bool right = Input.GetKey("d");
        

        currentHorizontalValue = Mathf.Lerp(currentHorizontalValue, targetSpeedHorizontal, Time.deltaTime * valueSmoothing);
        currentVerticalValue = Mathf.Lerp(currentVerticalValue, targetSpeedVertical, Time.deltaTime * valueSmoothing);

        PlayerAnimator.SetFloat("Y", currentVerticalValue);
        PlayerAnimator.SetFloat("X", currentHorizontalValue);

      
        if (forward)
        {
            transform.position += current_speed * transform.forward * Time.deltaTime;
        }
        if (back)
        {
            transform.position -= current_speed * transform.forward * Time.deltaTime;
        }
        if (left)
        {
            transform.position -= current_speed * transform.right * Time.deltaTime;
        }
        if (right)
        {
            transform.position += current_speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isZoomed = !isZoomed;

            playerCamera.GetComponent<Camera>().fieldOfView = isZoomed ? zoomFOV : normalFOV;
            PlayerAnimator.SetBool("Aim", true);

        }
      
    }

   

}
