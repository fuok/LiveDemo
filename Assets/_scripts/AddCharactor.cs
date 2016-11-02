using UnityEngine;
using System.Collections;

public class AddCharactor : MonoBehaviour
{
	public GameObject mCharacterPrefab;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Add ()
	{
		GameObject.Instantiate (mCharacterPrefab);
	}
}
