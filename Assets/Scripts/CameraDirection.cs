using UnityEngine;
using System.Collections;

public class CameraDirection : MonoBehaviour
{
	public Player player;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		this.transform.position = new Vector3(5, player.Y, this.transform.position.z);
	}
}
