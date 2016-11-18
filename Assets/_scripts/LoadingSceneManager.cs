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
		ParaManager.Instance.InitPara ();
		//TODO,简单处理
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
		yield return new WaitForSeconds (2f);
		async = SceneManager.LoadSceneAsync ("Main", LoadSceneMode.Single);
		yield return async;
	}

}
