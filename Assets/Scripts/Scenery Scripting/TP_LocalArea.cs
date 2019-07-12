using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Handles "teleporting" within a local area.
 * 
 * Requires a StartHandling object already present in scene to function.
 **/
public class TP_LocalArea : MonoBehaviour
{
    [SerializeField]
    private GameObject[] listOfTeleportPositions;

    [SerializeField]
    private int targetTeleport;

    void Awake()
    {
        AreaStart _areaStart = (AreaStart)GameObject.Find("StartHandling").GetComponent<AreaStart>();
        listOfTeleportPositions = _areaStart.getEntryPoints();
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERED TRIGGER");
        if (other.gameObject.name == "Player")
        {
            Debug.Log("AS PLAYER");
            //GameObject _player = GameObject.Find("Player");
            GameObject _player = other.gameObject;
            _player.transform.position = listOfTeleportPositions[targetTeleport].transform.position;
            _player.transform.rotation = listOfTeleportPositions[targetTeleport].transform.rotation;
        }
    }
}
