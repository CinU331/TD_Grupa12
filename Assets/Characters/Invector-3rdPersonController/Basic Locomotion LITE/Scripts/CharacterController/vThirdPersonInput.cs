﻿using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

namespace Invector.CharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region variables

        [Header("Default Inputs")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode armedInput = KeyCode.E;
        public KeyCode attackInput = KeyCode.Mouse0;
        public KeyCode blockInput = KeyCode.Mouse1;

        [Header("Camera Settings")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        protected vThirdPersonCamera tpCamera;                // acess camera info        
        [HideInInspector]
        public string customCameraState;                    // generic string to change the CameraState        
        [HideInInspector]
        public string customlookAtPoint;                    // generic string to change the CameraPoint of the Fixed Point Mode        
        [HideInInspector]
        public bool changeCameraState;                      // generic bool to change the CameraState        
        [HideInInspector]
        public bool smoothCameraState;                      // generic bool to know if the state will change with or without lerp  
        [HideInInspector]
        public bool keepDirection;                          // keep the current direction in case you change the cameraState

        protected vThirdPersonController cc;                // access the ThirdPersonController component     
        private float pom = 20;
        private bool pom2 = false;

        #endregion

        protected virtual void Start()
        {
            CharacterInit();
        }

        protected virtual void CharacterInit()
        {
            cc = GetComponent<vThirdPersonController>();
            if (cc != null)
                cc.Init();

            tpCamera = FindObjectOfType<vThirdPersonCamera>();
            if (tpCamera) tpCamera.SetMainTarget(this.transform);
        }

        protected virtual void LateUpdate()
        {
            if (cc == null) return;             // returns if didn't find the controller		    
            InputHandle();                      // update input methods
            UpdateCameraStates();               // update camera states
        }

        protected virtual void FixedUpdate()
        {
            cc.AirControl();
            CameraInput();
        }

        protected virtual void Update()
        {
            cc.UpdateMotor();                   // call ThirdPersonMotor methods               
            cc.UpdateAnimator();                // call ThirdPersonAnimator methods		               
        }

        protected virtual void InputHandle()
        {
            if (!vThirdPersonMotor.lockMovement)
            {
                MoveCharacter();
                SprintInput();
                StrafeInput();
                ArmedInput();
                AttackInput();
                BlockInput();
                JumpInput();
                GetComponent<PlayerCharacterState>().isRotating = false;
            }
            if (!vThirdPersonMotor.lockCamera)
            {
                CameraInput();
            }
            
        }

        #region Basic Locomotion Inputs      

        protected virtual void MoveCharacter()
        {
            vThirdPersonMotor.input.x = Input.GetAxis(horizontalInput);
            vThirdPersonMotor.input.y = Input.GetAxis(verticallInput);
        }

        protected virtual void ArmedInput()
        {
            if (Input.GetKeyDown(armedInput) && cc.isGrounded)
                StartCoroutine(cc.TakeWeapon());
        }

        protected virtual void AttackInput()
        {
            if (Input.GetKeyDown(attackInput) && !pom2) { pom = Time.time; pom2 = true; }
            if ((Time.time - pom) > 0.20 && (Time.time - pom) != Time.time && pom2) { cc.StrongAttack(); pom = 0; pom2 = false; }
            if (Input.GetKeyUp(attackInput) && (Time.time - pom) <= 0.25 && pom2) { cc.LightAttack(); pom = 0; pom2 = false; }
        }

        protected virtual void BlockInput()
        {
            if (Input.GetKeyDown(blockInput))
                cc.BlockUp();
            else if (Input.GetKeyUp(blockInput))
                cc.BlockDown();
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput) && !(PlayerCharacterState.CurrentEnergyPoints < 5) )
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
            if((PlayerCharacterState.CurrentEnergyPoints < 0)) cc.Sprint(false);
        }

        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput))
                cc.Jump();
        }

        #endregion

        #region Camera Methods

        protected virtual void CameraInput()
        {
            if (tpCamera == null)
                return;
            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);

            // tranform Character direction from camera if not KeepDirection
            if (!keepDirection)
                cc.UpdateTargetDirection(tpCamera != null ? tpCamera.transform : null);
            // rotate the character with the camera while strafing        
            RotateWithCamera(tpCamera != null ? tpCamera.transform : null);
        }

        protected virtual void UpdateCameraStates()
        {
            // CAMERA STATE - you can change the CameraState here, the bool means if you want lerp of not, make sure to use the same CameraState String that you named on TPCameraListData
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void RotateWithCamera(Transform cameraTransform)
        {
            if (cc.isStrafing && !vThirdPersonMotor.lockMovement)
            {
                cc.RotateWithAnotherTransform(cameraTransform);
            }
        }

        #endregion     
    }
}