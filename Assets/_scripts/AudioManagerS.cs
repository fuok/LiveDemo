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

	[Header ("BGM")]//用于背景
	//音频源数组,AudioSource即使在这里声明也是不能直接播放的，作用是为了存入Dictionary方便查找
	public AudioSource[] mAuSourceArray;
	//保存音频库索引
	private Dictionary<string,AudioSource> mDicSource;

	[Header ("voice")]//用于语音
	public AudioSource mAuSourceVoice;

	[Header ("SFX")]//用于音效
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
	//但这个方法仍存在问题，比如人物语音，如何判断该停止播放播放哪段音源，除非把人物语音每次都挂载到特定AudioResource上。
	//那么这里能处理的，就只有背景音乐和特效了

	/// <summary>
	///传值 “音乐剪辑名称” 播放 “游戏音效”
	/// 使用这个方法,注意这个方法不支持一个音效重复播放，适合用来播放背景音乐，不适合播放特效
	/// </summary>
	/// <param name="audioSourceName">游戏音效源</param>
	public void PlayBGM (string audioSourceName)
	{
		if (!string.IsNullOrEmpty (audioSourceName)) {
			if (mDicSource [audioSourceName]) {
				if (mDicSource [audioSourceName].isPlaying) {
					//如果当前bgm在播放，就什么也不做.
					return;
				}
				foreach (var item in transform.FindChild("bgm").GetComponentsInChildren<AudioSource>()) {
					//关闭其他bgm
					item.Stop ();
				}
				//AudioSource必须是组件形式才能播放，只是拿到对象（例如从字典里取出）是不行的。prefab的source也是不能播的,注意引用关系
				mDicSource [audioSourceName].Play ();
			}
		}
	}

	/// <summary>
	/// 播放语音
	/// </summary>
	/// <param name="audioClipName">Audio clip name.</param>
	public void PlayVOC (string audioClipName)
	{
		if (!string.IsNullOrEmpty (audioClipName)) {
			//TODO,待整理
		}
	}

	/// <summary>
	/// 可以同时播放相同Clip，适合播放音效,使用不断添加删除组件的方式
	/// </summary>
	/// <param name="audioName">Audio name.</param>
	public void PlaySFX (string audioClipName)
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
