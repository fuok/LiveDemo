using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using GameData;

namespace MyNamespace
{
	
	public class GameController : MonoBehaviour
	{

		private IEnumerator ie;

		void Start ()
		{
//			InitPara ();
		}

		void Update ()
		{
			
//			if (Input.GetMouseButtonDown (0)) {
//				if (ie.MoveNext ()) {
//					Paragraph para = ie.Current as Paragraph;
//					print (para.content);
//				} else {
//					print ("End!");
//				}
//			}
			
		}

		
		private void InitPara ()
		{
			TextAsset paraAsset = Resources.Load<TextAsset> ("paragraph/para_1");
			ParagraphData mData = JsonConvert.DeserializeObject<ParagraphData> (paraAsset.text);
			Resources.UnloadUnusedAssets ();
			
			if (true) {
				print (mData.paragraphList.Count);
				ie = mData.paragraphList.GetEnumerator ();
			}
		}
	}
}

