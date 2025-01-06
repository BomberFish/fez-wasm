using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDL
{
	public class Tag
	{
		private string sdlNamespace;

		private string name;

		private List<object> values;

		private Dictionary<string, string> attributeToNamespace;

		private Dictionary<string, object> attributes;

		private List<Tag> children;

		private List<object> valuesSnapshot;

		private bool valuesDirty;

		private Dictionary<string, object> attributesSnapshot;

		private bool attributesDirty;

		private Dictionary<string, string> attributeToNamespaceSnapshot;

		private List<Tag> childrenSnapshot;

		private bool childrenDirty;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = SDLUtil.ValidateIdentifier(value);
			}
		}

		public string SDLNamespace
		{
			get
			{
				return sdlNamespace;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value.Length != 0)
				{
					SDLUtil.ValidateIdentifier(value);
				}
				sdlNamespace = value;
			}
		}

		public object Value
		{
			get
			{
				if (values.Count == 0)
				{
					return null;
				}
				return values[0];
			}
			set
			{
				value = SDLUtil.CoerceOrFail(value);
				if (values.Count == 0)
				{
					AddValue(value);
				}
				else
				{
					values[0] = value;
				}
				valuesDirty = true;
			}
		}

		public IList<Tag> Children
		{
			get
			{
				if (childrenDirty)
				{
					childrenSnapshot = new List<Tag>(children);
				}
				return childrenSnapshot;
			}
			set
			{
				childrenDirty = true;
				children = new List<Tag>(value);
			}
		}

		public IList<object> Values
		{
			get
			{
				if (valuesDirty)
				{
					valuesSnapshot = new List<object>(values);
				}
				return valuesSnapshot;
			}
			set
			{
				valuesDirty = true;
				values.Clear();
				foreach (object item in value)
				{
					AddValue(item);
				}
			}
		}

		public IDictionary<string, object> Attributes
		{
			get
			{
				if (attributesDirty)
				{
					EnsureAttributesInitialized();
					attributesSnapshot = new Dictionary<string, object>(attributes);
				}
				return attributesSnapshot;
			}
			set
			{
				attributesDirty = true;
				EnsureAttributesInitialized();
				attributes.Clear();
				foreach (string key in value.Keys)
				{
					this[key] = value[key];
				}
			}
		}

		public IDictionary<string, string> AttributeToNamespace
		{
			get
			{
				if (attributesDirty)
				{
					EnsureAttributesInitialized();
					attributeToNamespaceSnapshot = new Dictionary<string, string>(attributeToNamespace);
				}
				return attributeToNamespaceSnapshot;
			}
		}

		public object this[string key]
		{
			get
			{
				if (attributes == null)
				{
					return null;
				}
				attributes.TryGetValue(key, out var value);
				return value;
			}
			set
			{
				this["", key] = value;
			}
		}

		public object this[string sdlNamespace, string key]
		{
			set
			{
				attributesDirty = true;
				EnsureAttributesInitialized();
				attributes[SDLUtil.ValidateIdentifier(key)] = SDLUtil.CoerceOrFail(value);
				if (sdlNamespace == null)
				{
					sdlNamespace = "";
				}
				if (sdlNamespace.Length != 0)
				{
					SDLUtil.ValidateIdentifier(sdlNamespace);
				}
				attributeToNamespace[key] = sdlNamespace;
			}
		}

		public object this[int index]
		{
			get
			{
				return values[index];
			}
			set
			{
				valuesDirty = true;
				values[index] = SDLUtil.CoerceOrFail(value);
			}
		}

		public Tag(string name)
			: this("", name)
		{
		}

		public Tag(string sdlNamespace, string name)
		{
			SDLNamespace = sdlNamespace;
			Name = name;
			values = new List<object>();
			children = new List<Tag>();
			attributesDirty = (childrenDirty = (valuesDirty = true));
		}

		private void EnsureAttributesInitialized()
		{
			if (attributes == null)
			{
				attributes = new Dictionary<string, object>();
				attributeToNamespace = new Dictionary<string, string>();
			}
		}

		public void AddChild(Tag child)
		{
			childrenDirty = true;
			children.Add(child);
		}

		public bool RemoveChild(Tag child)
		{
			childrenDirty = true;
			return children.Remove(child);
		}

		public void AddValue(object value)
		{
			valuesDirty = true;
			values.Add(SDLUtil.CoerceOrFail(value));
		}

		public bool RemoveValue(object value)
		{
			valuesDirty = true;
			return values.Remove(value);
		}

		public IList<Tag> GetChildren(bool recursively)
		{
			if (!recursively)
			{
				return Children;
			}
			List<Tag> list = new List<Tag>();
			foreach (Tag child in Children)
			{
				list.Add(child);
				if (recursively)
				{
					list.AddRange(child.GetChildren(recursively: true));
				}
			}
			return list;
		}

		public Tag GetChild(string childName)
		{
			return GetChild(childName, recursive: false);
		}

		public Tag GetChild(string childName, bool recursive)
		{
			foreach (Tag child2 in children)
			{
				if (child2.Name.Equals(childName))
				{
					return child2;
				}
				if (recursive)
				{
					Tag child = child2.GetChild(childName, recursive: true);
					if (child != null)
					{
						return child;
					}
				}
			}
			return null;
		}

		public IList<Tag> GetChildren(string childName)
		{
			return GetChildren(childName, recursive: false);
		}

		public IList<Tag> GetChildren(string childName, bool recursive)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag child in children)
			{
				if (child.Name.Equals(childName))
				{
					list.Add(child);
				}
				if (recursive)
				{
					list.AddRange(child.GetChildren(childName, recursive: true));
				}
			}
			return list;
		}

		public IList<object> GetChildrenValues(string name)
		{
			List<object> list = new List<object>();
			IList<Tag> list2 = GetChildren(name);
			foreach (Tag item in list2)
			{
				IList<object> list3 = item.Values;
				if (list3.Count == 0)
				{
					list.Add(null);
				}
				else if (list3.Count == 1)
				{
					list.Add(list3[0]);
				}
				else
				{
					list.Add(list3);
				}
			}
			return list;
		}

		public IList<Tag> GetChildrenForNamespace(string sdlNamespace)
		{
			return GetChildrenForNamespace(sdlNamespace, recursive: false);
		}

		public IList<Tag> GetChildrenForNamespace(string sdlNamespace, bool recursive)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag child in children)
			{
				if (child.SDLNamespace.Equals(sdlNamespace))
				{
					list.Add(child);
				}
				if (recursive)
				{
					list.AddRange(child.GetChildrenForNamespace(sdlNamespace, recursive: true));
				}
			}
			return list;
		}

		public IDictionary<string, object> GetAttributesForNamespace(string sdlNamespace)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			EnsureAttributesInitialized();
			foreach (string key in attributeToNamespace.Keys)
			{
				if (attributeToNamespace[key].Equals(sdlNamespace))
				{
					dictionary[key] = attributes[key];
				}
			}
			return dictionary;
		}

		public Tag ReadFile(string file)
		{
			return Read(new StreamReader(file, Encoding.UTF8));
		}

		public Tag ReadString(string text)
		{
			return Read(new StreamReader(text));
		}

		public Tag Read(StreamReader reader)
		{
			IList<Tag> list = new Parser(reader).Parse();
			foreach (Tag item in list)
			{
				AddChild(item);
			}
			return this;
		}

		public void WriteFile(string file)
		{
			WriteFile(file, includeRoot: false);
		}

		public void WriteFile(string file, bool includeRoot)
		{
			using StreamWriter writer = new StreamWriter(file, append: false, Encoding.UTF8);
			Write(writer, includeRoot);
		}

		public void Write(TextWriter writer, bool includeRoot)
		{
			string value = "\r\n";
			if (includeRoot)
			{
				writer.Write(ToString());
			}
			else
			{
				for (int i = 0; i < children.Count; i++)
				{
					writer.Write(children[i].ToString());
					if (i < children.Count - 1)
					{
						writer.Write(value);
					}
				}
			}
			writer.Close();
		}

		public override string ToString()
		{
			return ToString("");
		}

		private string ToString(string linePrefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(linePrefix);
			bool flag = false;
			if (sdlNamespace.Length == 0 && name.Equals("content"))
			{
				flag = true;
			}
			else
			{
				if (sdlNamespace.Length != 0)
				{
					stringBuilder.Append(sdlNamespace).Append(':');
				}
				stringBuilder.Append(name);
			}
			if (values.Count != 0)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(" ");
				}
				int count = values.Count;
				for (int i = 0; i < count; i++)
				{
					stringBuilder.Append(SDLUtil.Format(values[i]));
					if (i < count - 1)
					{
						stringBuilder.Append(" ");
					}
				}
			}
			if (attributes != null && attributes.Count != 0)
			{
				foreach (string key in attributes.Keys)
				{
					stringBuilder.Append(" ");
					string text = AttributeToNamespace[key];
					if (!text.Equals(""))
					{
						stringBuilder.Append(text + ":");
					}
					stringBuilder.Append(key + "=");
					stringBuilder.Append(SDLUtil.Format(attributes[key]));
				}
			}
			if (children.Count != 0)
			{
				if (!flag)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append("{\r\n");
				foreach (Tag child in children)
				{
					stringBuilder.Append(child.ToString(linePrefix + "\t") + "\r\n");
				}
				stringBuilder.Append(linePrefix + "}");
			}
			return stringBuilder.ToString();
		}

		public string ToXMLString()
		{
			return ToXMLString("");
		}

		private string ToXMLString(string linePrefix)
		{
			string text = "\r\n";
			if (linePrefix == null)
			{
				linePrefix = "";
			}
			StringBuilder stringBuilder = new StringBuilder(linePrefix + "<");
			if (!sdlNamespace.Equals(""))
			{
				stringBuilder.Append(sdlNamespace + ":");
			}
			stringBuilder.Append(name);
			if (values.Count != 0)
			{
				int num = 0;
				foreach (object value in values)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append("_val" + num + "=\"" + SDLUtil.Format(value, addQuotes: false) + "\"");
					num++;
				}
			}
			if (attributes != null && attributes.Count != 0)
			{
				foreach (string key in attributes.Keys)
				{
					stringBuilder.Append(" ");
					string text2 = attributeToNamespace[key];
					if (!text2.Equals(""))
					{
						stringBuilder.Append(text2 + ":");
					}
					stringBuilder.Append(key + "=");
					stringBuilder.Append("\"" + SDLUtil.Format(attributes[key], addQuotes: false) + "\"");
				}
			}
			if (children.Count != 0)
			{
				stringBuilder.Append(">" + text);
				foreach (Tag child in children)
				{
					stringBuilder.Append(child.ToXMLString(linePrefix + "    ") + text);
				}
				stringBuilder.Append(linePrefix + "</");
				if (!sdlNamespace.Equals(""))
				{
					stringBuilder.Append(sdlNamespace + ":");
				}
				stringBuilder.Append(name + ">");
			}
			else
			{
				stringBuilder.Append("/>");
			}
			return stringBuilder.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			return ToString().Equals(obj.ToString());
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
