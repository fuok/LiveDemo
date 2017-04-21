using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
	
	public class GameSave
	{
		public int savId;
		public string savParaId;
		public string savText;
		public string savImgPath;

		public GameSave ()
		{
			
		}

		public GameSave (int savId, string savParaId, string savText, string savImgPath)
		{
			this.savId = savId;
			this.savParaId = savParaId;
			this.savText = savText;
			this.savImgPath = savImgPath;
		}

		public override string ToString ()
		{
			return "savId=" + savId + ",savParaId=" + savParaId + ",savText=" + savText + ",savImgPath=" + savImgPath;
		}
	}
}

