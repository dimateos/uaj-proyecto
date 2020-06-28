﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TraceManagerMode {NONE, RECORD, REPRODUCE};

public class InputTraceManager : MonoBehaviour {

    private static InputTraceManager _instance = null;

    [Header("Saved Input")]
    private TraceManagerMode _traceMode = TraceManagerMode.NONE;
    private string _savedInputFilename = "";
    private string _savePath = "/TraceFiles/";

    [SerializeField] private SceneAsset[] _startScenes;
    [SerializeField] private SceneAsset[] _endScenes;

    private InputEventTrace _trace;
    private InputEventTrace.ReplayController _replayController;
    private bool _initialized = false;

    private List<string> _filenames;
    public Dropdown _dropdown;
    public GameObject _replayingIndicator, _recordingIndicator;

    public static InputTraceManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        // Do not destroy on scene load, for singleton pattern
        DontDestroyOnLoad(this.gameObject);
        _savePath = Application.persistentDataPath + _savePath;
        Debug.Log("Input Tracing: Path for saved files is '" + _savePath + "'");

        // Assign this as instance if there is none
        if (_instance == null) _instance = this;
        // Previous instances have priority
        else Destroy(this.gameObject);

        ReadFiles();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!_initialized) foreach (SceneAsset startScene in _startScenes)
                if (scene.name == startScene.name)
                    Init();
        else foreach (SceneAsset endScene in _endScenes)
                if (scene.name == endScene.name)
                    Quit();
    }

    private void Init() {
        if (_initialized) return;

        // Init the replay system
        switch (_traceMode) {

            // Do nothing
            case TraceManagerMode.NONE:
                _recordingIndicator.SetActive(false);
                _replayingIndicator.SetActive(false);
                break;

            // Saves input to file
            case TraceManagerMode.RECORD:
                // Filename based on date
                _savedInputFilename = "IT-" + System.DateTime.Now.ToString().Replace("/", "").Replace(" ", "_").Replace(":", ""); // format
                Debug.Log("Replay: Input will be saved to file '" + _savedInputFilename + "'");

                _trace = new InputEventTrace();
                _trace.Enable();
                _recordingIndicator.SetActive(true);
                _initialized = true;
                break;

            // Replays input from saved file
            case TraceManagerMode.REPRODUCE:
                /// todo: aqui hay que asignar el savedinputfilename
                Debug.Log("Replay: Loading Input Trace from file '" + _savedInputFilename + "'");

                if (File.Exists(_savePath + _savedInputFilename))
                    _trace = InputEventTrace.LoadFrom(_savePath + _savedInputFilename);
                else
                {
                    Debug.Log("Replay ERROR: File '" + _savedInputFilename + "' doesn't exist. Trace reproduction Cancelled.");
                    return;
                }

                foreach (InputDevice device in InputSystem.devices)
                    InputSystem.DisableDevice(device);

                _replayController = _trace.Replay();
                _replayController.WithAllDevicesMappedToNewInstances();
                _replayController.PlayAllEventsAccordingToTimestamps();
                _replayingIndicator.SetActive(true);
                _initialized = true;
                break;
        }
    }

    // Acquires all input trace file routes from the savePath
    private void ReadFiles()
    {
        _filenames = new List<string>();

        // If the directory does not exist, create it
        if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath);
        string[] allfiles = Directory.GetFiles(_savePath);

        for (int i = 0; i < allfiles.Length; i++)
        {
            //add filename skipping initial path
            int index = _savePath.Length;

            string filename = "";
            while (index < allfiles[i].Length)
            {
                filename += allfiles[i][index];
                index++;
            }

            _filenames.Add(filename);
        }

        //reverse so newest stays on top and is selected by default
        _filenames.Reverse();
    }

    public List<string> GetFilenames()
    {
        return _filenames;
    }

    public void SetFilename()
    {
        if (_dropdown != null && _filenames.Count > 0)
            _savedInputFilename = _filenames[_dropdown.value];
    }
    public string GetFilename() {
        return _savedInputFilename;
    }

    public void SetTraceMode(int mode)
    {
        _traceMode = (TraceManagerMode)mode;
    }

    private void Quit() {
        if (!_initialized) return;

        // Flush recorded data to file
        if (_traceMode == TraceManagerMode.RECORD) {
            Debug.Log("Replay: Saving Input to file '" + _savedInputFilename + "'");
            // Create save folder if it does not exist
            if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath);
            _trace.WriteTo(_savePath + _savedInputFilename);
        }
        else if (_traceMode == TraceManagerMode.REPRODUCE) {
            foreach (InputDevice device in InputSystem.devices)
                InputSystem.EnableDevice(device);

            _trace.Clear();
            _trace = null;
        }

        _initialized = false;
        return;
    }

    private void OnDestroy() {
        Quit();
    }
}
