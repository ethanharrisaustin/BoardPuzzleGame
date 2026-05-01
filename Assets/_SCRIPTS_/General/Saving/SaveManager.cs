namespace Saving
{
	using System.Runtime.Serialization.Formatters.Binary;
	using System.IO;
	using System.Collections.Generic;
	using UnityEngine;
    using MapRooms;

    [System.Serializable]

	public class VariablesToSave
	{
		public Dictionary<string, float[]> SavedDataFloat = new Dictionary<string, float[]>();
		public Dictionary<string, double[]> SavedDataDouble = new Dictionary<string, double[]>();
		public Dictionary<string, int[]> SavedDataInt = new Dictionary<string, int[]>();
		public Dictionary<string, ulong[]> SavedDataUlong = new Dictionary<string, ulong[]>();
		public Dictionary<string, string[]> SavedDataString = new Dictionary<string, string[]>();
		public Dictionary<string, bool[]> SavedDataBool = new Dictionary<string, bool[]>();
		public Dictionary<string, Vector3ToSave[]> SavedDataVector3 = new Dictionary<string, Vector3ToSave[]>();
		public Dictionary<string, byte[]> SavedDataByteArray = new Dictionary<string, byte[]>();
		public Dictionary<string, RoomObjectSave[]> SavedDataRoomObjectArray = new Dictionary<string, RoomObjectSave[]>();
	}

	[System.Serializable]
	public class Vector3ToSave
	{
		public float x;
		public float y;
		public float z;

		public Vector3ToSave(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3ToSave(Vector3 vector3)
		{
			x = vector3.x;
			y = vector3.y;
			z = vector3.z;
		}

		public Vector3 ToVector3()
		{
			Vector3 vector3 = new Vector3();

			vector3.x = x;
			vector3.y = y;
			vector3.z = z;

			return vector3;
		}
	}

	public class SaveManager 
	{
		#region Variables

		VariablesToSave variablesSave = new VariablesToSave();

		[HideInInspector] public bool saveGameAutomatically = false;

		[HideInInspector] public float saveEverySeconds = 30f;

		[HideInInspector] public bool saveOnApplicationClose = true;

		// Saving stuff
		public FileStream fileStream;

		public static int profile = 0;

		public string saveName = "not set";

		#endregion

		#region Monobehaviour functions

		//float autoSaveTimer = 0f;
		private void Update()
		{
			DeleteDataListener();
		}

		//float deleteKeyDown = 0f;
		void DeleteDataListener()
		{
			/*
			if (Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Delete))
			{
				deleteKeyDown += Time.unscaledDeltaTime;

				if (deleteKeyDown > 4f)
				{
					saveOnApplicationClose = false;

					DeleteDataForAllProfiles();

					Application.Quit();
				}
			}
			else
			{
				deleteKeyDown = 0f;

			}
			*/
		}

		/*
		private void OnApplicationFocus(bool focus)
		{
			//if (saveOnApplicationClose) SaveGame();
		}

		private void OnApplicationPause(bool pause)
		{
			if (saveOnApplicationClose) if (pause) SaveGame();
		}

		private void OnApplicationQuit()
		{
			if (saveOnApplicationClose) SaveGame();
		}

		*/

		#endregion

		public SaveManager(string saveName)
		{
			this.saveName = saveName;

			LoadGame();
		}

		#region Saving & Loading

		public void LoadGame()
		{
			InitAll();

			if (CheckSave())
			{

				BinaryFormatter bf = new BinaryFormatter();
				fileStream = File.Open(Application.persistentDataPath + SetFileName(), FileMode.Open);
				variablesSave = (VariablesToSave)bf.Deserialize(fileStream);
				fileStream.Close();

				//SetAllProfiles();
			}
			else
			{
				variablesSave = new VariablesToSave();
			}
		}

		/// <summary>
		/// Switch profile number 
		/// </summary>
		/// <param name="profile"></param>
		public void SwitchToProfile(int profile)
		{
			SaveManager.profile = profile;

			//LoadGame();
		}

		public void SaveGame()
		{
			InitAll();

			SaveVariables();
		}

		void SaveVariables()
		{
			BinaryFormatter bf = new BinaryFormatter();
			fileStream = File.Create(Application.persistentDataPath + SetFileName());
			bf.Serialize(fileStream, variablesSave);
			fileStream.Close();
		}

		string SetFileName()
		{
			return "/" + saveName + profile + ".dat";
		}

		public bool CheckSave()
		{
			if (File.Exists(Application.persistentDataPath + SetFileName()))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public void NewGame()
		{
			// Reset the saved data
			variablesSave = new VariablesToSave();

			// And save the overwrite
			SaveGame();

			// And load this new game
			LoadGame();
		}

		#endregion

		#region Deleting Data

		public void DeleteDataForAllProfiles()
		{
			for (int i = 0; i < 10; ++i)
			{
				profile = i;
				File.Delete(Application.persistentDataPath + SetFileName());
			}

			//Debug.Log(Application.persistentDataPath + SetFileName());

			//Debug.Log("Deleted data for all profiles");
		}

		public void DeleteDataForProfile()
		{
			// Delete the actual file
			File.Delete(Application.persistentDataPath + SetFileName());

			//profileNames[profile] = "";

			//Debug.Log("Deleted profile '" + profile + "'");
		}

		public void DeleteDataForProfile(int _profile)
		{
			int currentProfile = profile;

			profile = _profile;

			// Delete the actual file
			File.Delete(Application.persistentDataPath + SetFileName());

			//profileNames[profile] = "";

			profile = currentProfile;

			//Debug.Log("Deleted profile " + _profile);
		}

		#endregion

		#region Setting Variables

		bool initAllBeenCalled = false;
		void InitAll()
		{
			if (initAllBeenCalled) return; // This just makes sure these will only be called once, only for performance reasons

			InitInt();
			InitUlong();
			InitDouble();
			InitString();
			InitBool();
			InitFloat();
			InitVector3();
			InitByte();
			InitRoomObject();

			initAllBeenCalled = true;
		}

		#region Save Ints

		void InitInt()
		{
			if (variablesSave.SavedDataInt != null) return;

			variablesSave.SavedDataInt = new Dictionary<string, int[]>();
		}

		public void SetIntArray(string dataName, int[] data)
		{
			// If this hasn't been made yet, or the length is incorrect, create the new data
			variablesSave.SavedDataInt[dataName] = new int[data.Length];

			// And then set it
			variablesSave.SavedDataInt[dataName] = data;
		}
		public void SetInt(string dataName, int data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataInt.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataInt[dataName] = new int[1];
			}

			// And then set it
			variablesSave.SavedDataInt[dataName][0] = data;
		}
		public void SetIntArray(string dataName, int data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataInt.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataInt[dataName] = new int[index + 1];
			}

			if (variablesSave.SavedDataInt[dataName].Length <= index)
			{
				variablesSave.SavedDataInt[dataName] = new int[index + 1];
			}

			// And then set it
			variablesSave.SavedDataInt[dataName][index] = data;
		}


		public int GetInt(string dataName)
		{
			return GetInt(dataName, 0);
		}
		public int GetInt(string dataName, int defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataInt.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataInt[dataName][0];
		}


		public int GetIntArray(string dataName, int index)
		{
			return GetIntArray(dataName, index, 0);
		}
		public int GetIntArray(string dataName, int index, int defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataInt.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataInt[dataName][index];
		}


		public int[] GetIntArray(string dataName)
		{
			return GetIntArray(dataName, new int[] { 0 });
		}
		public int[] GetIntArray(string dataName, int[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataInt.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataInt[dataName];
		}

		public bool IntExists(string dataName)
		{
			return variablesSave.SavedDataInt.ContainsKey(dataName);
		}

		#endregion

		#region Save Ulongs

		void InitUlong()
		{
			if (variablesSave.SavedDataUlong != null) return;

			variablesSave.SavedDataUlong = new Dictionary<string, ulong[]>();
		}

		public void SetUlongArray(string dataName, ulong[] data)
		{
			// If this hasn't been made yet, or the length is incorrect, create the new data
			variablesSave.SavedDataUlong[dataName] = new ulong[data.Length];

			// And then set it
			variablesSave.SavedDataUlong[dataName] = data;
		}
		public void SetUlong(string dataName, ulong data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataUlong.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataUlong[dataName] = new ulong[1];
			}

			// And then set it
			variablesSave.SavedDataUlong[dataName][0] = data;
		}
		public void SetUlongArray(string dataName, ulong data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataUlong.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataUlong[dataName] = new ulong[index + 1];
			}

			if (variablesSave.SavedDataUlong[dataName].Length <= index)
			{
				variablesSave.SavedDataUlong[dataName] = new ulong[index + 1];
			}

			// And then set it
			variablesSave.SavedDataUlong[dataName][index] = data;
		}

		public ulong GetUlong(string dataName)
		{
			return GetUlong(dataName, 0);
		}
		public ulong GetUlong(string dataName, ulong defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataUlong.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataUlong[dataName][0];
		}


		public ulong GetUlongArray(string dataName, int index)
		{
			return GetUlongArray(dataName, index, 0);
		}
		public ulong GetUlongArray(string dataName, int index, ulong defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataUlong.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataUlong[dataName][index];
		}


		public ulong[] GetUlongArray(string dataName)
		{
			return GetUlongArray(dataName, new ulong[] { 0 });
		}
		public ulong[] GetUlongArray(string dataName, ulong[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataUlong.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataUlong[dataName];
		}


		public bool UlongExists(string dataName)
		{
			return variablesSave.SavedDataUlong.ContainsKey(dataName);
		}

		#endregion

		#region	Save Doubles

		void InitDouble()
		{
			if (variablesSave.SavedDataDouble != null) return;

			variablesSave.SavedDataDouble = new Dictionary<string, double[]>();
		}

		public void SetDoubleArray(string dataName, double[] data)
		{
			// If this hasn't been made yet, or the length is incorrect, create the new data
			variablesSave.SavedDataDouble[dataName] = new double[data.Length];

			// And then set it
			variablesSave.SavedDataDouble[dataName] = data;
		}
		public void SetDouble(string dataName, double data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataDouble.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataDouble[dataName] = new double[1];
			}

			// And then set it
			variablesSave.SavedDataDouble[dataName][0] = data;
		}
		public void SetDoubleArray(string dataName, double data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataDouble.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataDouble[dataName] = new double[index + 1];
			}

			if (variablesSave.SavedDataDouble[dataName].Length <= index)
			{
				variablesSave.SavedDataDouble[dataName] = new double[index + 1];
			}

			// And then set it
			variablesSave.SavedDataDouble[dataName][index] = data;
		}


		public double GetDouble(string dataName)
		{
			return GetDouble(dataName, 0d);
		}
		public double GetDouble(string dataName, double defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataDouble.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataDouble[dataName][0];
		}


		public double GetDoubleArray(string dataName, int index)
		{
			return GetDoubleArray(dataName, index, 0d);
		}
		public double GetDoubleArray(string dataName, int index, double defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataDouble.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataDouble[dataName][index];
		}

		public double[] GetDoubleArray(string dataName)
		{
			return GetDoubleArray(dataName, new double[] { 0d });
		}
		public double[] GetDoubleArray(string dataName, double[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataDouble.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataDouble[dataName];
		}


		public bool DoubleExists(string dataName)
		{
			return variablesSave.SavedDataDouble.ContainsKey(dataName);
		}

		#endregion

		#region	Save Floats

		void InitFloat()
		{
			if (variablesSave.SavedDataFloat != null) return;

			variablesSave.SavedDataFloat = new Dictionary<string, float[]>();
		}

		public void SetFloatArray(string dataName, float[] data)
		{
			// If this hasn't been made yet, or the length is incorrect, create the new data
			variablesSave.SavedDataFloat[dataName] = new float[data.Length];

			// And then set it
			variablesSave.SavedDataFloat[dataName] = data;
		}
		public void SetFloat(string dataName, float data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataFloat.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataFloat[dataName] = new float[1];
			}

			// And then set it
			variablesSave.SavedDataFloat[dataName][0] = data;
		}
		public void SetFloatArray(string dataName, float data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataFloat.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataFloat[dataName] = new float[index + 1];
			}

			if (variablesSave.SavedDataFloat[dataName].Length <= index)
			{
				variablesSave.SavedDataFloat[dataName] = new float[index + 1];
			}

			// And then set it
			variablesSave.SavedDataFloat[dataName][index] = data;
		}


		public float GetFloat(string dataName)
		{
			return GetFloat(dataName, 0f);
		}
		public float GetFloat(string dataName, float defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataFloat.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataFloat[dataName][0];
		}


		public float GetFloatArray(string dataName, int index)
		{
			return GetFloatArray(dataName, index, 0f);
		}
		public float GetFloatArray(string dataName, int index, float defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataFloat.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataFloat[dataName][index];
		}

		public float[] GetFloatArray(string dataName)
		{
			return GetFloatArray(dataName, new float[] { 0f });
		}
		public float[] GetFloatArray(string dataName, float[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataFloat.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataFloat[dataName];
		}
		public bool FloatExists(string dataName)
		{
			return variablesSave.SavedDataFloat.ContainsKey(dataName);
		}

		#endregion

		#region Save Strings

		void InitString()
		{
			if (variablesSave.SavedDataString != null) return;

			variablesSave.SavedDataString = new Dictionary<string, string[]>();
		}

		public void SetStringArray(string dataName, string[] data)
		{
			// If this hasn't been made yet, or the length is incorrect, create the new data
			variablesSave.SavedDataString[dataName] = new string[data.Length];

			// And then set it
			variablesSave.SavedDataString[dataName] = data;
		}
		public void SetString(string dataName, string data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataString.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataString[dataName] = new string[1];
			}

			// And then set it
			variablesSave.SavedDataString[dataName][0] = data;
		}
		public void SetStringArray(string dataName, string data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataString.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataString[dataName] = new string[index + 1];
			}

			if (variablesSave.SavedDataString[dataName].Length <= index)
			{
				variablesSave.SavedDataString[dataName] = new string[index + 1];
			}

			// And then set it
			variablesSave.SavedDataString[dataName][index] = data;
		}

		public string GetString(string dateName)
		{
			return GetString(dateName, "");
		}
		public string GetString(string dataName, string defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataString.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataString[dataName][0];
		}

		public string GetStringArray(string dataName, int index)
		{
			return GetStringArray(dataName, index, "");
		}
		public string GetStringArray(string dataName, int index, string defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataString.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataString[dataName][index];
		}

		public string[] GetStringArray(string dataName)
		{
			return GetStringArray(dataName, new string[] { "" });
		}
		public string[] GetStringArray(string dataName, string[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataString.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataString[dataName];
		}


		public bool StringExists(string dataName)
		{
			return variablesSave.SavedDataString.ContainsKey(dataName);
		}

		#endregion

		#region Save Bool

		void InitBool()
		{
			if (variablesSave.SavedDataBool != null) return;

			variablesSave.SavedDataBool = new Dictionary<string, bool[]>();
		}

		public void SetBoolArray(string dataName, bool[] data)
		{
			// If this hasn't been made yet, or the length is incorrect, create the new data
			variablesSave.SavedDataBool[dataName] = new bool[data.Length];

			// And then set it
			variablesSave.SavedDataBool[dataName] = data;
		}
		public void SetBool(string dataName, bool data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataBool.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataBool[dataName] = new bool[1];
			}

			// And then set it
			variablesSave.SavedDataBool[dataName][0] = data;
		}
		public void SetBoolArray(string dataName, bool data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataBool.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataBool[dataName] = new bool[index + 1];
			}

			if (variablesSave.SavedDataBool[dataName].Length <= index)
			{
				variablesSave.SavedDataBool[dataName] = new bool[index + 1];
			}

			// And then set it
			variablesSave.SavedDataBool[dataName][index] = data;
		}


		public bool GetBool(string dataName)
		{
			return GetBool(dataName, false);
		}
		public bool GetBool(string dataName, bool defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataBool.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataBool[dataName][0];
		}

		public bool GetBoolArray(string dataName, int index)
		{
			return GetBoolArray(dataName, index, false);
		}
		public bool GetBoolArray(string dataName, int index, bool defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataBool.ContainsKey(dataName) == false) return defaultValue;

			if (variablesSave.SavedDataBool[dataName].Length <= index)
			{
				variablesSave.SavedDataBool[dataName] = new bool[index + 1];
			}

			// Otherwise return the data
			return variablesSave.SavedDataBool[dataName][index];
		}

		public bool[] GetBoolArray(string dataName)
		{
			return GetBoolArray(dataName, new bool[] { false });
		}
		public bool[] GetBoolArray(string dataName, bool[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataBool.ContainsKey(dataName) == false) return defaultValue;
			// Otherwise return the data
			return variablesSave.SavedDataBool[dataName];
		}


		public bool BoolExists(string dataName)
		{
			return variablesSave.SavedDataBool.ContainsKey(dataName);
		}

		#endregion

		#region Save Vector3

		void InitVector3()
		{
			if (variablesSave.SavedDataVector3 != null) return;

			variablesSave.SavedDataVector3 = new Dictionary<string, Vector3ToSave[]>();
		}

		public void SetVector3Array(string dataName, Vector3[] data)
		{
			// If this hasn't been made yet, or the length is incorrect, create the new data
			variablesSave.SavedDataVector3[dataName] = new Vector3ToSave[data.Length];

			// And then set it
			variablesSave.SavedDataVector3[dataName] = ConvertVector3ToVector3ToSave(data);
		}
		public void SetVector3(string dataName, Vector3 data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataVector3.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataVector3[dataName] = new Vector3ToSave[1];
			}

			// And then set it
			variablesSave.SavedDataVector3[dataName][0] = ConvertVector3ToVector3ToSave(data);
		}
		public void SetVector3Array(string dataName, Vector3 data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataVector3.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataVector3[dataName] = new Vector3ToSave[index + 1];
			}

			// If the data was too short, increase the size of the array
			if (variablesSave.SavedDataVector3[dataName].Length <= index)
			{
				variablesSave.SavedDataVector3[dataName] = IncreaseArraySize(variablesSave.SavedDataVector3[dataName], index + 1);
			}

			// And then set it
			variablesSave.SavedDataVector3[dataName][index] = ConvertVector3ToVector3ToSave(data);
		}

		public Vector3 GetVector3(string dataName)
		{
			return GetVector3(dataName, Vector3.zero);
		}
		public Vector3 GetVector3(string dataName, Vector3 defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataVector3.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataVector3[dataName][0].ToVector3();
		}

		public Vector3 GetVector3Array(string dataName, int index)
		{
			return GetVector3Array(dataName, index, Vector3.zero);
		}
		public Vector3 GetVector3Array(string dataName, int index, Vector3 defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataVector3.ContainsKey(dataName) == false) return defaultValue;

			// If it is too short
			if (variablesSave.SavedDataVector3[dataName].Length <= index)
			{
				variablesSave.SavedDataVector3[dataName] = IncreaseArraySize(variablesSave.SavedDataVector3[dataName], index + 1);
			}

			// Otherwise return the data
			return variablesSave.SavedDataVector3[dataName][index].ToVector3();
		}

		public Vector3[] GetVector3Array(string dataName)
		{
			return GetVector3Array(dataName, new Vector3[] { Vector3.zero });
		}
		public Vector3[] GetVector3Array(string dataName, Vector3[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataVector3.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return ConvertVector3ToSaveToVector3(variablesSave.SavedDataVector3[dataName]);
		}
		public bool Vector3Exists(string dataName)
		{
			return variablesSave.SavedDataVector3.ContainsKey(dataName);
		}

		Vector3ToSave[] IncreaseArraySize(Vector3ToSave[] current, int newLength)
		{
			Vector3ToSave[] returnValue = new Vector3ToSave[newLength];

			for (int i = 0; i < current.Length; ++i)
			{
				returnValue[i] = current[i];
			}

			return returnValue;
		}

		Vector3ToSave vector3ToSave = new Vector3ToSave(0, 0, 0);

		Vector3ToSave[] ConvertVector3ToVector3ToSave(Vector3[] array)
		{
			Vector3ToSave[] returnValues = new Vector3ToSave[array.Length];

			for (int i = 0; i < array.Length; ++i)
			{
				returnValues[i] = ConvertVector3ToVector3ToSave(array[i]);
			}

			return returnValues;
		}

		Vector3ToSave ConvertVector3ToVector3ToSave(Vector3 value)
		{
			vector3ToSave = new Vector3ToSave(0, 0, 0);

			vector3ToSave.x = value.x;
			vector3ToSave.y = value.y;
			vector3ToSave.z = value.z;

			return vector3ToSave;
		}

		Vector3[] ConvertVector3ToSaveToVector3(Vector3ToSave[] array)
		{
			Vector3[] returnValues = new Vector3[array.Length];

			for (int i = 0; i < array.Length; ++i)
			{
				returnValues[i] = array[i].ToVector3();
			}

			return returnValues;
		}

		#endregion

		#region Save Byte

		void InitByte()
		{
			if (variablesSave.SavedDataByteArray != null) return;

			variablesSave.SavedDataByteArray = new Dictionary<string, byte[]>();
		}

		public void SetByteArray(string dataName, byte[] data)
		{
			if (data == null) return;

			// If this hasn't been made yet, or the length is incorrect, create the new data
			//variablesSave.SavedDataByteArray[dataName] = new byte[];

			// And then set it
			variablesSave.SavedDataByteArray[dataName] = data;
		}

		public void SetByte(string dataName, byte data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataByteArray.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataByteArray[dataName] = new byte[1];
			}

			// And then set it
			variablesSave.SavedDataByteArray[dataName][0] = data;
		}
		public void SetByteArray(string dataName, byte data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataByteArray.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataByteArray[dataName] = new byte[index + 1];
			}

			// If the data was too short, increase the size of the array
			if (variablesSave.SavedDataByteArray[dataName].Length <= index)
			{
				variablesSave.SavedDataByteArray[dataName] = IncreaseArraySize(variablesSave.SavedDataByteArray[dataName], index + 1);
			}

			// And then set it
			variablesSave.SavedDataByteArray[dataName][index] = data;
		}


		public byte GetByte(string dataName)
		{
			return GetByte(dataName, 0);
		}
		public byte GetByte(string dataName, byte defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataByteArray.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataByteArray[dataName][0];
		}

		public byte GetByteArray(string dataName, int index)
		{
			return GetByteArray(dataName, index, 0);
		}
		public byte GetByteArray(string dataName, int index, byte defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataByteArray.ContainsKey(dataName) == false) return defaultValue;

			// If it is too short
			if (variablesSave.SavedDataByteArray[dataName].Length <= index)
			{
				variablesSave.SavedDataByteArray[dataName] = IncreaseArraySize(variablesSave.SavedDataByteArray[dataName], index + 1);
			}

			// Otherwise return the data
			return variablesSave.SavedDataByteArray[dataName][index];
		}


		public byte[] GetByteArray(string dataName)
		{
			return GetByteArray(dataName, new byte[0]);
		}
		public byte[] GetByteArray(string dataName, byte[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataByteArray.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataByteArray[dataName];
		}
		public bool ByteExists(string dataName)
		{
			return variablesSave.SavedDataByteArray.ContainsKey(dataName);
		}

		byte[] IncreaseArraySize(byte[] current, int newLength)
		{
			byte[] returnValue = new byte[newLength];

			for (int i = 0; i < current.Length; ++i)
			{
				returnValue[i] = current[i];
			}

			return returnValue;
		}

		#endregion

		#region Save Room Object

		void InitRoomObject()
		{
			if (variablesSave.SavedDataRoomObjectArray != null) return;

			variablesSave.SavedDataRoomObjectArray = new Dictionary<string, RoomObjectSave[]>();
		}

		public void SetRoomObjectArray(string dataName, RoomObjectSave[] data)
		{
			if (data == null) return;

			// If this hasn't been made yet, or the length is incorrect, create the new data
			//variablesSave.SavedDataByteArray[dataName] = new byte[];

			// And then set it
			variablesSave.SavedDataRoomObjectArray[dataName] = data;
		}

		public void SetRoomObject(string dataName, RoomObjectSave data)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataRoomObjectArray.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataRoomObjectArray[dataName] = new RoomObjectSave[1];
			}

			// And then set it
			variablesSave.SavedDataRoomObjectArray[dataName][0] = data;
		}
		public void SetRoomObjectArray(string dataName, RoomObjectSave data, int index)
		{
			// If this hasn't been made yet, create the new data
			if (variablesSave.SavedDataRoomObjectArray.ContainsKey(dataName) == false)
			{
				variablesSave.SavedDataRoomObjectArray[dataName] = new RoomObjectSave[index + 1];
			}

			// If the data was too short, increase the size of the array
			if (variablesSave.SavedDataRoomObjectArray[dataName].Length <= index)
			{
				variablesSave.SavedDataRoomObjectArray[dataName] = IncreaseArraySize(variablesSave.SavedDataRoomObjectArray[dataName], index + 1);
			}

			// And then set it
			variablesSave.SavedDataRoomObjectArray[dataName][index] = data;
		}


		public RoomObjectSave GetRoomObject(string dataName)
		{
			return GetRoomObject(dataName, null);
		}
		public RoomObjectSave GetRoomObject(string dataName, RoomObjectSave defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataByteArray.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataRoomObjectArray[dataName][0];
		}

		public RoomObjectSave GetRoomObjectArray(string dataName, int index)
		{
			return GetRoomObjectArray(dataName, index, null);
		}
		public RoomObjectSave GetRoomObjectArray(string dataName, int index, RoomObjectSave defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataRoomObjectArray.ContainsKey(dataName) == false) return defaultValue;

			// If it is too short
			if (variablesSave.SavedDataRoomObjectArray[dataName].Length <= index)
			{
				variablesSave.SavedDataRoomObjectArray[dataName] = IncreaseArraySize(variablesSave.SavedDataRoomObjectArray[dataName], index + 1);
			}

			// Otherwise return the data
			return variablesSave.SavedDataRoomObjectArray[dataName][index];
		}


		public RoomObjectSave[] GetRoomObjectArray(string dataName)
		{
			return GetRoomObjectArray(dataName, new RoomObjectSave[0]);
		}
		public RoomObjectSave[] GetRoomObjectArray(string dataName, RoomObjectSave[] defaultValue)
		{
			// If data doesn't exist
			if (variablesSave.SavedDataByteArray.ContainsKey(dataName) == false) return defaultValue;

			// Otherwise return the data
			return variablesSave.SavedDataRoomObjectArray[dataName];
		}
		public bool RoomObjectExists(string dataName)
		{
			return variablesSave.SavedDataRoomObjectArray.ContainsKey(dataName);
		}

		RoomObjectSave[] IncreaseArraySize(RoomObjectSave[] current, int newLength)
		{
			RoomObjectSave[] returnValue = new RoomObjectSave[newLength];

			for (int i = 0; i < current.Length; ++i)
			{
				returnValue[i] = current[i];
			}

			return returnValue;
		}

		#endregion


		#endregion
	}
}