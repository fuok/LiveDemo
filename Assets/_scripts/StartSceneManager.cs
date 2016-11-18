using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
	public Button btnStart, btnExit;
	//	private AsyncOperation async;
	//	private int progress = 0;

	// Use this for initialization
	void Start ()
	{
		btnStart.onClick.AddListener (delegate() {
			SceneManager.LoadScene ("Load");
		});
		btnExit.onClick.AddListener (() => {
			Application.Quit ();
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
		//test
		if (Input.GetKeyDown (KeyCode.P)) {
			ParaManager.Instance.CleanParaDB ();
		}

	}

	//	private IEnumerator LoadScene ()
	//	{
	//		async = SceneManager.LoadSceneAsync ("Main", LoadSceneMode.Single);
	//		yield return async;
	//	}

}
