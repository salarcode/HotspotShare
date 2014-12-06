namespace HotspotShare.Classes
{
	public class Pair<TKey, TVal>
	{
		public TKey Key { get; set; }
		public TVal Value { get; set; }

		public Pair()
		{

		}
		public Pair(TKey key, TVal value)
		{
			Key = key;
			Value = value;
		}
	}
}
