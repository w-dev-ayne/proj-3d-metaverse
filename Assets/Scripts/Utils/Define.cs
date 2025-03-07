using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Define
{

	public enum UIEvent
	{
		Click,
		Pressed,
		PointerDown,
		PointerUp,
	}

	public enum Scene
	{
		Unknown,
		Loading,
		Main,
		Lab1,
		Lab2
	}
	
	public enum Sound
	{
		Bgm,
		Effect,
		Speech,
		Effect2,
		Effect3,
		Max,
	}

	public enum QuizAnswer
	{
		O,
		X
	}

	public enum TriggerEventType
	{
		Info,
		Quiz,
		Teleport,
		Collect,
		Workbench,
		Custom,
		FailZone,
		Item
	}

	public enum CollectWord
	{
		G,
		E,
		N,
		O
	}

	public enum Place
	{
		Hall,
		Lab1,
		Lab2,
		None
	}

	public enum QuestState
	{
		UnCompleted,
		OnGoing,
		Completed
	}

	public enum QuestType
	{
		Daily,
		Sub,
		None
	}

	public enum Language
	{
		Korean,
		English
	}

	public enum WorkbenchType
	{
		Sun,
		Water,
		Done
	}

	public enum QuizType
	{
		Crypto,
		DNA,
		Max
	}
}
