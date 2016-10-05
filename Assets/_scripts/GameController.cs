using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using GameData;
using UnityEngine.UI;

namespace MyNamespace
{
	
	public class GameController : MonoBehaviour
	{
		public Text mMainText;


		void Start ()
		{
//			InitPara ();
		}

		void Update ()
		{
			
			if (Input.GetMouseButtonDown (0)) {
				Paragraph para = ParaManager.GetNextPara ();
				print (para.ToString ());
				mMainText.text = para.content;
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

