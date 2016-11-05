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
		[Header ("人物显示")]
		public GameObject mLive2DHolder;
		public GameObject[] mLiveCharacters = new GameObject[3]{ null, null, null };

		void Start ()
		{
//			初始化控件
			mShowText.onClick.AddListener (ShowNextParagraph);
//			初始化文字DoTween
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
//				tweener.Kill (true);
				print ("text done");
			});
			tweener.Pause ();//这里必须先暂停，否则后面restart重设duration也没用,而是会继续使用这里的0f
		}

		private void ShowNextParagraph ()
		{
			//获取新的Paragraph
			Paragraph para = ParaManager.GetNextPara ();
			print (para.ToString ());

			//文字显示
//			mMainText.text = para.content;
			//设置dotween
//			GameObject mMainTextClone = GameObject.Instantiate (mMainTextPrefab);
//			mMainTextClone.transform.SetParent (mCanvasTrans, false);//加false后uGUI位置就对了
//			tweener.ChangeEndValue (para.content, 5f, true);//这里就不需要ChangeEndValue了
			tweener.ChangeValues ("", para.content, para.content.Length / 5);//直接使用ChangeValues
			tweener.Restart ();

			//人物显示
			string[] models = new string[3]{ para.model_0, para.model_1, para.model_2 };//3个位置上的模型名
			for (int i = 0; i < models.Length; i++) {
				print ("输出：" + i + "=" + models [i]);
				//获取人物并判断和已有人物是否相同
				if (mLiveCharacters [i] && !string.IsNullOrEmpty (models [i]) && mLiveCharacters [i].name.Contains (models [i])) {//已存在同名模型，什么也不做
					continue;
				} else {//表中读取的模型不存在，需要加载模型
					GameObject.Destroy (mLiveCharacters [i]);
					if (!string.IsNullOrEmpty (models [i])) {//如果是空位，销毁后不需要加载
						GameObject tempPrefab = Resources.Load<GameObject> ("prefabs/" + models [i]);
						switch (i) {
						case 0:
							tempPrefab.GetComponent<Benchmark> ().mPosX = 0f;//根据model设置位置
							break;
						case 1:
							tempPrefab.GetComponent<Benchmark> ().mPosX = -1f;
							break;
						case 2:
							tempPrefab.GetComponent<Benchmark> ().mPosX = 1f;
							break;
						}
						GameObject character = GameObject.Instantiate (tempPrefab);
						mLiveCharacters [i] = character;
					}
				}
			}


		}




	}
}

