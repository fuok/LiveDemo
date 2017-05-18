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
		yield return new WaitForSeconds (3f);//并不能影响下面的
		print ("jin ru la !!!");//甚至会因为没到3秒就切换了而完全不会输出
		yield return async;//也就是说这里不写return async也完全没影响
	}

}
