/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using UnityEngine;
using System.Collections;

public class LAppDefine
{
	
	public static bool DEBUG_LOG = true;
	public static bool DEBUG_TOUCH_LOG = false;
	public static bool DEBUG_DRAW_HIT_AREA = false;
	
	
	
	public const float VIEW_MAX_SCALE = 2f;
	public const float VIEW_MIN_SCALE = 0.8f;
	
	public const float VIEW_LOGICAL_LEFT = -1;
	public const float VIEW_LOGICAL_RIGHT = 1;
	
	public const float VIEW_LOGICAL_MAX_LEFT = -2;
	public const float VIEW_LOGICAL_MAX_RIGHT = 2;
	public const float VIEW_LOGICAL_MAX_BOTTOM = -2;
	public const float VIEW_LOGICAL_MAX_TOP = 2;
	
	public const float SCREEN_WIDTH = 20.0f;
	public const float SCREEN_HEIGHT = 20.0f;
	

	//MOTION类目,目前官方范例全是用以下类目
	public const string MOTION_GROUP_IDLE = "idle";
	public const string MOTION_GROUP_TAP_BODY = "tap_body";
	public const string MOTION_GROUP_FLICK_HEAD = "flick_head";
	public const string MOTION_GROUP_PINCH_IN = "pinch_in";
	public const string MOTION_GROUP_PINCH_OUT = "pinch_out";
	public const string MOTION_GROUP_SHAKE = "shake";
	
	//点击区域
	public const string HIT_AREA_HEAD = "head";
	public const string HIT_AREA_BODY = "body";

	//PRIORITY
	public const int PRIORITY_NONE = 0;//什么都不做
	public const int PRIORITY_IDLE = 1;//可能是使该动画成为idle状态，因为暂时看不到效果所以不清楚
	public const int PRIORITY_NORMAL = 2;//可以正常插入的动作
	public const int PRIORITY_FORCE = 3;//强制播放动画，如果连点会抽筋
}