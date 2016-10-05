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
		public string next;

		public Paragraph ()
		{
			
		}

		public Paragraph (string id, string content, string next)
		{
			this.id = id;
			this.content = content;
			this.next = next;
		}

		public override string ToString ()
		{
			return "id=" + id + ",content=" + content + ",next=" + next;
		}
	}
}

