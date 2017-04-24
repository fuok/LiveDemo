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
		for (int i = 0; i < itemSaveGame.Length; i++) {
			itemSaveGame [i].transform.Find ("Button Save").GetComponent<Button> ().onClick.AddListener (new UnityEngine.Events.UnityAction (() => {
				SaveGame (i, GameController.Instance.currentPara);
				SetLoadedGame ();
			}));
			itemSaveGame [i].transform.Find ("Button Load").GetComponent<Button> ().onClick.AddListener (new UnityEngine.Events.UnityAction (() => {
				LoadGame (i);
			}));
		}
	}

	void OnEnable ()
	{
//		print ("kaishi");
		SetLoadedGame ();
	}

	//储存游戏
	private void SaveGame (int saveId, Paragraph currentPara)
	{
		//先保存屏幕
		string path = Utils.WriteRenderTexture2File (rtScreenCapture);
		//声明Save对象
		GameSave save = new GameSave (saveId, currentPara.id, currentPara.content, path);
		GameSaveBean.Instance.AddGameSave2DB (save);
		//UI刷新
//		txtSaveId.text = save.savId.ToString ();
//		txtSaveContent.text = save.savText;
//		StartCoroutine (LoadLocalImage (imgSaveThumbnail, save.savImgPath));
		//			print ("缩略图地址：" + save.savImgPath);
	}

	//获取存档界面
	private void SetLoadedGame ()
	{
		for (int i = 0; i < itemSaveGame.Length; i++) {
			GameSave save = GameSaveBean.Instance.GetGameSaveFromDB (i);
			if (save != null) {
				itemSaveGame [i].transform.Find ("Text ID").GetComponent<Text> ().text = save.savId.ToString ();
				itemSaveGame [i].transform.Find ("Text Content").GetComponent<Text> ().text = save.savText;
				StartCoroutine (LoadLocalImage (itemSaveGame [i].transform.Find ("Raw Thumbnail").GetComponent<RawImage> (), save.savImgPath));
			}
		}
	}

	//读取游戏
	private void LoadGame (int saveId)
	{
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
