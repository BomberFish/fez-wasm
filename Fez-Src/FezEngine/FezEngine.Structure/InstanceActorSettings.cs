using System;
using Common;
using ContentSerialization.Attributes;

namespace FezEngine.Structure
{
	public class InstanceActorSettings : IEquatable<InstanceActorSettings>
	{
		public const int Steps = 16;

		[Serialization(Ignore = true)]
		public bool Inactive { get; set; }

		[Serialization(Optional = true)]
		public int? ContainedTrile { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public string SignText { get; set; }

		[Serialization(Optional = true)]
		public bool[] Sequence { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public string SequenceSampleName { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public string SequenceAlternateSampleName { get; set; }

		[Serialization(Optional = true)]
		public int? HostVolume { get; set; }

		public InstanceActorSettings()
		{
		}

		public InstanceActorSettings(InstanceActorSettings copy)
		{
			ContainedTrile = copy.ContainedTrile;
			SignText = copy.SignText;
			if (copy.Sequence != null)
			{
				Sequence = new bool[16];
				Array.Copy(copy.Sequence, Sequence, 16);
			}
			SequenceSampleName = copy.SequenceSampleName;
			SequenceAlternateSampleName = copy.SequenceAlternateSampleName;
			HostVolume = copy.HostVolume;
		}

		public override int GetHashCode()
		{
			return Util.CombineHashCodes(ContainedTrile, SignText, Sequence, SequenceSampleName, SequenceAlternateSampleName, HostVolume);
		}

		public bool Equals(InstanceActorSettings other)
		{
			return (object)other != null && other.ContainedTrile == ContainedTrile && other.SignText == SignText && object.Equals(other.Sequence, Sequence) && other.SequenceSampleName == SequenceSampleName && other.SequenceAlternateSampleName == SequenceAlternateSampleName && other.HostVolume == HostVolume;
		}

		public override bool Equals(object obj)
		{
			return obj is InstanceActorSettings && Equals(obj as InstanceActorSettings);
		}

		public static bool operator ==(InstanceActorSettings lhs, InstanceActorSettings rhs)
		{
			return lhs?.Equals(rhs) ?? ((object)rhs == null);
		}

		public static bool operator !=(InstanceActorSettings lhs, InstanceActorSettings rhs)
		{
			return !(lhs == rhs);
		}
	}
}
