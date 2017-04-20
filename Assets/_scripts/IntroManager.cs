using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
	public Button btnStart, btnExit;
	//	private AsyncOperation async;
	//	private int progress = 0;

	void Start ()
	{
		InitDatabase ();
		InitUI ();
	}

	void Update ()
	{
		//test
		if (Input.GetKeyDown (KeyCode.P)) {
			DatabaseManager.Instance.CleanParaDB ();
		}

	}

	void InitDatabase ()
	{
		DatabaseManager.Instance.StartDatabase ();
	}

	void InitUI ()
	{
		btnStart.onClick.AddListener (delegate() {
			SceneManager.LoadScene ("[Play]");
		});
		btnExit.onClick.AddListener (() => {
			Application.Quit ();
		});
	}

	//	private IEnumerator LoadScene ()
	//	{
	//		async = SceneManager.LoadSceneAsync ("Main", LoadSceneMode.Single);
	//		yield return async;
	//	}

}
