namespace FrizEngine
{
	[System.Serializable]
	public struct ScoreableObject
	{
		public int score;
		public int value;

		public ScoreableObject (int score, int value)
		{
			this.score = score;
			this.value = value;
		}
	}
}