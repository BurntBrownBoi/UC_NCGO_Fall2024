using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace App.Resource.Scripts.Obj
{
    public class ammoPickup : NetworkBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!IsServer) return;
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<BulletSpawner>()._ammo.Value++;

                Destroy(gameObject);
            }
        }
    }
}