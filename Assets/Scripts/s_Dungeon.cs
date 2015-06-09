using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_Dungeon : MonoBehaviour {

	public enum TileTypes{EMPTY, WALL, PLAYER, SPIDER, DAGGER};
	
	public TileTypes[,] m_dungeon;

	public float tempTurnSwitch = 1.0f;

	public GameObject m_wall;
	public GameObject m_player;
	public GameObject m_enemy;

	private List<GameObject> m_enemies;
	private List<GameObject> m_walls;

	private int m_maxWallInLine = 1;
	private int m_maxEnemyInLine = 1;

	private bool m_playerInput = false;
	
	// Use this for initialization
	void Start () {
		m_dungeon = new TileTypes[9,12];

		m_enemies = new List<GameObject>();
		m_walls = new List<GameObject>();

		//setup random dungeon
		for (int i = 0; i < 9; ++i)
		{
			for (int j = 0; j < 12; ++j)
			{
				if (i == 0 || i == 8)
				{
					m_dungeon[i, j] = TileTypes.WALL;
					GameObject newWall = (GameObject)Instantiate (m_wall, new Vector3(i, j, -1),transform.rotation);
					m_walls.Add(newWall);
				}
				else
				{
					m_dungeon[i, j] = TileTypes.EMPTY;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		tempTurnSwitch -= Time.deltaTime;

		if (!m_playerInput)
		{
			if (tempTurnSwitch < 0)
			{
				m_playerInput = true;
				tempTurnSwitch = 1.0f;
			}
			//wait for the player to make an action
		}
		else
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
					GameObject newWall = (GameObject)Instantiate (m_wall, new Vector3(i, 11, -1),transform.rotation);
					m_walls.Add(newWall);
				}
				else if (currentWallCount < m_maxWallInLine)
				{
					if (Random.value < wallChance)
					{
						m_dungeon[i, 11] = TileTypes.WALL;
						GameObject newWall = (GameObject)Instantiate (m_wall, new Vector3(i, 11, -1),transform.rotation);
						m_walls.Add(newWall);

						currentWallCount++;
					}
				}
			}
			//add new enemies

			m_playerInput = false;
		}
	}
}
