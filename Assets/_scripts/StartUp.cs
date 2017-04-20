using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
	public GameObject mDatabaseManager;

	// Use this for initialization
	void Start ()
	{
		GameObject.DontDestroyOnLoad (mDatabaseManager);
		SceneManager.LoadScene ("[Load]");
	}
}
