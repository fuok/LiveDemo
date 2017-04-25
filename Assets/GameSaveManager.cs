using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameData;

public class GameSaveManager : MonoBehaviour
{
	public GameObject panelSaveGame;
	public GameObject[] itemSaveGame;
	public RenderTexture rtScreenCapture;

	void Start ()
	{
		itemSaveGame [0].transform.Find ("Button Save").GetComponent<Button> ().onClick.AddListener (new UnityEngine.Events.UnityAction (() => {
			SaveGame (1);
			ShowLoadedGame ();
		}));
		itemSaveGame [0].transform.Find ("Button Load").GetComponent<Button> ().onClick.AddListener (new UnityEngine.Events.UnityAction (() => {
			LoadGame (1);
		}));
		
		itemSaveGame [1].transform.Find ("Button Save").GetComponent<Button> ().onClick.AddListener (new UnityEngine.Events.UnityAction (() => {
			SaveGame (2);
			ShowLoadedGame ();
		}));
		itemSaveGame [1].transform.Find ("Button Load").GetComponent<Button> ().onClick.AddListener (new UnityEngine.Events.UnityAction (() => {
			LoadGame (2);
		}));
		itemSaveGame [2].transform.Find ("Button Save").GetComponent<Button> ().onClick.AddListener (new UnityEngine.Events.UnityAction (() => {
			SaveGame (3);
			ShowLoadedGame ();
		}));
		itemSaveGame [2].transform.Find ("Button Load").GetComponent<Button> ().onClick.AddListener (new UnityEngine.Events.UnityAction (() => {
			LoadGame (3);
		}));
	}

	void OnEnable ()
	{
		ShowLoadedGame ();
	}

	//储存游戏
	private void SaveGame (int saveId)
	{
		print ("SaveGame (),saveId=" + saveId);
		Paragraph currentPara = GameController.Instance.currentPara;
		//先保存屏幕
		string path = Utils.WriteRenderTexture2File (rtScreenCapture);
		//声明Save对象
		GameSave save = new GameSave (saveId, currentPara.id, currentPara.content, path);
		GameSaveBean.Instance.AddGameSave2DB (save);
	}

	//获取存档界面
	private void ShowLoadedGame ()
	{
		for (int i = 0; i < itemSaveGame.Length; i++) {
			GameSave save = GameSaveBean.Instance.GetGameSaveFromDB (i + 1);
			print (save.ToString ());
			if (save.savId != 0) {
				itemSaveGame [i].transform.Find ("Text ID").GetComponent<Text> ().text = save.savId.ToString ();
				itemSaveGame [i].transform.Find ("Text Content").GetComponent<Text> ().text = save.savText;
				StartCoroutine (LoadLocalImage (itemSaveGame [i].transform.Find ("Raw Thumbnail").GetComponent<RawImage> (), save.savImgPath));
			}
		}
	}

	//读取游戏
	private void LoadGame (int saveId)
	{
		print ("LoadGame (),saveId=" + saveId);
		GameSave save = GameSaveBean.Instance.GetGameSaveFromDB (saveId);
		GameController.Instance.ShowParagraph (GameController.Instance.GetParagraphById (save.savParaId));
//		print (save.ToString ());
	}

	IEnumerator LoadLocalImage (RawImage img, string filePath)
	{
		WWW www = new WWW ("file://" + filePath);
		yield return www;
		img.texture = www.texture;
	}
}
