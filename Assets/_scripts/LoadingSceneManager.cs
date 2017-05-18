using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
	private AsyncOperation async;
	private int progress = 0;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (StartMain ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (async != null) {
			progress = (int)(async.progress * 100);
			print (progress);
		}
	}

	private IEnumerator StartMain ()
	{
		yield return new WaitForEndOfFrame ();
		async = SceneManager.LoadSceneAsync ("[Intro]", LoadSceneMode.Single);
		print ("jin ru la !!!");
		yield return async;
	}

}
