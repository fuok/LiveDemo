using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
	public Button btnStart;

	// Use this for initialization
	void Start ()
	{
		btnStart.onClick.AddListener (delegate() {
			SceneManager.LoadScene ("Main", LoadSceneMode.Single);
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

}
