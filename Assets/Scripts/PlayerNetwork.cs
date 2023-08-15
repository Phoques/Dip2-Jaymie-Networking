using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//NetworkBehaviour derives from Monobehaviour.
public class PlayerNetwork : NetworkBehaviour
{

    void Update()
    {
        //This should stop the host moving the client. If it isnt the host / owner, dont do anything.
        if (!IsOwner) { return; }



        Vector3 _moveDirection = new Vector3(0,0,0);

        if (Input.GetKey(KeyCode.W))
        {
            _moveDirection.z = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _moveDirection.z = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _moveDirection.x = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _moveDirection.x = 1;
        }

        float _moveSpeed = 5;
        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;

    }
}
