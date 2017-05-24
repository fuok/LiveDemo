using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
	public GameObject panelMain;
	public Button btnStart, btnContinue, btnSetting, btnExit;
	public GameObject panelSetting;
	public Button btnBack;
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
			DatabaseManager.Instance.CleanDB ();
		}

	}

	void InitDatabase ()
	{
		DatabaseManager.Instance.StartDatabase ();
	}

	void InitUI ()
	{
		btnStart.onClick.AddListener (delegate() {
			Constants.fromBeginning = true;
			SceneManager.LoadScene ("[Play]");
		});
		btnContinue.onClick.AddListener (() => {
			Constants.fromBeginning = false;
			SceneManager.LoadScene ("[Play]");
		});
		btnSetting.onClick.AddListener (() => {
			panelMain.SetActive (false);
			panelSetting.SetActive (true);
		});
		btnExit.onClick.AddListener (() => {
			Application.Quit ();
		});
		btnBack.onClick.AddListener (() => {
			panelSetting.SetActive (false);
			panelMain.SetActive (true);
		});
	}

	public void ShowMainPanel ()//dotween动画触发
	{
		panelMain.SetActive (true);
	}

}
