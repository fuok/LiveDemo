﻿using UnityEngine;
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
		public string model_0;
		public string model_1;
		public string model_2;
		public string next;

		public Paragraph ()
		{
			
		}

		public Paragraph (string next)
		{
			this.next = next;
		}

		public Paragraph (string id, string content, string model_0, string model_1, string model_2, string next)
		{
			this.id = id;
			this.content = content;
			this.model_0 = model_0;
			this.model_1 = model_1;
			this.model_2 = model_2;
			this.next = next;
		}

		public override string ToString ()
		{
			return "id=" + id + ",content=" + content + ",model_0=" + model_0 + ",model_1=" + model_1 + ",model_2=" + model_2 + ",next=" + next;
		}
	}
}

