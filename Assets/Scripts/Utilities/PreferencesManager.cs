using System;
using System.Collections.Generic;
using System.Linq;
using Saveables;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class PreferencesManager : MonoSingleton<PreferencesManager>
    {
        [Serializable]
        public class RunJson
        {
            public List<RunData> Runs;
        }

        [HideInInspector]
        public UnityEvent<PreferencesManager> SettingsUpdated;
        
        [HideInInspector]
        public UnityEvent<PreferencesManager> ScoresUpdated;

        private const string _playerPrefencesName = "settings";
        private const string _scoresPrefsName = "storyScores";

        public Preferences Settings = new();

        private RunJson _runJson = new();

        protected override void OnInit()
        {
            ScoresUpdated ??= new();
            LoadScores();
        }

        public void SaveScores(RunData data)
        {
            Debug.Log("Saving story scores...");
            if (_runJson.Runs == null)
            {
               _runJson.Runs = new();
            }

            if(!_runJson.Runs.Any(run => run.StartDateTime == data.StartDateTime))
            {
                _runJson.Runs.Add(data);
            }
            else
            {
                var tempRun = _runJson.Runs.First(run => run.StartDateTime == data.StartDateTime);
                _runJson.Runs.Remove(tempRun);
                _runJson.Runs.Add(data);
            }

            var json = JsonUtility.ToJson(_runJson);
            PlayerPrefs.SetString(_scoresPrefsName, json);
            ScoresUpdated.Invoke(this);
            Debug.Log("Updated scores");
        }

        public void LoadScores()
        {
            Debug.Log("Loading scores");
            var json = PlayerPrefs.GetString(_scoresPrefsName);

            if(string.IsNullOrWhiteSpace(json))
            {
                Debug.Log("Could not load scores - recreating!");

                _runJson = new()
                {
                    Runs = new()
                };
                
                PlayerPrefs.SetString(_scoresPrefsName, JsonUtility.ToJson(_runJson));
                ScoresUpdated.Invoke(this);
                return;
            }
            else
            {
                _runJson = JsonUtility.FromJson<RunJson>(json);
                ScoresUpdated.Invoke(this);
            }
        }

        public RunData GetLatestRun()
        {
            var run = _runJson.Runs.OrderByDescending(run => run.StartDateTime).FirstOrDefault(run => run.levelScores.Count > 0);
            if (run == null)
            {
                run = new()
                {
                    StartDateTime = DateTime.UtcNow.Ticks,
                    levelScores = new()
                };
            }
            return run;
        }
    }
}
