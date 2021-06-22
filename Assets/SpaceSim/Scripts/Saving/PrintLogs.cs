using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SpaceSim.Saving
{
    public class PrintLogs : MonoBehaviour
    {
        [SerializeField]
        private bool clearLogsOnStart = false;
        public static PrintLogs Instance;
        private string saveFilePath;

        void Start() {
            if (clearLogsOnStart) ClearLogs();
            
            saveFilePath = Application.persistentDataPath + "/logs.txt";

            string startText = "\n\nNew session begin, saving logs to: " + saveFilePath;

            Debug.Log(startText);
        }

        private void OnEnable() => Application.logMessageReceived += AppendLog;

        private void OnDisable() => Application.logMessageReceived -= AppendLog;

        public void AppendLog(string addedText, string stackTrace, LogType type) {
            if (!File.Exists(saveFilePath)) {
                File.Create(saveFilePath);
            }

            try {
                string oldText = File.ReadAllText(saveFilePath);
                string newText = oldText + "[" + DateTime.Now.ToString() + "] " + addedText + "\n";
                File.WriteAllText(saveFilePath, newText);
            }
            catch (IOException e) {
                Application.logMessageReceived -= AppendLog;
                Debug.LogError(e + " Error logged to file");
            }
            catch (Exception e) {
                Application.logMessageReceived -= AppendLog;
                Debug.LogError(e + " Error logged to file");
            }
        }

        public void ClearLogs() {
            if (!File.Exists(saveFilePath)) {
                File.Create(saveFilePath);
            }

            File.WriteAllText(saveFilePath, "");
        }
    }
}