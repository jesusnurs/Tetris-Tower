using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
	private Button targetButton;
	private Vector3 originalScale;

	private void Start()
	{
		targetButton = GetComponent<Button>();
		originalScale = targetButton.transform.localScale;
		targetButton.onClick.AddListener(() => AnimateButton());
	}

	private void AnimateButton()
	{
		Sequence buttonSequence = DOTween.Sequence();

		buttonSequence
			.Append(targetButton.transform.DOScale(originalScale * 0.9f, 0.1f).SetEase(Ease.InQuad))
			.Append(targetButton.transform.DOScale(originalScale * 1.1f, 0.15f).SetEase(Ease.OutQuad))
			.Append(targetButton.transform.DOScale(originalScale * 1.0f, 0.1f).SetEase(Ease.OutBack));

		buttonSequence.Play();
	}
}