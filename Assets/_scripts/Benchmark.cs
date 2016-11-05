using UnityEngine;
using System;
using System.Collections.Generic;
using live2d;

public class Benchmark : MonoBehaviour
{
	//创建模型数量，暂时没什么用，只使用一个
	public int NUM_MODELS = 10;
	[Header ("模型图片")]
	public Texture2D[] texture;
	[Header ("模型文件")]
	public TextAsset mocFile;
	[Header ("动作文件")]
	public TextAsset mtnFile;

	private List<Live2DModelUnity> models;
	private MotionQueueManager motionMgr = new MotionQueueManager ();
	private Live2DMotion motion;

	public float mPosX = 0f;

	void Start ()
	{
		Live2D.init ();

		models = new List<Live2DModelUnity> ();
		for (int i = 0; i < NUM_MODELS; i++) {
			models.Add (CreateModel ());
		}

		if (mtnFile != null) {
			motion = Live2DMotion.loadMotion (mtnFile.bytes);
		}
	}
		
	//创建模型，每个模型只会用一次
	private Live2DModelUnity CreateModel ()
	{
		var live2DModel = Live2DModelUnity.loadModel (mocFile.bytes);
		for (int i = 0; i < texture.Length; i++) {
			live2DModel.setTexture (i, texture [i]);
		}
		float modelWidth = live2DModel.getCanvasWidth ();

		Matrix4x4 live2DCanvasPos;
		live2DCanvasPos = Matrix4x4.Ortho (0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

		Matrix4x4 objectPos = transform.localToWorldMatrix;
		objectPos.SetTRS (new Vector3 (mPosX, 0, 0), Quaternion.identity, new Vector3 (1, 1, 1));
		live2DModel.setMatrix (objectPos * live2DCanvasPos);
		return live2DModel;
	}

	void Update ()
	{
		if (motionMgr.isFinished ()) {
			motionMgr.startMotion (motion);
		}
		
		for (int i = 0; i < NUM_MODELS; i++) {
			var live2DModel = models [i];
			if (live2DModel == null)
				continue;
			
			motionMgr.updateParam (live2DModel);
			
			live2DModel.update ();
		}
	}

	//会调用两次，原因不懂
	void OnRenderObject ()
	{
		for (int i = 0; i < NUM_MODELS; i++) {
			var live2DModel = models [i];
			if (live2DModel == null)
				continue;
			live2DModel.draw ();
		}
	}

	public void ChangePosition ()
	{
	}
}