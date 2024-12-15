using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace App.Resource.Scripts.Obj
{ 
    public class BulletSpawner : NetworkBehaviour
    {

        [SerializeField] public NetworkVariable<int> _ammo = new NetworkVariable<int>(4);
        [SerializeField] private Transform _startingPoint;
        [SerializeField] private NetworkObject _ProjectilePrefab;


        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void FireProjectileRpc(RpcParams rpcParams = default)
        {
            if (_ammo.Value > 0)
            {
                NetworkObject newProjectile = NetworkManager.Instantiate(_ProjectilePrefab, _startingPoint.position, _startingPoint.rotation);

                newProjectile.SpawnWithOwnership(rpcParams.Receive.SenderClientId);

                _ammo.Value--;
            }
        }



    }
}