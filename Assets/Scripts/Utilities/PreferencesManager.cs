using System.Collections.Generic;
using System.Linq;
using Saveables;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class PreferencesManager : MonoSingleton<PreferencesManager>
    {
        [HideInInspector]
        public UnityEvent<PreferencesManager> SettingsUpdated;
        
        [HideInInspector]
        public UnityEvent<PreferencesManager> ScoresUpdated;

        private const string _playerPrefencesName = "settings";
        private const string _scoresPrefsName = "storyScores";

        public Preferences Settings = new();

        public List<RunData> Runs = new();

        protected override void OnInit()
        {
            SettingsUpdated ??= new();
            ScoresUpdated ??= new();
            LoadPrefs();
            LoadScores();
        }

        public void SaveSettings(Preferences preferredPrefs)
        {
            Settings = preferredPrefs;
            var json = JsonUtility.ToJson(Settings);
            PlayerPrefs.SetString(_playerPrefencesName, json);
            SettingsUpdated.Invoke(this);
        }

        public void SaveScores(RunData data)
        {
            if(!Runs.Any(run => run.StartDateTime == data.StartDateTime))
            {
                Runs.Append(data);
            }
            else
            {
                var tempRun = Runs.First(run => run.StartDateTime == data.StartDateTime);
                Runs.Remove(tempRun);
                Runs.Append(data);
            }

            var json = JsonUtility.ToJson(Runs);
            PlayerPrefs.SetString(_scoresPrefsName, json);
            ScoresUpdated.Invoke(this);
        }

        public void LoadPrefs()
        {
            var json = PlayerPrefs.GetString(_playerPrefencesName);

            if(string.IsNullOrWhiteSpace(json))
            {
                Debug.Log("Could not load settings - recreating!");
                
                //Should create defaults here;
                Settings = new();
                
                PlayerPrefs.SetString(_playerPrefencesName, JsonUtility.ToJson(Settings));
                SettingsUpdated.Invoke(this);
                return;
            }
            else
            {
                Settings = JsonUtility.FromJson<Preferences>(json);
                SettingsUpdated.Invoke(this);
            }
        }

        public void LoadScores()
        {
            var json = PlayerPrefs.GetString(_scoresPrefsName);

            if(string.IsNullOrWhiteSpace(json))
            {
                Debug.Log("Could not load scores - recreating!");

                Runs = new();
                
                PlayerPrefs.SetString(_scoresPrefsName, JsonUtility.ToJson(Settings));
                ScoresUpdated.Invoke(this);
                return;
            }
            else
            {
                Runs = JsonUtility.FromJson<List<RunData>>(json);
                ScoresUpdated.Invoke(this);
            }
        }

        public RunData GetLatestRun()
        {
            var run = Runs.OrderByDescending(run => run.StartDateTime).FirstOrDefault();
            if (run == null)
            {
                run = new();
            }
            return run;
        }
    }
}
