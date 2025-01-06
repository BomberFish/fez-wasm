using System;

namespace Common
{
	public class EndTriggerAttribute : Attribute
	{
		public string Trigger { get; set; }

		public EndTriggerAttribute(string trigger)
		{
			Trigger = trigger;
		}
	}
}
