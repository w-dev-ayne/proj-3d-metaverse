using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        public enum PlayMode
        {
            Pc,
            Mobile
        }

        public PlayMode playMode = PlayMode.Mobile;
        
        public Joystick joyStick;
        
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;

        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            InitializeTpCamera();
        }

        protected virtual void FixedUpdate()
        {
            if (cc == null)
            {
                InitilizeController();
            }
            else
            {
                cc.UpdateMotor();               // updates the ThirdPersonMotor methods
                cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
                cc.ControlRotationType();  
            } // handle the controller rotation type
        }

        protected virtual void Update()
        {
            if (cc == null)
                return;
            
            InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();
            joyStick = GameObject.FindObjectOfType<Joystick>();

            if (cc != null)
            {
                Debug.Log("CC Init");
                cc.Init();
            }
        }

        protected virtual void InitializeTpCamera()
        {
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

        protected virtual void InputHandle()
        {
            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
        }

        public virtual void MoveInput()
        {
            if (playMode == PlayMode.Mobile)
            {
                cc.input.x = joyStick.Horizontal;
                cc.input.z = joyStick.Vertical;
            }
            else
            {
                cc.input.x = Input.GetAxis(horizontalInput);
                cc.input.z = Input.GetAxis(verticallInput);
            }
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            if (playMode == PlayMode.Mobile)
            {
                if (Input.touchCount > 0)
                {
                    foreach(Touch touch in Input.touches)
                    {
                        if (!IsTouchInJoystick(touch))
                        {
                            var X = (touch.deltaPosition.x / Screen.width) * (100);
                            var Y = (touch.deltaPosition.y / Screen.height) * (100);
                            tpCamera.RotateCamera(X, Y);
                        }
                    }
                }
            }
            else
            {
                var Y = Input.GetAxis(rotateCameraYInput);
                var X = Input.GetAxis(rotateCameraXInput);
                
                tpCamera.RotateCamera(X, Y);
            }
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            Debug.Log($"{cc.name}");
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
        }

        public void OnClickJumpButton()
        {
            if(JumpConditions())
                cc.Jump();
        }

        #endregion

        private bool IsTouchInJoystick(Touch touch)
        {
            return joyStick.joystickPointer == touch.fingerId;
        }
    }
}