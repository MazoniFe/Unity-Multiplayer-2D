using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;


    public void SetRoomInfo(RoomInfo roominfo)
    {
        _text.text = roominfo.Name;
    }
}
