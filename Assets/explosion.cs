using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviourPunCallbacks
{
    void Start()
    {
        StartCoroutine(DestroyObject(1.2f));
    }

    IEnumerator DestroyObject(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (PhotonNetwork.IsMasterClient)
        {
        PhotonNetwork.Destroy(this.gameObject);
        }
    }
}

