using UnityEngine;

public class RoomCheck : MonoBehaviour
{
    public bool wasVisited, itemRoom, bossTeleportRoom;
    public enum DoorDirection { NSLO, NSO, NSL, NLO, SLO, SO, SL, LO, NO, NL, SN, N, S, L, O, MAIN};
	public DoorDirection doorDirection;
	public GameObject minimapIcon;
	private void Start() { minimapIcon = this.transform.GetChild(0).gameObject; }
}