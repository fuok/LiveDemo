using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using GameData;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace MyNamespace
{
	
	public class GameController : MonoBehaviour
	{
		[Header ("背景显示区域")]
		public GameObject mBgImage1;
		public GameObject mBgImage2;
		//用两张图片切换显示，轮流fade in/out实现转场效果
		private bool useFirstBg;
		[Header ("头像区域")]
		public RawImage mPortraitImage;
		public Texture mTransparentImage;
		[Header ("文字显示区域")]
		public Text mMainText;
		public Button mShowText;
		private Tweener tweenerText;
		//跳过文字显示过程
		private bool isTextShowing;
		[Header ("人物显示")]
		//		[SerializeField]
		//		private GameObject[] mLiveCharacters = new GameObject[3]{ null, null, null };
		public GameObject[] mModelsList;
		//		private string[] mModelsName = new string[]{ "haru", "wanko" };
		private Dictionary<string,GameObject> mModelsDic = new Dictionary<string, GameObject> ();
		[Header ("选项分支")]
		public GameObject mOptionPanel;
		public Button btnOption1, btnOption2;
		[Header ("Save/Load/Quit")]
		public Button btnSave;
		public Button btnLoad;
		public Button btnQuit;

		//保存当前para,针对第一次进来的时候,要预设一个初始next值
		private Paragraph currentPara = new Paragraph ("1");
		//保存前一个para
		private Paragraph lastPara = new Paragraph ();

		void Awake ()
		{
			//塞model数据，mModelsList和mModelsName长度和顺序必须一致
			for (int i = 0; i < mModelsList.Length; i++) {
				mModelsDic.Add (mModelsList [i].GetComponent<LAppModelProxy> ().name, mModelsList [i]);
			}
		}

		void Start ()
		{
			//
			InitUI ();
			//进来后开始游戏
			string continueParaId = PlayerPrefs.GetString (Constants.CONTINUE_PARA_ID, "1");
			Paragraph pNext = GetParagraphById (continueParaId);
			ShowParagraph (pNext);
		}

		void Update ()
		{
			//			if (Input.GetKeyDown (KeyCode.A)) {
			//				tweenerText.ChangeValues ("", "12345hfejhfjdhsf", 5f);
			//				tweenerText.Rewind ();//Rewind是动画回初始状态，这里会回到5f的状态，按照说明，ChangeValues相当于修改初始值+Rewind
			//			}

			//			if (Input.GetKeyDown (KeyCode.Y)) {
			//				AudioManagerS.Instance.PlayBGM ("bgm_1");
			//			}
			//			if (Input.GetKeyDown (KeyCode.U)) {
			//				AudioManagerS.Instance.PlayBGM ("bgm_2");
			//			}
		}

		/// <summary>
		/// 初始化.
		/// </summary>
		private void InitUI ()
		{
			//初始化控件
			mShowText.onClick.AddListener (delegate() {
				if (isTextShowing) {
					//文字快速显示
					tweenerText.Complete ();
				} else {
					Paragraph pNext = GetParagraphById (currentPara.next);
					ShowParagraph (pNext);
				}
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

		/// <summary>
		/// 初始化文字tweener
		/// </summary>
		private void InitTweener ()
		{
			tweenerText = mMainText.DOText ("", 0f, true, ScrambleMode.None, null);
			tweenerText.SetAutoKill (false);
			tweenerText.SetLoops (1);
			tweenerText.SetEase (Ease.Linear);
			tweenerText.OnComplete (() => {
//				tweener.Kill (true);
				print ("text done");
				isTextShowing = false;
			});
			tweenerText.Pause ();//这里必须先暂停，否则后面restart重设duration也没用,而是会继续使用这里的0f
		}

		/// <summary>
		/// Gets the paragraph by identifier.
		/// </summary>
		/// <returns>The paragraph by identifier.</returns>
		/// <param name="id">Identifier.</param>
		private Paragraph GetParagraphById (string id)
		{
			//获取新的Paragraph
			return ParaBean.Instance.GetPara (id);//如果id为空，会取到一个空para
		}

		/// <summary>
		/// Shows the next paragraph.
		/// </summary>
		private void ShowParagraph (Paragraph para)
		{
			if (!string.IsNullOrEmpty (para.id)) {//如果id为空就什么也不做，这里主要针对的是遇到分支的情况
				//把取到的para保存到当前,⭐️
				lastPara = currentPara;
				currentPara = para;
				print (para.ToString ());

				//背景显示,原先背景显示和头像显示使用的逻辑是不同的：背景是只有有值才发生改变，无值则保持现状。但读取进度会显示错误的背景
//				if (!string.IsNullOrEmpty (para.background)) {
				if (!para.background.Equals (lastPara.background)) {
					print (para.background + "-" + lastPara.background);
					if (useFirstBg) {//两站背景图轮流切换，这里doTween不需要初始化，直接用，不深究;如果希望面片上的贴图渐隐，shader需要用透明
						useFirstBg = false;
						mBgImage1.GetComponent<Renderer> ().material = Resources.Load<Material> ("background/material/" + para.background);
						//载入的图片默认alpha为255，所以设置为0
						Color cc = mBgImage1.GetComponent<Renderer> ().material.color;
						mBgImage1.GetComponent<Renderer> ().material.color = new Color (cc.r, cc.g, cc.b, 0f);
						mBgImage1.GetComponent<Renderer> ().material.DOFade (1f, 2f);
						//显示另一张
						mBgImage2.GetComponent<Renderer> ().material.DOFade (0f, 2f);
					} else {
						useFirstBg = true;
						mBgImage2.GetComponent<Renderer> ().material = Resources.Load<Material> ("background/material/" + para.background);
						//载入的图片默认alpha为255，所以设置为0
						Color cc = mBgImage2.GetComponent<Renderer> ().material.color;
						mBgImage2.GetComponent<Renderer> ().material.color = new Color (cc.r, cc.g, cc.b, 0f);
						mBgImage2.GetComponent<Renderer> ().material.DOFade (1f, 2f);
						//显示另一张
						mBgImage1.GetComponent<Renderer> ().material.DOFade (0f, 2f);
					}
				}
				//⭐️,头像逻辑是每个para中都要有值，如果是空值就会显示空头像。
				//头像显示
				if (mPortraitImage.texture && mPortraitImage.texture.name.Equals (para.portrait)) {
					print ("头像已存在");
				} else if (string.IsNullOrEmpty (para.portrait)) {
					//空头像，显示透明图
					mPortraitImage.texture = mTransparentImage;
				} else {
					mPortraitImage.texture = Resources.Load<Texture> ("portrait/" + para.portrait);
				}
				
				//文字显示
				//设置dotween
				//			mMainTextClone.transform.SetParent (mCanvasTrans, false);//加false后uGUI位置就对了
				//			tweener.ChangeEndValue (para.content, 5f, true);//这里就不需要ChangeEndValue了
				tweenerText.ChangeValues ("", para.content, para.content.Length / 20f);//直接使用ChangeValues//使用富文本后出字速度明显慢了//注意最后的参数是float时间才是正常的
				tweenerText.Restart ();
				isTextShowing = true;

				//人物live2d显示
//				string[] models = new string[3]{ para.model_0, para.model_1, para.model_2 };//3个位置上的模型名
//				for (int i = 0; i < models.Length; i++) {
//					//				print ("输出：" + i + "=" + models [i]);
//					//获取人物并判断和已有人物是否相同
//					if (mLiveCharacters [i] && !string.IsNullOrEmpty (models [i]) && mLiveCharacters [i].name.Contains (models [i])) {//已存在同名模型，什么也不做
//						continue;
//					} else {//表中读取的模型不存在，需要加载模型
//						GameObject.Destroy (mLiveCharacters [i]);
//						if (!string.IsNullOrEmpty (models [i])) {//如果是空位，销毁后不需要加载
//							GameObject tempPrefab = Resources.Load<GameObject> ("models/" + models [i]);
//							switch (i) {
//							case 0:
//								tempPrefab.GetComponent<Benchmark> ().mPos.x = 0f;//根据model设置位置
//								break;
//							case 1:
//								tempPrefab.GetComponent<Benchmark> ().mPos.x = -1f;
//								break;
//							case 2:
//								tempPrefab.GetComponent<Benchmark> ().mPos.x = 1f;
//								break;
//							}
//							GameObject character = GameObject.Instantiate (tempPrefab);
//							mLiveCharacters [i] = character;
//						}
//					}
//				}
				string[] models = new string[3]{ para.model_0, para.model_1, para.model_2 };//3个位置上的模型名,json获取
				//遍历所有角色，把需要的显示出来，不需要的隐藏
				IEnumerator ie = mModelsDic.Keys.GetEnumerator ();
				while (ie.MoveNext ()) {
//					print ("test:" + ie.Current);
					bool mGetModel = false;
					for (int i = 0; i < models.Length; i++) {
						if (!string.IsNullOrEmpty (models [i]) && models [i].Equals (ie.Current)) {
							print ("角色已存在");
							GameObject model = mModelsDic [ie.Current.ToString ()];
							model.GetComponent<LAppModelProxy> ().SetVisible (true);
							//根据配置位置修改live位置
							switch (i) {
							case 0:
								model.transform.position = new Vector3 (Constants.POSITION_CENTER, model.transform.position.y, 0f);
								//动作
								if (!string.IsNullOrEmpty (para.motion_0)) {
									model.GetComponent<LAppModelProxy> ().StartRandomMotion (para.motion_0, LAppDefine.PRIORITY_NORMAL);
								}
								break;
							case 1:
								model.transform.position = new Vector3 (Constants.POSITION_LEFT, model.transform.position.y, 0f);
								//动作
								if (!string.IsNullOrEmpty (para.motion_1)) {
									model.GetComponent<LAppModelProxy> ().StartRandomMotion (para.motion_1, LAppDefine.PRIORITY_NORMAL);
								}
								break;
							case 2:
								model.transform.position = new Vector3 (Constants.POSITION_RIGHT, model.transform.position.y, 0f);
								//动作
								if (!string.IsNullOrEmpty (para.motion_2)) {
									model.GetComponent<LAppModelProxy> ().StartRandomMotion (para.motion_2, LAppDefine.PRIORITY_NORMAL);
								}
								break;
							default:
								break;
							}
							mGetModel = true;
							break;
						}
					}
					if (!mGetModel) {
						mModelsDic [ie.Current.ToString ()].GetComponent<LAppModelProxy> ().SetVisible (false);
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
			Paragraph pNext = GetParagraphById (currentPara.next);
			ShowParagraph (pNext);
		}

		private void SaveGame ()
		{
			PlayerPrefs.SetString ("saveData_1", currentPara.id);
		}

		private void LoadGame ()
		{
			string id = PlayerPrefs.GetString ("saveData_1", "");//如果没有存档就取空
			ShowParagraph (GetParagraphById (id));
		}

		private void QuitGame ()
		{
			SceneManager.LoadScene ("[Intro]", LoadSceneMode.Single);
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

