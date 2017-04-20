using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
	public GameObject mParaManager;

	// Use this for initialization
	void Start ()
	{
		GameObject.DontDestroyOnLoad (mParaManager);
		SceneManager.LoadScene ("[Intro]");
	}
	
	// Update is called once per frame
	//	void Update () {
	//
	//	}
}
