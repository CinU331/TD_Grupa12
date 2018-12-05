using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Invector.CharacterController
{
   
    public class vThirdPersonController : vThirdPersonAnimator
    {
        private GameObject closestOccupiedTower;
        public void Update()
        {
            List<GameObject> buildingSpots = new List<GameObject> (GameObject.FindGameObjectsWithTag("BuildingSpot"));
            List<GameObject> filtredList = new List<GameObject>();

            foreach (GameObject buildingSpot in buildingSpots)
            {
                buildingSpot.SendMessage("SetOccupiedVisible", false);
                if (buildingSpot.GetComponent<BuildingSpot>().isOccupied == true)
                    filtredList.Add(buildingSpot);
            }

            if (filtredList.Count != 0)
            {
                filtredList.Sort(delegate (GameObject a, GameObject b)
                {
                    return Vector3.Distance(this.transform.position, a.transform.position)
                    .CompareTo(
                      Vector3.Distance(this.transform.position, b.transform.position));
                });
                if (filtredList.Count != 0)
                {
                    filtredList[0].SendMessage("SetOccupiedVisible", true);
                }
            }
        }

        [Header("--- Weapon Setup ---")]
        public GameObject Weapon;
        public GameObject Hand;
        public GameObject Back;

        protected virtual void Start()
        {

        }

        public virtual void Attack()
        {
            if (!isGrounded) return;
            if (isArmed) animator.SetTrigger("Attack");
            else StartCoroutine(TakeWeapon());
        }

        public virtual void BlockUp()
        {
            if (!isGrounded) return;
            if (isArmed) animator.SetBool("Block", true);
        }

        public virtual void BlockDown()
        {
            animator.SetBool("Block", false);
        }

        public virtual void Sprint(bool value)
        {
            isSprinting = value;
            if (isSprinting) jumpForward += 2f;
            else jumpForward -= 2f;
        }

        public IEnumerator TakeWeapon()
        {
            isArmed = !isArmed;
            yield return new WaitForSeconds(0.83f);
            if (isArmed)
            {
                Weapon.transform.SetParent(Hand.transform);
                Weapon.transform.localPosition = new Vector3(0.00016f, 0.00063f, -0.00089f);
                Weapon.transform.localEulerAngles = new Vector3(13.931f, -134.905f, -94.051f);
                freeRunningSpeed = 3;
                freeSprintSpeed = 4;
                //Debug.Log("Do ręki");
            }
            else
            {
                Weapon.transform.SetParent(Back.transform);
                Weapon.transform.localPosition = new Vector3(0.00117f, 0.00024f, -0.00195f);
                Weapon.transform.localEulerAngles = new Vector3(-3.305f, 0, 0);
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