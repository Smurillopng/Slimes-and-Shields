using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomController : MonoBehaviour
{
	private Node[,] _grid; // Array de nodes
	public int currentLevel; 
	private readonly List<Node> _dungeon = new List<Node>(); // Lista de nodes que formam o labirinto
	private readonly Stack<Node> _procedural = new Stack<Node>(); // Pilha de nodes que formam o labirinto
    public GameObject[] lvlRooms; // Array de prefabs de parada
    public GameObject mainRoom;
    private GameObject _player, _playerSpawn;
	private const int Espacamento = 20; // Distancia entre os nodes
    [ReadOnly] public StageManager stageManager; // Referencia ao script StageManager
    [ReadOnly] public List<GameObject> oDoor, lDoor, sDoor, nDoor, snDoor, nlDoor, noDoor, loDoor, slDoor, soDoor, 
									   sloDoor, nloDoor, nslDoor, nsoDoor, nsloDoor; // Listas de quartos

    private void Awake()
    {
	    stageManager = FindObjectOfType<StageManager>();
	    _player = GameObject.FindGameObjectWithTag("Player");
	    _playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
    }

	private void LoadLists(IEnumerable<GameObject> level)
    {
	    foreach (var room in level)
	    {
		    var doorDirection = room.GetComponent<RoomCheck>().doorDirection;
		    switch (doorDirection)
		    {
			    case RoomCheck.DoorDirection.L:
				    lDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.O:
				    oDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.S:
				    sDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.N:
				    nDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.SN:
				    snDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.NL:
				    nlDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.NO:
				    noDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.LO:
				    loDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.SL:
				    slDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.SO:
				    soDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.SLO:
				    sloDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.NLO:
				    nloDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.NSL:
				    nslDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.NSO:
				    nsoDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.NSLO:
				    nsloDoor.Add(room);
				    break;
			    case RoomCheck.DoorDirection.MAIN:
				    break;
			    default:
				    throw new ArgumentOutOfRangeException();
		    }
	    }
    }

    public class Node 
    {
	    public bool hasRoom { get; set; } // Se o node deve ter uma sala
        public bool walkable { get; set; }
        public GameObject parada { get; set; } = new GameObject(); // GameObject a ser instanciado
        public List<Node> vizinhos { get; set; } = new List<Node>(); // Lista de nodes vizinhos

        public Node(bool walkable,int i,int j) 
        {
	        this.walkable = walkable; 
	        parada.name = $"Node ({i}:{j})"; // Cria o nome do GameObject
	        parada.tag = "Node"; // Cria o tag do GameObject
        }

        public Node(int i,int j) 
        {
	        walkable = true;
            hasRoom = false;
            parada.name = $"Node ({i}:{j})"; // Cria o nome do GameObject
            parada.tag = "Node"; // Cria o tag do GameObject
        }
    }
    
    private List<Node> GetAdjacentNodes(Node[,] m, int i, int j)
    {
	    var l = new List<Node>(); // Lista de nodes vizinhos
	    var norte = false;
	    var sul = false;
	    var leste = false;
	    var oeste = false;
	    
	    //Norte
        if(i - 1 >= 0)
        {
	        if( m[i - 1, j].hasRoom)
            {
		        norte = true;
            }
        }

	    //Sul
        if(i + 1<m.GetLength(0))
        {
	        if( m[i+1, j].hasRoom)
            {
	            sul = true;
            }
        }

	    //Leste
        if(j - 1 >= 0)
        {
	        if( m[i, j - 1].hasRoom)
            {
	            leste = true;
            }
        }

	    //Oeste
        if(j + 1<m.GetLength(1))
        {
	        if( m[i, j + 1].hasRoom)
            {
	            oeste = true;
            }
        }
        
		#region Quartos
	    if(oeste && leste && norte && sul)
	    {
		    _grid[i,j].parada = Instantiate(nsloDoor[Random.Range(0, nsloDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(oeste && leste && norte)
	    { 
	    	_grid[i,j].parada = Instantiate(nslDoor[Random.Range(0, nslDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(oeste && leste && sul)
	    { 
	    	_grid[i,j].parada = Instantiate(nsoDoor[Random.Range(0, nsoDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(oeste && sul && norte)
	    { 
	    	_grid[i,j].parada = Instantiate(sloDoor[Random.Range(0, sloDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(sul && leste && norte)
	    { 
	    	_grid[i,j].parada = Instantiate(nloDoor[Random.Range(0, nloDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(sul && leste)
	    { 
	    	_grid[i,j].parada = Instantiate(noDoor[Random.Range(0, noDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(sul && oeste)
	    { 
	    	_grid[i,j].parada = Instantiate(soDoor[Random.Range(0, soDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(norte && leste)
	    { 
	    	_grid[i,j].parada = Instantiate(nlDoor[Random.Range(0, nlDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(norte && oeste)
	    { 
	    	_grid[i,j].parada = Instantiate(slDoor[Random.Range(0, slDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(sul && norte)
	    { 
	    	_grid[i,j].parada = Instantiate(loDoor[Random.Range(0, loDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(oeste && leste)
	    { 
	    	_grid[i,j].parada = Instantiate(snDoor[Random.Range(0, snDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(sul)
	    { 
	    	_grid[i,j].parada = Instantiate(oDoor[Random.Range(0, oDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(norte)
	    { 
	    	_grid[i,j].parada = Instantiate(lDoor[Random.Range(0, lDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(oeste)
	    { 
	    	_grid[i,j].parada = Instantiate(sDoor[Random.Range(0, sDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
	    else if(leste)
	    { 
	    	_grid[i,j].parada = Instantiate(nDoor[Random.Range(0, nDoor.Count - 1)], new Vector3(i * Espacamento, transform.position.y, j*Espacamento), Quaternion.identity);
	        stageManager.AddRoom(_grid[i,j].parada);
	    }
		#endregion

	    return l; // Retorna a lista de nodes vizinhos
    }
    
	private void ProceduralGetAdjacent(Node[,] m, int i, int j, int lastR, int r)
    {
	    var random = Random.Range(0,10);

        // No acima
	    if(i - 1 >= 0 && random > 5)
        {
            if(m[i - 1, j].walkable)
            {
	            m[i-1, j].walkable= false; 
                _dungeon.Add( m[i - 1, j]); 
            }
		    if(r == lastR)
		    {
		    	m[i-1, j].hasRoom= true;
		    }
        }

        //No abaixo
	    if(i + 1<m.GetLength(0) && random >5)
        {
            if(m[i + 1, j].walkable)
            {
	            m[i + 1, j].walkable= false;
                _dungeon.Add(m[i + 1, j]);
            }
		    if(r == lastR)
		    {
		    	m[i+1, j].hasRoom= true;
		    }
        }

        // No  esquerdo
	    if(j - 1 >= 0 && random <=5)
	    {
            if(m[i, j - 1].walkable)
            {
                m[i, j - 1].walkable= false;
                _dungeon.Add( m[i, j - 1]);
            }
		    if(r == lastR)
		    {
		    	m[i, j-1].hasRoom= true;
		    }
        }

        //No direito
	    if(j + 1<m.GetLength(1) && random <= 5)
        {
            if(m[i, j + 1].walkable)
            {
	            m[i, j + 1].walkable= false;
                _dungeon.Add(m[i, j + 1]);
            }
		    if(r == lastR)
		    {
		    	m[i, j+1].hasRoom= true;
		    }
        }
    }

	public IEnumerator GeraMapa(float time)
    {
	    currentLevel++;
	    LoadLists(lvlRooms);
	    yield return new WaitForSeconds(time);
	    _player.transform.position = _playerSpawn.transform.position;
	    _grid = new[,]
        {
	        {new Node(0,0),        new Node(0,1),         new Node(0,2),         new Node(0,3),         new Node(0,4),         new Node(0,5),        new Node(0,6),         new Node(0,7),         new Node(0,8),         new Node(0,9),         new Node(0,10) },
            {new Node(1,0),        new Node(1,1),         new Node(1,2),         new Node(1,3),         new Node(1,4),         new Node(1,5),        new Node(1,6),         new Node(1,7),         new Node(1,8),         new Node(1,9),         new Node(1,10) },
            {new Node(2,0),        new Node(2,1),         new Node(2,2),         new Node(2,3),         new Node(2,4),         new Node(2,5),        new Node(2,6),         new Node(2,7),         new Node(2,8),         new Node(2,9),         new Node(2,10) },
            {new Node(3,0),        new Node(3,1),         new Node(3,2),         new Node(3,3),         new Node(3,4),         new Node(3,5),        new Node(3,6),         new Node(3,7),         new Node(3,8),         new Node(3,9),         new Node(3,10) },
            {new Node(4,0),        new Node(4,1),         new Node(4,2),         new Node(4,3),         new Node(4,4),         new Node(false,4,5),  new Node(4,6),         new Node(4,7),         new Node(4,8),         new Node(4,9),         new Node(4,10) },
            {new Node(5,0),        new Node(5,1),         new Node(5,2),         new Node(5,3),         new Node(false,5,4),   new Node(false,5,5),  new Node(false,5,6),   new Node(5,7),         new Node(5,8),         new Node(5,9),         new Node(5,10) },
            {new Node(6,0),        new Node(6,1),         new Node(6,2),         new Node(6,3),         new Node(6,4),         new Node(false,6,5),  new Node(6,6),         new Node(6,7),         new Node(6,8),         new Node(6,9),         new Node(6,10) },
            {new Node(7,0),        new Node(7,1),         new Node(7,2),         new Node(7,3),         new Node(7,4),         new Node(7,5),        new Node(7,6),         new Node(7,7),         new Node(7,8),         new Node(7,9),         new Node(7,10) },
            {new Node(8,0),        new Node(8,1),         new Node(8,2),         new Node(8,3),         new Node(8,4),         new Node(8,5),        new Node(8,6),         new Node(8,7),         new Node(8,8),         new Node(8,9),         new Node(8,10) },
            {new Node(9,0),        new Node(9,1),         new Node(9,2),         new Node(9,3),         new Node(9,4),         new Node(9,5),        new Node(9,6),         new Node(9,7),         new Node(9,8),         new Node(9,9),         new Node(9,10) },
            {new Node(10,0),       new Node(10,1),        new Node(10,2),        new Node(10,3),        new Node(10,4),        new Node(10,5),       new Node(10,6),        new Node(10,7),        new Node(10,8),        new Node(10,9),        new Node(10,10) },
        }; // Cria o mapa

	    _grid[5,5].parada = Instantiate(mainRoom, new Vector3(5 * Espacamento, transform.position.y, 5 * Espacamento), Quaternion.identity);
        _procedural.Push(_grid[5,5]);
	    var cavaquinho = 0;
	    const int lastRound = 1;
	    
	    while(cavaquinho<lastRound)
	    {
		    for(var i =0; i < _grid.GetLength(0);i++ ){
	            for(var j =0; j < _grid.GetLength(1);j++)
	            {
		            if (_grid[i, j].walkable || _grid[i, j].hasRoom) continue;
		            ProceduralGetAdjacent(_grid,i,j,lastRound-1,i);
		            _grid[i,j].hasRoom = true;
	            }
		    }
		    cavaquinho++;
	    }
	    
	    // Cria os quartos
	    for(var i =0; i < _grid.GetLength(0);i++ ){
		    for(var j =0; j < _grid.GetLength(1);j++){
		    	if(Equals(_grid[i,j], _grid[5,5]))
		    	{
		    		//
		    	}
			    else if(_grid[i,j].hasRoom){ 
			    	GetAdjacentNodes(_grid,i,j);
		        }
		    }
		    
	    }
		
	    var destroyNode = GameObject.FindGameObjectsWithTag("Node");
	    foreach (var n in destroyNode)
	    {
		    Destroy(n); // Destroi os Nodes
	    }

	    stageManager.StartMinimap();
	    stageManager.SetItemRoom(2);
	    stageManager.SetBossRoom();
	    
	    
    }
}

