using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum TraceManagerMode {RECORD, REPRODUCE, NONE};

public class InputTraceManager : MonoBehaviour
{
    
    private static InputTraceManager _instance = null;

    [Header("Saved Input")]
    [SerializeField] private TraceManagerMode _traceMode = TraceManagerMode.NONE;
    [SerializeField] private string _savedInputFilename;
    private string _savePath = "/TraceFiles/";


    private InputEventTrace _trace;
    private InputEventTrace.ReplayController _replayController;

    public static InputTraceManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null) _instance = this;
        else Destroy(this.gameObject);
        _savePath = Application.persistentDataPath + _savePath;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Init the replay system
        switch (_traceMode)
        {
            // Replays input from saved file
            case TraceManagerMode.REPRODUCE:
                /// todo: aqui hay que asignar el savedinputfilename
                Debug.Log("Replay: Loading Input Trace from file '" + _savedInputFilename + "'");
                _trace = InputEventTrace.LoadFrom(_savePath + _savedInputFilename);
                _replayController = _trace.Replay();
                _replayController.PlayAllEventsAccordingToTimestamps();
                break;
            // Saves input to file
            case TraceManagerMode.RECORD:
                Debug.Log("Replay: Input will be saved to file '" + _savedInputFilename + "'");
                _trace = new InputEventTrace();
                _trace.Enable();
                // Filename based on date
                _savedInputFilename = "IT-" + System.DateTime.Now.ToString().Replace("/", "").Replace(" ", "_").Replace(":", ""); // format
                break;
            case TraceManagerMode.NONE:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        // Flush recorded data to file
        if (_traceMode == TraceManagerMode.RECORD)
        {
            Debug.Log("Replay: Saving Input to file '" + _savedInputFilename + "'");
            // Create save folder if it does not exist
            if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath); 
            _trace.WriteTo(_savePath + _savedInputFilename);
        }
        return;
    }
}
