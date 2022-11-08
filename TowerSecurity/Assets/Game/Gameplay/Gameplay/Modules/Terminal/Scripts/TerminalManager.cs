using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

using TMPro;

public class TerminalManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject cmdEntryPrefab = null;

    [Header("Required components")]
    [SerializeField] private TMP_InputField terminalInput = null;
    [SerializeField] private Transform logHolder = null;

    private List<CmdEntry> activeEntries = null;
    private ObjectPool<CmdEntry> entriesPool = null;

    private Action<string> OnInputCommand = null;

    private void OnEnable()
    {
        GameManager.OnHistoryInput += UpdateInputField;
    }

    private void OnDisable()
    {
        GameManager.OnHistoryInput -= UpdateInputField;
    }

    public void Init(Action<string> onInputCommand)
    {
        OnInputCommand = onInputCommand;

        activeEntries = new List<CmdEntry>();
        entriesPool = new ObjectPool<CmdEntry>(CreateEntry, GetEntry, ReleaseEntry);

        SelectInputField();
    }

    public void AddInterpreterLines(List<string> userInput)
    {
        ClearCmdEntries();
        GenerateCmdEntries(userInput);
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(terminalInput.text))
        {
            SelectInputField();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnInputCommand(terminalInput.text);
            ClearInputField();
            SelectInputField();
        }

        
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

    private void UpdateInputField(string userInput) {
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
