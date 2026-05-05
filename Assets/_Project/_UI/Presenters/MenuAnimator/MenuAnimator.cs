using DG.Tweening;
using SpaceshipBattle.Core;
using TMPro;
using UnityEngine;

namespace SpaceshipBattle.Presenters
{
    public class MenuAnimator : MonoBehaviour
    {
        [Header("Title")]
        [SerializeField] TextMeshProUGUI _titleText;

        [Header("Buttons")]
        [SerializeField] RectTransform _buttonStart;
        [SerializeField] RectTransform _buttonScores;

        [Header("Canvas")]
        [SerializeField] CanvasGroup _canvasGroup;

        private void OnEnable()  => PlayIntro();
        private void OnDisable() => DOTween.Kill(gameObject);

        private void PlayIntro()
        {
            _canvasGroup.alpha = 0f;

            if (_titleText != null)
            {
                _titleText.alpha  = 0f;
                _titleText.transform.localScale = Vector3.one * GameConstants.Animation.TitleInitialScale;
            }

            if (_buttonStart != null)
                _buttonStart.anchoredPosition  += Vector2.down * GameConstants.Animation.ButtonSlideOffset;

            if (_buttonScores != null)
                _buttonScores.anchoredPosition += Vector2.down * GameConstants.Animation.ButtonSlideOffset;

            Sequence seq = DOTween.Sequence().SetId(gameObject);

            seq.Append(
                _canvasGroup
                    .DOFade(1f,GameConstants.Animation.MenuFadeInDuration).SetEase(Ease.InQuad));

            if (_titleText != null)
            {
                seq.Insert(GameConstants.Animation.TitleFadeInDelay,
                    _titleText
                        .DOFade(1f, GameConstants.Animation.TitleFadeInDuration)
                        .SetEase(Ease.OutQuad));

                seq.Insert(GameConstants.Animation.TitleFadeInDelay,
                    _titleText.transform
                        .DOScale(1f, GameConstants.Animation.TitleFadeInDuration)
                        .SetEase(Ease.OutBack));

                seq.AppendCallback(() =>
                    _titleText.transform
                        .DOScale(GameConstants.Animation.TitlePulseScale,
                                 GameConstants.Animation.TitlePulseDuration)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine)
                        .SetId(gameObject));
            }

            if (_buttonStart != null)
            {
                seq.Insert(GameConstants.Animation.ButtonStartDelay,
                    _buttonStart
                        .DOAnchorPosY(
                            _buttonStart.anchoredPosition.y + GameConstants.Animation.ButtonSlideOffset,
                            GameConstants.Animation.ButtonSlideDuration)
                        .SetEase(Ease.OutBack));
            }

            if (_buttonScores != null)
            {
                seq.Insert(GameConstants.Animation.ButtonScoresDelay,
                    _buttonScores
                        .DOAnchorPosY(
                            _buttonScores.anchoredPosition.y + GameConstants.Animation.ButtonSlideOffset,
                            GameConstants.Animation.ButtonSlideDuration)
                        .SetEase(Ease.OutBack));
            }
        }
    }
}