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
		string path = Constants.LOCAL_PATH + "/" + DateTime.Now.ToFileTime ().ToString () + ".png";
		File.WriteAllBytes (path, tex2D.EncodeToPNG ());  
		return path;
	}

}
