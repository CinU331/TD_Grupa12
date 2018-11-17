using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Invector.CharacterController
{
    public class vThirdPersonController : vThirdPersonAnimator
    {
        [Header("--- Weapon Setup ---")]
        public GameObject Weapon;
        public GameObject Hand;
        public GameObject Back;

        protected virtual void Start()
        {
#if !UNITY_EDITOR
                //Cursor.visible = false;
#endif
        }

        public virtual void Attack()
        {
            if (!isGrounded) return;
            animator.SetTrigger("Attack");
        }

        public virtual void Sprint(bool value)
        {
            isSprinting = value;
        }

        public IEnumerator TakeWeapon()
        {
            isArmed = !isArmed;
            yield return new WaitForSeconds(0.83f);
            if (isArmed)
            {
                Weapon.transform.SetParent(Hand.transform);
                Weapon.transform.localPosition = new Vector3(0.00016f, 0.00063f, -0.00089f);
                freeRunningSpeed = 3;
                freeSprintSpeed = 4;
                //Debug.Log("Do ręki");
            }
            else
            {
                Weapon.transform.SetParent(Back.transform);
                Weapon.transform.localPosition = new Vector3(0.00033f, -0.00044f, -0.00172f);
                freeRunningSpeed = 4;
                freeSprintSpeed = 6;
                //Debug.Log("Do pleców");
            }
        }

        public virtual void Strafe()
        {
            if (locomotionType == LocomotionType.OnlyFree) return;
            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            // conditions to do this action
            bool jumpConditions = isGrounded && !isJumping;
            // return if jumpCondigions is false
            if (!jumpConditions) return;
            // trigger jump behaviour
            jumpCounter = jumpTimer;
            isJumping = true;
            // trigger jump animations            
            if (_rigidbody.velocity.magnitude < 1)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", 0.2f);
        }

        public virtual void RotateWithAnotherTransform(Transform referenceTransform)
        {
            var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafeRotationSpeed * Time.fixedDeltaTime);
            targetRotation = transform.rotation;
        }
    }
}