using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace App.Resource.Scripts.Player
{

    public class PlayerMovement : NetworkBehaviour
    {

        [SerializeField] private Animator _myAnimator;
        [SerializeField] private OwnerNetworkAnimator _ownerAnimator;


        private void Awake()
        {
            if (_myAnimator == null)
            {
                _myAnimator = gameObject.GetComponent<Animator>();
            }
            
            if (_ownerAnimator == null)
            {
                _ownerAnimator = gameObject.GetComponent<OwnerNetworkAnimator>();
            }
        }


        private void FixedUpdate()
        {

            if (!IsOwner) return;
            Vector3 moveDirection = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
            if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
            if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;
            if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;
            
            
            if (Input.GetKey(KeyCode.Space)) _ownerAnimator.SetTrigger("JumpTrigger");

            if (Input.GetKey(KeyCode.Z)) _ownerAnimator.SetTrigger("PunchTrigger");
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _myAnimator.SetBool("IsSprinting", true);
            }
            else
            {
                _myAnimator.SetBool("IsSprinting", false);
            }



            _myAnimator.SetBool("IsWalking", moveDirection.z != 0 || moveDirection.x != 0);



            float moveSpeed = 3f;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }
    }
}