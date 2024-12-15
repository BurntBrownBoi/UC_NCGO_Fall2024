using App.Resource.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace App.Resource.Scripts
{
    public class ProjectileObj : NetworkBehaviour
    {
        [SerializeField] float _speed = 40f;
        [SerializeField] private float _damage = 10;
        [SerializeField] private float destructTime = 5f;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            GetComponent<Rigidbody>().velocity = this.transform.forward * _speed;
            StartCoroutine(AutoDestruct());
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Equals("Player") && other.gameObject.GetComponent<NetworkObject>().OwnerClientId != this.OwnerClientId)
            {
                other.gameObject.GetComponent<HealthNetScript>().DamageObjRpc(_damage);
            }
        }

        private IEnumerator AutoDestruct()
        {
            yield return new WaitForSeconds(destructTime);
            this.NetworkObject.Despawn();
        }


    }
}