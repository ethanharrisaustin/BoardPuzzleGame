namespace Saving
{   
    using UnityEngine;

    public class Room : MonoBehaviour
    {
        string saveName = "room";

        static Room instance;
        static SaveManager saveManager;

        int levelNumber = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            instance = this;

            GetSaveManager();
        }

        public static SaveManager GetSaveManager()
        {
            if (instance == null) return null;

            // If we don't have a saving manager or have moved to a different level, we load in a new saving manager for that level 
            if (saveManager == null || saveManager.saveName != instance.SaveName()) saveManager = new SaveManager(instance.SaveName());

            return saveManager;
        }

        public string SaveName()
        {
            return saveName + levelNumber;
        }

    }
}
