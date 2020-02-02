namespace LevelEditor {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
	using System;
    [System.Serializable]
	public class Level : ICloneable{
		public Vector2Int gridSize;
		public List<CellView> cellViews;
		public string name;
        public Level(Vector2Int gridSize,List<CellView> cellViews,string name)
		{
			this.cellViews=cellViews;
			this.gridSize = gridSize;
			this.name=name;
		}
        public object Clone()
        {
			return this;
        }
	}
}