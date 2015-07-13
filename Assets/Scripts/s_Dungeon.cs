using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_Dungeon : MonoBehaviour {

	public enum TileTypes{EMPTY, WALL, PLAYER, SPIDER, DAGGER};
	
	public TileTypes[,] m_dungeon;

	public bool pushDownDungeon = false;
	public int currentPushAmmount = 0;
	public float pushDownDelay = 0.2f;

	public GameObject m_wallPrefab;
	public GameObject m_playerPrefab;
	public GameObject m_enemyPrefab;

	private List<GameObject> m_enemies;
	private List<GameObject> m_walls;
	private GameObject m_player;

	private float m_playerActionTimer = 0.5f;

	private int m_maxWallInLine = 1;
	private int m_maxEnemyInLine = 1;

	private const int m_dungeonHeight = 12;
	private const int m_dungeonWidth = 9;

	private bool m_playerInput = false;
	
	// Use this for initialization
	void Start () {
		m_dungeon = new TileTypes[m_dungeonWidth,m_dungeonHeight];

		m_enemies = new List<GameObject>();
		m_walls = new List<GameObject>();

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
		m_playerActionTimer -= Time.deltaTime;

		if (!m_playerInput)
		{
			if (m_player.transform.position.y == m_dungeonHeight - 1)
			{
				m_playerInput = !m_playerInput;
				pushDownDungeon = true;
			}
			//wait for the player to make an action

			if ( Input.GetKeyDown(KeyCode.UpArrow) && m_player.transform.position.y < 11)
			{
				//check if the position above the player is clear using the array
				m_player.transform.position = new Vector3(m_player.transform.position.x,
				                                          m_player.transform.position.y + 1,
				                                          m_player.transform.position.z);
			}
			if ( Input.GetKeyDown(KeyCode.DownArrow) && m_player.transform.position.y > 0)
			{
				m_player.transform.position = new Vector3(m_player.transform.position.x,
				                                          m_player.transform.position.y - 1,
				                                          m_player.transform.position.z);
			}
			if ( Input.GetKeyDown(KeyCode.LeftArrow) && m_player.transform.position.x > 1)
			{
				m_player.transform.position = new Vector3(m_player.transform.position.x - 1,
				                                          m_player.transform.position.y,
				                                          m_player.transform.position.z);
			}
			if ( Input.GetKeyDown(KeyCode.RightArrow) && m_player.transform.position.x < 7)
			{
				m_player.transform.position = new Vector3(m_player.transform.position.x + 1,
				                                          m_player.transform.position.y,
				                                          m_player.transform.position.z);
			}
		}
		else
		{
			if (currentPushAmmount < m_dungeonHeight)
			{
				if (pushDownDelay < .0f)
				{
					//push everything down
					for (int i = 0; i < m_walls.Count; ++i)
					{
						m_walls[i].transform.position = new Vector3(m_walls[i].transform.position.x,
						                                            m_walls[i].transform.position.y - 1,
						                                            m_walls[i].transform.position.z);
					}
					for (int i = 0; i < m_enemies.Count; ++i)
					{
						m_enemies[i].transform.position = new Vector3(m_enemies[i].transform.position.x,
						                                              m_enemies[i].transform.position.y - 1,
						                                              m_enemies[i].transform.position.z);
					}
					m_player.transform.position = new Vector3(m_player.transform.position.x,
					                                          m_player.transform.position.y - 1,
					                                          m_player.transform.position.z);
					//delete entities with position.y < 0
						
					//make decisions for all other entities

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
					pushDownDelay = 0.2f;
				}
			}
			else
			{
				m_playerInput = false;
				currentPushAmmount = 0;
			}
		}
	}
}
