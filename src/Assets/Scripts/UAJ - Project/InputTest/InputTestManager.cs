using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR;

public class InputTestManager : MonoBehaviour
{
    public enum recordState {recording, playing, none };


    public GameObject player, mirror;
    public GameObject recordUI, playUI;

    private InputEventTrace record = null;
    private recordState state = recordState.none;
    private GameObject initial;
    private Keyboard keyboard = null, keyboard2 = null;
    private InputEventTrace.ReplayController replay;

    private Vector3 offset = new Vector3(-8.902f, 0);

    void Start()
    {
        if(player == null || mirror == null)
        {
            Debug.LogError("Missing player or mirror GameObjects");
            Destroy(this);
        }

        keyboard = (Keyboard)InputSystem.GetDevice("Keyboard1");
        keyboard2 = (Keyboard)InputSystem.GetDevice("Keyboard2");
        foreach (UnityEngine.InputSystem.InputDevice device in InputSystem.devices)
        {
            InputSystem.EnableDevice(device);
            //Debug.Log(device.deviceId);
            //Debug.Log(device.name);
        }
        

        mirror.GetComponent<SimpleMovement>().enabled = false;
        initial = new GameObject();
    }

    void Update()
    {
        if(state == recordState.playing && replay.finished)
        {
            state = recordState.none;
            replay.paused = true;
            mirror.GetComponent<SimpleMovement>().enabled = false;
            player.GetComponent<SimpleMovement>().enabled = true;
            InputSystem.EnableDevice(keyboard);

            UpdateUI();
        }
    }

    public void Record()
    {
        switch (state)
        {
            case recordState.recording:
                Stop();
                break;
            case recordState.playing:
                break;
            case recordState.none:
                StartRecord();
                break;
            default:
                break;
        }
        UpdateUI();
    }

    public void Stop()
    {
        switch (state)
        {
            case recordState.recording:
                record.Disable();
                break;
            case recordState.playing:
                replay.paused = true;
                mirror.GetComponent<SimpleMovement>().enabled = false;
                player.GetComponent<SimpleMovement>().enabled = true;
                InputSystem.EnableDevice(keyboard);
                break;
            case recordState.none:
                break;
            default:
                break;
        }
        state = recordState.none;
        UpdateUI();
    }

    public void Play()
    {
        if (state == recordState.recording)
            Stop();
        else
            StartMirror();

        UpdateUI();
    }

    private void StartMirror()
    {
        if (record == null) return;
        mirror.GetComponent<SimpleMovement>().enabled = true;
        player.GetComponent<SimpleMovement>().enabled = false;

        
        InputSystem.DisableDevice(keyboard);
        mirror.transform.position = initial.transform.position + offset;
        mirror.transform.rotation = initial.transform.rotation;

        replay = record.Replay();
        //replay.WithAllDevicesMappedToNewInstances();
        replay.WithDeviceMappedFromTo(keyboard, keyboard2);
        replay.PlayAllEventsAccordingToTimestamps();
        replay.paused = false;
        state = recordState.playing;
    }

    private void StartRecord()
    {
        initial.transform.position = player.transform.position;
        initial.transform.rotation = player.transform.rotation;
        record = new InputEventTrace();
        record.Enable();
        state = recordState.recording;
    }

    private void OnDestroy()
    {
        if(replay != null)
            replay.Dispose();
    }

    private void UpdateUI()
    {
        switch (state)
        {
            case recordState.recording:
                recordUI.SetActive(true);
                playUI.SetActive(false);
                break;
            case recordState.playing:
                recordUI.SetActive(false);
                playUI.SetActive(true);
                break;
            case recordState.none:
                recordUI.SetActive(false);
                playUI.SetActive(false);
                break;
            default:
                break;
        }
    }
}
