using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
	public Button btnStart, btnExit;
	private AsyncOperation async;
	private int progress = 0;

	// Use this for initialization
	void Start ()
	{
		btnStart.onClick.AddListener (delegate() {
			StartCoroutine (LoadScene ());
		});
		btnExit.onClick.AddListener (() => {
			Application.Quit ();
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (async != null) {
			progress = (int)(async.progress * 100);
			print (progress);
		}
	}

	private IEnumerator LoadScene ()
	{
		async = SceneManager.LoadSceneAsync ("Main", LoadSceneMode.Single);
		yield return async;
	}

}
