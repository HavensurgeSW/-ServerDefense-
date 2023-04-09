using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

using TMPro;

public class TerminalManager : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] private TMP_InputField terminalInput = null;
    [SerializeField] private GameObject cmdEntryPrefab = null;
    [SerializeField] private Transform logHolder = null;
    
    [Header("History Configuration")]
    [SerializeField] private int maxCachedInputsCount = 5;

    private List<CmdEntry> activeEntries = null;
    private ObjectPool<CmdEntry> entriesPool = null;
    private List<string> userHistory = null;
    private int currentHistoryIndex = 0;
    private bool isEnabled = true;

    private Action<string> OnHistoryInput = null;
    private Action<string> OnInputCommand = null;

    private void Update()
    {
        if (!isEnabled)
        {
            return;
        }

        HandleTerminalInput();
        HandleUserHistory();
    }

    public void Init(Action<string> onInputCommand)
    {
        OnInputCommand = onInputCommand;
        OnHistoryInput = UpdateInputField;

        userHistory = new List<string>();
        activeEntries = new List<CmdEntry>();
        entriesPool = new ObjectPool<CmdEntry>(CreateEntry, GetEntry, ReleaseEntry);

        SelectInputField();
    }

    public void AddInterpreterLines(List<string> userInput)
    {
        ClearCmdEntries();
        GenerateCmdEntries(userInput);
    }

    public void AddInterpreterLines(TerminalResponseSO response)
    {
        ClearCmdEntries();
        GenerateCmdEntries(response);
    }

    public void ToggleTerminalInteraction(bool status)
    {
        isEnabled = status;
        terminalInput.enabled = status;
        terminalInput.interactable = status;

        if (status)
        {
            SelectInputField();
            terminalInput.MoveTextEnd(false);
        }
        else
        {
            terminalInput.DeactivateInputField();
        }
    }

    public TerminalResponseSO GenerateCustomTerminalResponse(List<string> lines)
    {
        TerminalResponseSO responseSO = ScriptableObject.CreateInstance<TerminalResponseSO>();
        responseSO.OverrideResponse(lines);
        return responseSO;
    }

    public void DeleteGeneratedTerminalResponse(TerminalResponseSO response)
    {
        response.ClearResponses();
        ScriptableObject.Destroy(response);
    }

    private void HandleTerminalInput()
    {
        if (string.IsNullOrEmpty(terminalInput.text))
        {
            SelectInputField();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            InputTerminalText(terminalInput.text);
            ClearInputField();
            SelectInputField();
        }        
    }

    private void HandleUserHistory()
    {
        if (userHistory == null || userHistory.Count == 0)
        {
            return;
        }

        bool updateText = false;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentHistoryIndex >= userHistory.Count)
            {
                return;
            }

            if (currentHistoryIndex < userHistory.Count - 1)
            {
                currentHistoryIndex++;
            }

            updateText = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentHistoryIndex--;

            if (currentHistoryIndex <= 0)
            {
                currentHistoryIndex = 0;
            }

            updateText = true;
        }

        if (updateText)
        {
            OnHistoryInput?.Invoke(userHistory[currentHistoryIndex]);
        }
    }

    private void InputTerminalText(string text)
    {
        OnInputCommand(text);

        userHistory.Add(text);

        if (userHistory.Count > maxCachedInputsCount)
        {
            userHistory.RemoveAt(0);
        }

        currentHistoryIndex = userHistory.Count;
    }

    private void SelectInputField()
    {
        terminalInput.ActivateInputField();
        terminalInput.Select();
    }

    private void ClearInputField()
    {
        terminalInput.text = string.Empty;
    }

    private void UpdateInputField(string userInput) 
    {
        terminalInput.text = userInput;
        terminalInput.MoveTextEnd(false);
    }

    private void GenerateCmdEntries(List<string> userInput)
    {
        for (int i = 0; i < userInput.Count; i++)
        {
            CmdEntry entry = entriesPool.Get();
            entry.SetSiblingIndex(i);
            entry.SetText(userInput[i]);
            activeEntries.Add(entry);
        }
    }

    private void GenerateCmdEntries(TerminalResponseSO response)
    {
        for (int i = 0; i < response.RESPONSE.Count; i++)
        {
            CmdEntry entry = entriesPool.Get();
            entry.SetSiblingIndex(i);
            entry.SetText(response.RESPONSE[i]);
            activeEntries.Add(entry);
        }
    }

    public void ClearCmdEntries()
    {
        for (int i = 0; i < activeEntries.Count; i++)
        {
            entriesPool.Release(activeEntries[i]);
        }

        activeEntries.Clear();
    }

    private CmdEntry CreateEntry()
    {
        return Instantiate(cmdEntryPrefab, logHolder).GetComponent<CmdEntry>();
    }

    private void GetEntry(CmdEntry entry)
    {
        entry.ToggleStatus(true);
    }

    private void ReleaseEntry(CmdEntry entry)
    {
        entry.SetText(string.Empty);
        entry.ToggleStatus(false);
    }
}
