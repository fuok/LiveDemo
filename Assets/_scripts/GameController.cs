using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using GameData;
using UnityEngine.UI;
using DG.Tweening;

namespace MyNamespace
{
	
	public class GameController : MonoBehaviour
	{
		public Text mMainText;
		public GameObject mCube;

		void Start ()
		{
//			InitPara ();
		}

		void Update ()
		{
			
			if (Input.GetMouseButtonDown (0)) {
				Paragraph para = ParaManager.GetNextPara ();
				print (para.ToString ());
//				mMainText.text = para.content;

				//设置dotween文字
				mMainText.text = "";
				Tweener tweener = mMainText.DOText (para.content, para.content.Length / 5, true, ScrambleMode.None, null);
				tweener.SetDelay (0.1f);
				tweener.SetLoops (1);
				tweener.SetEase (Ease.Linear);
				tweener.SetAutoKill (false);
				tweener.OnComplete (() => {
//					tweener.Kill (true);
				});

//				DOTweenAnimation dota=mMainText.GetComponent<DOTweenAnimation>();
//				dota.
//				dota.DOPlay();

//				Tweener tt2=mCube.


			}
			if (Input.GetKeyDown (KeyCode.P)) {
				PlayerPrefs.DeleteKey ("dataBaseVersion");
			}
			
		}

		
		//		private void InitPara ()
		//		{
		//			TextAsset paraAsset = Resources.Load<TextAsset> ("paragraph/para_1");
		//			ParagraphData mData = JsonConvert.DeserializeObject<ParagraphData> (paraAsset.text);
		//			Resources.UnloadUnusedAssets ();
		//
		//			if (true) {
		//				print (mData.paragraphList.Count);
		//				ie = mData.paragraphList.GetEnumerator ();
		//			}
		//		}
	}
}

