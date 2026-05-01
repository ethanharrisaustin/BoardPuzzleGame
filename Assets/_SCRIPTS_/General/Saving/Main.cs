namespace Saving
{   
    using UnityEngine;

    public class Main : MonoBehaviour
    {
        const string saveName = "main";

        static SaveManager saveManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            // Loads in save file
            saveManager = new SaveManager(saveName);
        }

        public static SaveManager GetSaveManager()
        {
            return saveManager;
        }
    }
}
