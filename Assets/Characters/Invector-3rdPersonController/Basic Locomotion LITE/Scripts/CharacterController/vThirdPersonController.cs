using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Invector.CharacterController
{
   
    public class vThirdPersonController : vThirdPersonAnimator
    {
        private GameObject closestOccupiedTower;
        public static bool IsTired = false;

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
        public GameObject shockWave;

        protected virtual void Start()
        {

        }

        public virtual void StrongAttack()
        {
            if (/*!isGrounded || */animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack") || animator.GetCurrentAnimatorStateInfo(1).IsTag("Block")) return;
            if (isArmed)
            {
                if (PlayerCharacterState.DecreaseEnergy(30)) animator.SetTrigger("Attack");
                if (isSprinting) StartCoroutine(JumpAttack());
            }
            else StartCoroutine(TakeWeapon());
        }

        public IEnumerator JumpAttack()
        {
            float range = 6;
            float pushFactor = 7f;
            List<GameObject> Enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Respawn"));

            lockMovement = true;
            Sprint(true);
            yield return new WaitForSeconds(1.7f);
            Sprint(false);
            input = Vector2.zero;
            shockWave.GetComponentInChildren<ParticleSystem>().Play();
            foreach (GameObject e in Enemies)
                if (Vector3.Distance(Weapon.transform.position, e.transform.position) <= range)
                {
                    float dst = Vector3.Distance(e.transform.position, Weapon.transform.position);
                    Vector3 dir = e.transform.position - Weapon.transform.position;
                    StartCoroutine(CanonTower.MoveOverSeconds(e, new Vector3(e.transform.position.x + pushFactor * dir.x / dst, e.transform.position.y, e.transform.position.z + pushFactor * dir.z / dst), 0.5f));
                    e.gameObject.GetComponent<Animator>().SetBool("isStunned", true);
                    e.gameObject.SendMessage("DealCriticlaDamage", new DamageParameters
                    {
                        damageAmount = 500f,
                        duration = 4f,
                        slowDownFactor = 0f,
                        criticProbability = 10,
                        showPopup = true,
                        damageSourceObject = GameObject.FindGameObjectWithTag("Player")
                    });
                }

            yield return new WaitForSeconds(1f);
            lockMovement = false;
        }

        public virtual void LightAttack()
        {
            if (!isGrounded || animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack") || animator.GetCurrentAnimatorStateInfo(1).IsTag("Block")) return;
            if (isArmed)
            {
                if (PlayerCharacterState.DecreaseEnergy(15)) animator.SetTrigger("LightAttack");
            }
            else StartCoroutine(TakeWeapon());
        }

        public virtual void BlockUp()
        {
            if (!isGrounded || !isArmed && IsTired) return;
            animator.SetBool("Block", true);
            isStrafing = true;
        }

        public virtual void BlockDown()
        {
            animator.SetBool("Block", false);
            isStrafing = false;
        }

        public virtual void Sprint(bool value)
        {
            if (input == Vector2.zero ) return;
            isSprinting = value;
            if (isSprinting) jumpForward = 5f;
            else jumpForward = 3f;
        }

        public IEnumerator TakeWeapon()
        {
            animator.SetTrigger("Axe");
            yield return new WaitForSeconds(0.83f);
            isArmed = !isArmed;
            if (isArmed)
            {
                Weapon.transform.SetParent(Hand.transform);
                Weapon.transform.localPosition = new Vector3(0.00016f, 0.00063f, -0.00089f);
                Weapon.transform.localEulerAngles = new Vector3(13.931f, -134.905f, -94.051f);
                freeRunningSpeed *= armedSlowerFactor;
                freeSprintSpeed *= armedSlowerFactor;
                strafeRunningSpeed *= armedSlowerFactor;
                strafeSprintSpeed *= armedSlowerFactor;
                //Debug.Log("Do ręki");
            }
            else
            {
                Weapon.transform.SetParent(Back.transform);
                Weapon.transform.localPosition = new Vector3(0.00117f, 0.00024f, -0.00195f);
                Weapon.transform.localEulerAngles = new Vector3(-3.305f, 0, 0);
                freeRunningSpeed *= (1f / armedSlowerFactor);
                freeSprintSpeed *= (1f / armedSlowerFactor);
                strafeRunningSpeed *= (1f / armedSlowerFactor);
                strafeSprintSpeed *= (1f / armedSlowerFactor);
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
            bool jumpConditions = isGrounded && !isJumping && PlayerCharacterState.DecreaseEnergy(10);
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