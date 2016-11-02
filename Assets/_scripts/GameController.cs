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
		[Header ("文字显示区域")]
		public Text mMainText;
		public Button mShowText;
		private Tweener tweener;
		public GameObject mCube;

		void Start ()
		{
			mShowText.onClick.AddListener (ShowNextText);
			InitTweener ();
		}

		void Update ()
		{

			if (Input.GetKeyDown (KeyCode.P)) {
				PlayerPrefs.DeleteKey ("dataBaseVersion");
			}
			
			if (Input.GetKeyDown (KeyCode.A)) {
//				mMainText.text = "";
				tweener.ChangeValues ("", "12345hfejhfjdhsf", 5f);
//				tweener.Restart ();
				tweener.Rewind ();//Rewind是动画回初始状态，这里会回到5f的状态，按照说明，ChangeValues相当于修改初始值+Rewind
			}
		}

		/// <summary>
		/// 初始化文字tweener
		/// </summary>
		private void InitTweener ()
		{
			tweener = mMainText.DOText ("", 0f, true, ScrambleMode.None, null);
			tweener.SetAutoKill (false);
			tweener.SetLoops (1);
			tweener.SetEase (Ease.Linear);
			tweener.OnComplete (() => {
				//					tweener.Kill (true);
				print ("text done");
			});
			tweener.Pause ();//这里必须先暂停，否则后面restart重设duration也没用,而是会继续使用这里的0f
		}

		private void ShowNextText ()
		{
			Paragraph para = ParaManager.GetNextPara ();
			print (para.ToString ());
//			mMainText.text = para.content;

			//设置dotween文字

//			GameObject mMainTextClone = GameObject.Instantiate (mMainTextPrefab);
//			mMainTextClone.transform.SetParent (mCanvasTrans, false);//加false后uGUI位置就对了
//
//			tweener.ChangeEndValue (para.content, 5f, true);//这里就不需要ChangeEndValue了
			tweener.ChangeValues ("", para.content, para.content.Length / 5);//直接使用ChangeValues
			tweener.Restart ();
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

