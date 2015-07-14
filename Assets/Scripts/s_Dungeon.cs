using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_Dungeon : MonoBehaviour {

	public enum TileTypes{EMPTY, WALL, PLAYER, SPIDER, DAGGER};
	
	public TileTypes[,] m_dungeon;

	public bool pushDownDungeon = false;
	public int currentPushAmmount = 0;
	public float pushDownDelay = 0.15f;

	public GameObject m_wallPrefab;
	public GameObject m_playerPrefab;
	public GameObject m_enemyPrefab;
	public GameObject m_daggerPrefab;

	private List<GameObject> m_enemies;
	private List<GameObject> m_walls;
	private List<GameObject> m_daggers;
	private GameObject m_player;

	private int m_maxWallInLine = 1;
	private int m_maxEnemyInLine = 1;

	private const int m_dungeonHeight = 12;
	private const int m_dungeonWidth = 9;

	private bool m_loadNewArea = false;
	private bool m_playerInput = false;
	
	// Use this for initialization
	void Start () {
		m_dungeon = new TileTypes[m_dungeonWidth,m_dungeonHeight];

		m_enemies = new List<GameObject>();
		m_walls = new List<GameObject>();
		m_daggers = new List<GameObject>();

		//setup random dungeon
		for (int i = 0; i < m_dungeonWidth; ++i)
		{
			for (int j = 0; j < m_dungeonHeight; ++j)
			{
				if (i == 0 || i == m_dungeonWidth - 1)
				{
					m_dungeon[i, j] = TileTypes.WALL;
					GameObject newWall = (GameObject)Instantiate (m_wallPrefab, new Vector3(i, j, -1),transform.rotation);
					m_walls.Add(newWall);
				}
				else
				{
					m_dungeon[i, j] = TileTypes.EMPTY;
				}
			}
		}

		m_dungeon[ (m_dungeonWidth - 1) / 2, 3] = TileTypes.PLAYER;
		m_player = (GameObject)Instantiate (m_playerPrefab, new Vector3((m_dungeonWidth - 1) / 2, 3, -1),transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		pushDownDelay -= Time.deltaTime;

		if (!m_loadNewArea)
		{
			if (!m_playerInput)
			{
				if (m_player.transform.position.y == m_dungeonHeight - 1)
				{
					m_loadNewArea = true;
					pushDownDungeon = true;
					pushDownDelay = 0.15f;
				}
				//wait for the player to make an action

				if ( Input.GetKeyDown(KeyCode.UpArrow) && m_player.transform.position.y < 11)
				{
					//check if the position above the player is clear using the array
					if (m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y + 1] == TileTypes.EMPTY)
					{
						m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.EMPTY;
						m_player.transform.position = new Vector3(m_player.transform.position.x,
						                                          m_player.transform.position.y + 1,
						                                          m_player.transform.position.z);
						m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.PLAYER;
						m_playerInput = true;
					}
				}
				if ( Input.GetKeyDown(KeyCode.DownArrow) && m_player.transform.position.y > 0)
				{
					if (m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y - 1] == TileTypes.EMPTY)
					{
						m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.EMPTY;
						m_player.transform.position = new Vector3(m_player.transform.position.x,
						                                          m_player.transform.position.y - 1,
						                                          m_player.transform.position.z);
						m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.PLAYER;
						m_playerInput = true;
					}
				}
				if ( Input.GetKeyDown(KeyCode.LeftArrow) && m_player.transform.position.x > 1)
				{
					if (m_dungeon[(int)m_player.transform.position.x - 1, (int)m_player.transform.position.y] == TileTypes.EMPTY)
					{
						m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.EMPTY;
						m_player.transform.position = new Vector3(m_player.transform.position.x - 1,
						                                          m_player.transform.position.y,
						                                          m_player.transform.position.z);
						m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.PLAYER;
						m_playerInput = true;
					}
				}
				if ( Input.GetKeyDown(KeyCode.RightArrow) && m_player.transform.position.x < 7)
				{
					if (m_dungeon[(int)m_player.transform.position.x + 1, (int)m_player.transform.position.y] == TileTypes.EMPTY)
					{
						m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.EMPTY;
						m_player.transform.position = new Vector3(m_player.transform.position.x + 1,
						                                          m_player.transform.position.y,
						                                          m_player.transform.position.z);
						m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.PLAYER;
						m_playerInput = true;
					}
				}
				if ( Input.GetKeyDown(KeyCode.Space ) && m_player.transform.position.y < 11 && 
				    m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y + 1] == TileTypes.EMPTY)
				{
					//spawn dagger
					GameObject newDagger = (GameObject)Instantiate (m_daggerPrefab, new Vector3((int)m_player.transform.position.x, (int)m_player.transform.position.y + 1, -1),transform.rotation);
					m_daggers.Add(newDagger);
					m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y + 1] = TileTypes.DAGGER;
					m_playerInput = true;
				}
			}
			else
			{
				//make ai decisions

				//move daggers up
				for (int i = 0; i < m_daggers.Count; ++i)
				{
					if (m_daggers[i] != null)
					{
						if (m_dungeon[(int)m_daggers[i].transform.position.x, (int)m_daggers[i].transform.position.y + 1] == TileTypes.EMPTY)
						{
							m_dungeon[(int)m_daggers[i].transform.position.x, (int)m_daggers[i].transform.position.y] = TileTypes.EMPTY;
							m_daggers[i].transform.position = new Vector3(m_daggers[i].transform.position.x,
						 	                                         	  m_daggers[i].transform.position.y + 1,
						  	                                  	          m_daggers[i].transform.position.z);
						}
						else
						{
							m_dungeon[(int)m_daggers[i].transform.position.x, (int)m_daggers[i].transform.position.y] = TileTypes.EMPTY;
							Destroy(m_daggers[i]);
						}

						if (m_daggers[i].transform.position.y <= 11)
						{
							m_dungeon[(int)m_daggers[i].transform.position.x,  (int)m_daggers[i].transform.position.y] = TileTypes.DAGGER;//issue code
						}
						else
						{
							Destroy(m_daggers[i]);
						}
					}
				}

				m_playerInput = false;
			}
		}
		else
		{
			if (currentPushAmmount < m_dungeonHeight - 1)
			{
				if (pushDownDelay < .0f)
				{
					//push everything down
					for (int i = 0; i < m_walls.Count; ++i)
					{
						if (m_walls[i] != null)
						{
							m_dungeon[(int)m_walls[i].transform.position.x, (int)m_walls[i].transform.position.y] = TileTypes.EMPTY;
							m_walls[i].transform.position = new Vector3(m_walls[i].transform.position.x,
							                                            m_walls[i].transform.position.y - 1,
						 	                                           m_walls[i].transform.position.z);
							if (m_walls[i].transform.position.y >= 0)
							{
								m_dungeon[(int)m_walls[i].transform.position.x,  (int)m_walls[i].transform.position.y] = TileTypes.WALL;
							}
							else
							{
								Destroy(m_walls[i]);
							}
						}
					}
					for (int i = 0; i < m_enemies.Count; ++i)
					{
						m_dungeon[(int)m_enemies[i].transform.position.x, (int)m_enemies[i].transform.position.y] = TileTypes.EMPTY;
						m_enemies[i].transform.position = new Vector3(m_enemies[i].transform.position.x,
						                                              m_enemies[i].transform.position.y - 1,
						                                              m_enemies[i].transform.position.z);
						if (m_enemies[i].transform.position.y >= 0)
						{
							m_dungeon[(int)m_walls[i].transform.position.x,  (int)m_walls[i].transform.position.y] = TileTypes.SPIDER;
						}
						else
						{
							Destroy (m_enemies[i]);
						}
					}
					m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.EMPTY;
					m_player.transform.position = new Vector3(m_player.transform.position.x,
					                                          m_player.transform.position.y - 1,
					                                          m_player.transform.position.z);
					m_dungeon[(int)m_player.transform.position.x, (int)m_player.transform.position.y] = TileTypes.PLAYER;
						
					//add new walls
					int currentWallCount = 0;
					float wallChance = 0.1f;
					for (int i = 0; i < 9; ++i)
					{
						if (i == 0 || i == 8)
						{
							m_dungeon[i, 11] = TileTypes.WALL;
							GameObject newWall = (GameObject)Instantiate (m_wallPrefab, new Vector3(i, 11, -1),transform.rotation);
							m_walls.Add(newWall);
						}
						else if (currentWallCount < m_maxWallInLine)
						{
							if (Random.value < wallChance)
							{
								m_dungeon[i, 11] = TileTypes.WALL;
								GameObject newWall = (GameObject)Instantiate (m_wallPrefab, new Vector3(i, 11, -1),transform.rotation);
								m_walls.Add(newWall);
	
								currentWallCount++;
							}
						}
					}
					//add new enemies

					currentPushAmmount++;
					pushDownDelay = 0.15f;
				}
			}
			else
			{
				m_loadNewArea = false;
				currentPushAmmount = 0;
			}
		}
	}
}
