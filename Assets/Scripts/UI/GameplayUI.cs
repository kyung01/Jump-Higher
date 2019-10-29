using UnityEngine;
using System.Collections;

public class GameplayUI : MonoBehaviour
{

	[SerializeField]
	UnityEngine.UI.Text text;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void init()
	{
		text.text = "0";
	}
	public void setScore(int score)
	{

		text.text = "" + score;

	}
}
