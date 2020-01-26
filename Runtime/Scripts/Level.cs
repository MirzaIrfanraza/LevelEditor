namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	[System.Serializable]
	public class Level {
		public List<CellView> cellViews;
		public Level(List<CellView> cellViews)
		{
			this.cellViews=cellViews;
		}	
	}
}