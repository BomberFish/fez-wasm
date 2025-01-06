namespace EasyStorage
{
	public interface ISaveDevice
	{
		bool Save(string fileName, SaveAction saveAction);

		bool Load(string fileName, LoadAction loadAction);

		bool Delete(string fileName);

		bool FileExists(string fileName);
	}
}
