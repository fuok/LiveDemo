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
		[Header ("背景显示区域")]
		public RawImage mBgImage;
		[Header ("头像区域")]
		public RawImage mPortraitImage;
		[Header ("文字显示区域")]
		public Text mMainText;
		public Button mShowText;
		private Tweener tweener;
		[Header ("人物显示")]
//		public GameObject mLive2DHolder;
		[SerializeField]
		private GameObject[] mLiveCharacters = new GameObject[3]{ null, null, null };
		[Header ("选项分支")]
		public GameObject mOptionPanel;
		public Button btnOption1, btnOption2;
		[Header ("Save/Load/Quit")]
		public Button btnSave;
		public Button btnLoad;
		public Button btnQuit;

		//临时保存当前para
		private Paragraph currentPara = new Paragraph ("1");
		//针对第一次进来的时候,要预设一个next

		void Start ()
		{
			Init ();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		private void Init ()
		{
			//初始化控件
			mShowText.onClick.AddListener (delegate() {
				ShowParagraph (currentPara.next);
			});
			btnOption1.onClick.AddListener (delegate() {
				ChooseOption (1);
			});
			btnOption2.onClick.AddListener (delegate() {
				ChooseOption (2);
			});
			btnSave.onClick.AddListener (SaveGame);
			btnLoad.onClick.AddListener (LoadGame);
			btnQuit.onClick.AddListener (QuitGame);
			//初始化文字DoTween
			InitTweener ();
		}

		void Update ()
		{
			if (Input.GetKeyDown (KeyCode.A)) {
//				mMainText.text = "";
				tweener.ChangeValues ("", "12345hfejhfjdhsf", 5f);
//				tweener.Restart ();
				tweener.Rewind ();//Rewind是动画回初始状态，这里会回到5f的状态，按照说明，ChangeValues相当于修改初始值+Rewind
			}

//			if (Input.GetKeyDown (KeyCode.Y)) {
//				AudioManagerS.Instance.PlayBGM ("bgm_1");
//			}
//			if (Input.GetKeyDown (KeyCode.U)) {
//				AudioManagerS.Instance.PlayBGM ("bgm_2");
//			}
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

		/// <summary>
		/// Shows the next paragraph.
		/// </summary>
		private void ShowParagraph (string id)
		{
			//获取新的Paragraph
			Paragraph para = ParaManager.Instance.GetNextPara (id);//如果id为空，会取到一个空para
			print (para.ToString ());
			if (!string.IsNullOrEmpty (id)) {//如果id为空就什么也不做，这里主要针对的是遇到分支的情况
				//⭐️目前背景显示和头像显示使用的逻辑是相同的，就是每个para中都要有值，如果是空值就会显示空背景和空头像。
				//背景显示
				if (mBgImage.texture && mBgImage.texture.name.Equals (para.background)) {
					print ("背景已存在");
				} else {
					mBgImage.texture = Resources.Load<Texture> ("background/" + para.background);
				}

				//头像显示
				if (mPortraitImage.texture && mPortraitImage.texture.name.Equals (para.portrait)) {
					print ("头像已存在");
				} else {
					mPortraitImage.texture = Resources.Load<Texture> ("portrait/" + para.portrait);
				}
				
				//文字显示
				//			mMainText.text = para.content;
				//设置dotween
				//			mMainTextClone.transform.SetParent (mCanvasTrans, false);//加false后uGUI位置就对了
				//			tweener.ChangeEndValue (para.content, 5f, true);//这里就不需要ChangeEndValue了
				tweener.ChangeValues ("", para.content, para.content.Length / 20f);//直接使用ChangeValues//使用富文本后出字速度明显慢了//注意最后的参数是float时间才是正常的
				tweener.Restart ();
				
				//人物live2d显示
				string[] models = new string[3]{ para.model_0, para.model_1, para.model_2 };//3个位置上的模型名
				for (int i = 0; i < models.Length; i++) {
					//				print ("输出：" + i + "=" + models [i]);
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

				//特殊脚本方法
				if (!string.IsNullOrEmpty (para.function)) {
					Invoke (para.function, 0.5f);
				}

				//背景音乐播放
				AudioManagerS.Instance.PlayBGM (para.bgm);

				//选项显示
				if (string.IsNullOrEmpty (para.next) && !string.IsNullOrEmpty (para.option_1)) {
					print ("option_1:" + para.option_1);
					print ("option_2:" + para.option_2);
					mOptionPanel.SetActive (true);
					btnOption1.GetComponentInChildren<Text> ().text = para.option_1;
					btnOption2.GetComponentInChildren<Text> ().text = para.option_2;
				}

				//把取到的para保存到当前,⭐️
				currentPara = para;
			}

		}

		/// <summary>
		/// Chooses the option.
		/// </summary>
		/// <param name="index">Index.</param>
		private void ChooseOption (int index)
		{
			switch (index) {
			case 1:
				currentPara.next = currentPara.goto_1;
				break;
			case 2:
				currentPara.next = currentPara.goto_2;
				break;
			default:
				break;
			}
			mOptionPanel.SetActive (false);
			ShowParagraph (currentPara.next);
		}


		private void SaveGame ()
		{
			PlayerPrefs.SetString ("saveData_1", currentPara.id);
		}

		private void LoadGame ()
		{
			string id = PlayerPrefs.GetString ("saveData_1", "");//如果没有存档就取空
			ShowParagraph (id);
		}

		private void QuitGame ()
		{
			Application.Quit ();
		}

		//----------------------------------------------------------------------------

		/// <summary>
		/// Shakes the camera.
		/// </summary>
		private void ShakeCamera ()
		{
			Camera.main.DOShakePosition (2f, 0.2f);
		}

	}
}

