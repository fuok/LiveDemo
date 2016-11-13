/***********************
 * Title: "AudioManagerS"
 * Function:
 * 	- 
 * UsedBy:	
 * Author:	LY
 * Date:	2016.11.14
 * Record:	优化
 ***********************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManagerS : MonoBehaviour
{
	public static AudioManagerS Instance{ get; private set; }

	[Header ("AudioSource")]//用于背景和语音
	//音频源数组
	public AudioSource[] mAuSourceArray;
	//保存音频库索引
	private Dictionary<string,AudioSource> mDicSource;

	[Header ("AudioClip")]//用于音效
	//音频剪辑数组
	public AudioClip[] mAuClipArray;
	//音频剪辑索引
	private Dictionary<string,AudioClip> mDicClip;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		//保存声音数据到Dictionary
		if (mAuSourceArray.Length > 0) {
			mDicSource = new Dictionary<string, AudioSource> ();
			foreach (AudioSource audioSource in mAuSourceArray) {
				mDicSource.Add (audioSource.name, audioSource);
			}
		}
		if (mAuClipArray.Length > 0) {
			mDicClip = new Dictionary<string, AudioClip> ();
			foreach (AudioClip audioClip in mAuClipArray) {
				mDicClip.Add (audioClip.name, audioClip);
			}
		}
	}
	//Awake_end

	/// <summary>
	///传值 “音乐剪辑” 播放 “背景音乐”
	/// </summary>
	/// <param name="audiClip">游戏音效剪辑</param>
	//	public static void PlayEffAudio (AudioClip audiClip)
	//	{   
	//		if (audiClip) {//播放
	//			AudioSource[] AS = mAudioManagerInstance.GetComponents<AudioSource> ();//先获取当前对象下的AudioSource
	//			for (int i = 1; i < AS.Length; i++) {//跳过下标0,0位预留给背景音乐
	//				if (!AS [i].isPlaying) {
	//					AS [i].clip = audiClip;//音频剪辑的赋值
	//					AS [i].Play ();//播放背景音乐
	//					return;
	//				} else {
	//					//元素被占用
	//				}
	//			}
	//			//当需要同时播放多个音效或当前对象的AudioSource不足,添加新AudioSource
	//			AudioSource NewAS = mAudioManagerInstance.AddComponent<AudioSource> ();
	//			NewAS.loop = false;
	//			NewAS.clip = audiClip;
	//			NewAS.Play ();
	//		}
	//	}

	//音乐播放分为几种不同情况:
	//1,同一首音乐，则继续播放。2,同一首音乐，从头播放。
	//3,新的音乐，停止原先的播放新的。4,新的音乐，但同时播放。5,音效，无论相同的还是不同的，都同时播放

	/// <summary>
	///传值 “音乐剪辑名称” 播放 “游戏音效”
	/// 使用这个方法,注意这个方法不支持一个音效重复播放，适合用来播放背景音乐和语音，不适合播放特效
	/// </summary>
	/// <param name="audioSourceName">游戏音效源</param>
	/// <param name="stopPrevious">是否停止当前的声音播放新的</param>
	public void PlayAudioSingle (string audioSourceName, bool stopPrevious)//TODO
	{
		if (!string.IsNullOrEmpty (audioSourceName)) {
			if (mDicSource [audioSourceName]) {
				mDicSource [audioSourceName].Play ();
			}
		}
	}

	/// <summary>
	/// Plaies the eff audio multiple.
	/// </summary>
	/// <param name="audioName">Audio name.</param>
	public void PlayAudioMultiple (string audioClipName)
	{
		if (!string.IsNullOrEmpty (audioClipName)) {
			if (mDicClip [audioClipName]) {
				AudioSource aSource = gameObject.AddComponent<AudioSource> ();
				aSource.loop = false;
				aSource.clip = mDicClip [audioClipName];
				aSource.Play ();
				Destroy (aSource, aSource.clip.length);//播放完销毁
			}
		}
	}

}
