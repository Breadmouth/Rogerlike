using UnityEngine;
using System.Collections;

public class s_Dungeon : MonoBehaviour {

	public enum TileTypes{EMPTY, WALL, PLAYER, SPIDER, DAGGER};
	
	public TileTypes[,] m_dungeon;

	public GameObject m_wall;
	public GameObject m_player;
	public GameObject m_enemy;
	
	// Use this for initialization
	void Start () {
		m_dungeon = new TileTypes[100,100];

		//setup random dungeon
		for (int i = 0; i < 100; ++i)
		{
			for (int j = 0; j < 100; ++j)
			{
				m_dungeon[i, j] = TileTypes.EMPTY;
			}
		}
		m_dungeon[1, 0] = TileTypes.WALL;
		m_dungeon[0, 0] = TileTypes.PLAYER;
		m_dungeon[0, 1] = TileTypes.SPIDER;

		//spawn walls where wall belong
		for (int i = 0; i < 100; ++i)
		{
			for (int j = 0; j < 100; ++j)
			{
				if (m_dungeon[i, j] == TileTypes.WALL)
				{
					GameObject newWall = (GameObject)Instantiate (m_wall, new Vector3(i, j, -1),transform.rotation);
				}
				if (m_dungeon[i, j] == TileTypes.PLAYER)
				{
					GameObject newPlayer = (GameObject)Instantiate (m_player, new Vector3(i, j, -1),transform.rotation);
				}
				if (m_dungeon[i, j] == TileTypes.SPIDER)
				{
					GameObject newEnemy = (GameObject)Instantiate (m_enemy, new Vector3(i, j, -1),transform.rotation);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
