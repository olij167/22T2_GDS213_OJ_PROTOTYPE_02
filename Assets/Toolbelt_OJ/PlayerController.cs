using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;

namespace Toolbelt_OJ
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        PhotonView view;
        public Camera cam;
        public CinemachineVirtualCamera vCam;
        public float baseSpeed = 5f, sprintSpeed, stamina, staminaDecreaseRate, staminaIncreaseRate;
        [HideInInspector] public float moveSpeed, maxStamina;
        //public Rigidbody theRB;
        public float jumpForce = 4f;
        public bool isJumping;
        private CharacterController controller;

        [HideInInspector] public Vector3 moveDirection;
        public float gravScale = 1.0f;

        public List<AudioClip> footstepSounds, jumpSounds;
        AudioSource audioSource;

        private Slider staminaBar;
        private GameObject zoomText;

        // Start is called before the first frame update
        void Start()
        {
            moveSpeed = baseSpeed;

            staminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<Slider>();

            zoomText = staminaBar.transform.GetChild(1).gameObject;

            maxStamina = stamina;

            staminaBar.maxValue = maxStamina;
            //theRB = GetComponent<Rigidbody>();
            controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = true;

            view = GetComponent<PhotonView>();
            audioSource = GetComponent<AudioSource>();

            if (!view.IsMine)
            {
                //Destroy(cam);
                cam.enabled = false;
                vCam.enabled = false;
                cam.GetComponent<AudioListener>().enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //transform.Translate(theRB.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, theRB.velocity.y, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime), Space.Self);

            //if (Input.GetButtonDown("Jump"))
            //{
            //    theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z) * Time.deltaTime;
            //}

            //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0f, Input.GetAxis("Vertical") * moveSpeed);

            if (view.IsMine)
            {
                float yStore = moveDirection.y;
                moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
                moveDirection = moveDirection.normalized * moveSpeed;
                moveDirection.y = yStore;

                if (!controller.isGrounded)
                {
                    isJumping = true;
                }

                if (controller.isGrounded && !isJumping)
                {
                    //moveDirection.y = 0f;
                    if (Input.GetButtonDown("Jump"))
                    {
                        moveDirection.y = jumpForce;

                        if (audioSource.isPlaying)
                        {
                            audioSource.Stop();
                        }
                    }

                    if (controller.velocity.magnitude > 2f && !audioSource.isPlaying)
                    {
                        //audioSource.volume = Random.Range(0.25f, 0.35f);
                        audioSource.pitch = Random.Range(0.8f, 1.1f);
                        audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Count)]);
                    }

                }
                else if (controller.isGrounded && isJumping)
                {
                    audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Count)]);
                    isJumping = false;
                }

                switch (Input.GetKey(KeyCode.LeftShift))
                {
                    case true:
                        {
                            if (stamina >= 0f)
                            {
                                moveSpeed = sprintSpeed;
                                stamina -= Time.deltaTime * staminaDecreaseRate;
                                zoomText.SetActive(true);

                            }
                            else
                            {
                                moveSpeed = baseSpeed;
                                //zoomText.SetActive(false);
                            }
                            break;
                        }
                    case false:
                        {
                            moveSpeed = baseSpeed;
                            zoomText.SetActive(false);

                            if (stamina <= maxStamina)
                            {
                                stamina += Time.deltaTime * staminaIncreaseRate;
                            }
                            break;
                        }
                }

                staminaBar.value = stamina;

                moveDirection.y = moveDirection.y + (Physics.gravity.y * gravScale * Time.deltaTime);
                controller.Move(moveDirection * Time.deltaTime);
            }
            
        }

        [PunRPC]
        public void Teleport(GameObject player, GameObject location)
        {
            player.transform.position = location.transform.position;
        }
    }
}
