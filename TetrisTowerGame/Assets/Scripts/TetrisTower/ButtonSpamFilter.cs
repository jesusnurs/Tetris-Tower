using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSpamFilter : MonoBehaviour
{
    [SerializeField] private float spamFilterTime = 0.5f;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(FilterButtonSpam);
    }


    private void FilterButtonSpam()
    {
        FilterButtonSpamAsync().Forget();
    }

    private async UniTaskVoid FilterButtonSpamAsync()
    {
        button.interactable = false;
        await UniTask.Delay(TimeSpan.FromSeconds(spamFilterTime));
        if (button)
            button.interactable = true;
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(FilterButtonSpam);
    }
}