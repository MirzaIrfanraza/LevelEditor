﻿namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	[System.Serializable]
	public class Cell  
	{
		public int toolId;
		public Cell(int toolId)
		{
			this.toolId=toolId;
		}
	}
}