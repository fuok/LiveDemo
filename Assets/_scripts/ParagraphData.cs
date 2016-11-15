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
		public string background;
		public string portrait;
		public string content;
		public string model_0;
		public string model_1;
		public string model_2;
		public string bgm;
		public string function;
		public string option_1;
		public string goto_1;
		public string option_2;
		public string goto_2;
		public string next;

		public Paragraph ()
		{
			
		}

		public Paragraph (string next)
		{
			this.next = next;
		}

		public Paragraph (string id, string background, string portrait, string content, string model_0, string model_1, string model_2, string bgm, string function, string option_1, string goto_1, string option_2, string goto_2, string next)
		{
			this.id = id;
			this.background = background;
			this.portrait = portrait;
			this.content = content;
			this.model_0 = model_0;
			this.model_1 = model_1;
			this.model_2 = model_2;
			this.bgm = bgm;
			this.function = function;
			this.option_1 = option_1;
			this.goto_1 = goto_1;
			this.option_2 = option_2;
			this.goto_2 = goto_2;
			this.next = next;
		}

		public override string ToString ()
		{
			return "id=" + id + ",background=" + background + ",portrait=" + portrait + ",content=" + content + ",model_0=" + model_0 + ",model_1=" + model_1 + ",model_2=" + model_2 + ",bgm=" + bgm + ",function=" + function + ",option_1=" + option_1 + ",goto_1=" + goto_1 + ",option_2=" + option_2 + ",goto_2=" + goto_2 + ",next=" + next;
		}
	}
}

