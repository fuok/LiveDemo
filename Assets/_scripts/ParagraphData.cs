using UnityEngine;
using System.Collections.Generic;

namespace GameData
{
	public class ParagraphData
	{
		public List<Paragraph> paragraphList = new List<Paragraph> ();
	}

	public class Paragraph
	{
		public string id;
		public string content;
	}
}

