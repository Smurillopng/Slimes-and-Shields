using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [ReadOnly] public List<GameObject> roomList = new List<GameObject>();
	public GameObject specialItem;
	public GameObject pedestal;
	public GameObject bossTeleporter;
    public GameObject minimapCam, minimapTarget, icon, iconTarget;

    public void AddRoom(GameObject room) { roomList.Add(room); }
        
    public void SpawnItem(GameObject room)
    {
	    var position = room.transform.position;
        Instantiate(pedestal, position, pedestal.transform.rotation);
	    Instantiate(specialItem, new Vector3(position.x, position.y + 1, position.z), Quaternion.identity);
    }
        
    public void SpawnBossTeleport(GameObject room)
    {
        var position = room.transform.position;
        Instantiate(bossTeleporter, new Vector3(position.x, position.y + 1, position.z), Quaternion.identity);
    }

    public void SetItemRoom(int iRoomQtt)
    {
        for (var i = 0; i < iRoomQtt; i++)
        {
            var randomRoom =Random.Range(1, roomList.Count - 1);
            roomList[randomRoom].GetComponent<RoomCheck>().itemRoom = true;
            roomList[randomRoom].GetComponent<RoomCheck>().wasVisited = true;
        }
    }
      
    public void SetBossRoom()
    {
        var randomBool = Random.value > 0.5;
        switch (randomBool)
        {
            case true:
                roomList[0].GetComponent<RoomCheck>().bossTeleportRoom = true;
                roomList[0].GetComponent<RoomCheck>().wasVisited = true;
                break;
            case false:
                roomList[^1].GetComponent<RoomCheck>().bossTeleportRoom = true;
                roomList[^1].GetComponent<RoomCheck>().wasVisited = true;
                break;
        }
    }

    public void StartMinimap()
    {
        minimapCam = GameObject.FindGameObjectWithTag("Minimap");
        minimapTarget = GameObject.FindGameObjectWithTag("MinimapTarget");
        icon = GameObject.FindGameObjectWithTag("Icon");
        iconTarget = GameObject.FindGameObjectWithTag("IconTarget");
        icon.transform.position = iconTarget.transform.position;
        minimapCam.transform.position = minimapTarget.transform.position;
    }
}