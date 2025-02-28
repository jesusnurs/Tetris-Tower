using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Action OnGameStarted;
    public Action OnGameRestarted;
    public Action OnGameEnded;
    public Action OnGameResumed;
    public Action OnGamePaused;
    public Action OnMainMenuOpened;

    private void Start()
    {
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        OnMainMenuOpened?.Invoke();
    }
    
    public void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    public void RestartGame()
    {
        OnGameRestarted?.Invoke();
    }

    public void EndGame()
    {
        OnGameEnded?.Invoke();
    }
    
    public void ResumeGame()
    {
        OnGameResumed?.Invoke();
    }
    
    public void PauseGame()
    {
        OnGamePaused?.Invoke();
    }
}
