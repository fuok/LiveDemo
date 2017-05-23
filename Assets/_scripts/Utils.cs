using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Utils
{
	public static string WriteRenderTexture2File (RenderTexture rt)
	{
		RenderTexture.active = rt;
		Texture2D tex2D = new Texture2D (rt.width, rt.height, TextureFormat.RGB24, false);
		tex2D.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
		tex2D.Apply ();
		RenderTexture.active = null;
		string path = Constants.SAVE_PATH + DateTime.Now.ToFileTime ().ToString () + ".png";
		File.WriteAllBytes (path, tex2D.EncodeToPNG ());  
		return path;
	}

	public static void DeleteFile (string path)
	{
		if (File.Exists (path)) {
			File.Delete (path);
		}
	}

	public static Color Hex2RGB (string hexStr)
	{
		//value = #ab364f
//		Debug.Log (hexStr);
		int r = Convert.ToInt32 ("0x" + hexStr.Substring (1, 2), 16);
		int g = Convert.ToInt32 ("0x" + hexStr.Substring (3, 2), 16);
		int b = Convert.ToInt32 ("0x" + hexStr.Substring (5, 2), 16);
		Color color = new Color (r / 255f, g / 255f, b / 255f);
//		Debug.Log (color.ToString ());
		return color;
	}
}
