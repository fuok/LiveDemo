/***********************
 * Title: "哈哈"
 * Function:
 * 	- 
 * UsedBy:	
 * Author:	LY
 * Date:	2016.3
 * Record:	
 ***********************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManagerS : MonoBehaviour
{
	private static bool mUseAudioClip;
	//音频剪辑数组
	public AudioClip[] mAuClipArray;
	//音频源数组,这两种使用时任选
	public AudioSource[] mAuSourceArray;
	//保存音频库索引,同样为AudioClip和AudioSource任选
	private static Dictionary<string, AudioClip> mDicClip;
	private static Dictionary<string,AudioSource> mDicSource;
	//背景音乐音频源
	private static AudioSource mAuSourceBGMusic;
	//用于接收音效组件
	private static GameObject mAudioManagerInstance;

	void Awake ()
	{
		//当传入AudioClip时
		if (mAuClipArray.Length > 0) {
			mDicClip = new Dictionary<string, AudioClip> ();//加载音频库
			foreach (AudioClip audioClip in mAuClipArray) {
				mDicClip.Add (audioClip.name, audioClip);
			}
			mUseAudioClip = true;
		}
		//当传入是AudioSource时
		else if (mAuSourceArray.Length > 0) {
			mDicSource = new Dictionary<string, AudioSource> ();
			foreach (AudioSource audioSource in mAuSourceArray) {
				mDicSource.Add (audioSource.clip.name, audioSource);
			}
			mUseAudioClip = false;
		}
		//为背景音乐单独添加一个音频源
		mAuSourceBGMusic = this.gameObject.AddComponent<AudioSource> ();//背景音乐,位于AudioSource组件数组下标0,所以第一个添加

		//接收音效音频源
		mAudioManagerInstance = this.gameObject;
	}
	//Awake_end
	


	/// <summary>
	///传值 “音乐剪辑” 播放 “背景音乐” 
	/// </summary>
	/// <param name="audiClip">背景音乐剪辑</param>
	public static void PlayBGMusic (AudioClip audiClip)
	{
		if (audiClip) {
			mAuSourceBGMusic.loop = true;
			mAuSourceBGMusic.clip = audiClip;    //音频剪辑的赋值
			mAuSourceBGMusic.Play ();             //播放背景音乐
		}
	}

	/// <summary>
	/// 传入背景音乐为AudioSource
	/// </summary>
	/// <param name="audioSource">Audio source.</param>
	public static void PlayBGMusic (AudioSource audioSource)
	{
		if (audioSource) {
			audioSource.Play ();
		}
	}

	/// <summary>
	///传值 “音乐剪辑名称” 播放 “背景音乐” 
	/// </summary>
	/// <param name="strAudioClipName">背景音乐名称</param>
	public static void PlayBGMusic (string strAudioClipName)
	{
		
		if (!string.IsNullOrEmpty (strAudioClipName)) {
			if (mUseAudioClip) {
				PlayBGMusic (mDicClip [strAudioClipName]);
			} else {
				PlayBGMusic (mDicSource [strAudioClipName]);
			}
		}
	}

	/// <summary>
	///传值 “音乐剪辑” 播放 “背景音乐”
	/// </summary>
	/// <param name="audiClip">游戏音效剪辑</param>
	public static void PlayEffAudio (AudioClip audiClip)
	{   
		if (audiClip) {//播放
			AudioSource[] AS = mAudioManagerInstance.GetComponents<AudioSource> ();//先获取当前对象下的AudioSource
			for (int i = 1; i < AS.Length; i++) {//跳过下标0,0位预留给背景音乐
				if (!AS [i].isPlaying) {
					AS [i].clip = audiClip;//音频剪辑的赋值
					AS [i].Play ();//播放背景音乐
					return;
				} else {
					//元素被占用
				}
			}
			//当需要同时播放多个音效或当前对象的AudioSource不足,添加新AudioSource
			AudioSource NewAS = mAudioManagerInstance.AddComponent<AudioSource> ();
			NewAS.loop = false;
			NewAS.clip = audiClip;
			NewAS.Play ();
		}
	}

	/// <summary>
	/// 传入AudioSource时使用这个方法,这个方法目前不支持多个音效一起播放
	/// </summary>
	/// <param name="audioSource">Audio source.</param>
	public static void PlayEffAudio (AudioSource audioSource)
	{
		if (audioSource) {
			audioSource.Play ();
		}
	}

	/// <summary>
	///传值 “音乐剪辑名称” 播放 “游戏音效”
	/// </summary>
	/// <param name="strAudioClipName">游戏音效剪辑名称</param>
	public static void PlayEffAudio (string audioName)
	{
		if (!string.IsNullOrEmpty (audioName)) {
			if (mUseAudioClip) {
				PlayEffAudio (mDicClip [audioName]);
			} else {
				PlayEffAudio (mDicSource [audioName]);      
			}
		}        
	}
}
