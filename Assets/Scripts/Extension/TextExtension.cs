using TMPro;
using UnityEngine;
using UnityEngineTimers;

namespace TimersSystemUnity.Extension
{
    public static partial class Extension
    {
        public static Color SetAlpha(this TMP_Text text, float value)
        {
            Color color = text.color;
            color.a = Mathf.Lerp(0.0f, 1.0f, value);
            return text.color = color;
        }

        public static void SetAplhaDynamic(this TMP_Text text, float time, AnimationCurve easing, bool isChangeActive = true)
        {
            if (isChangeActive)
            {
                text.gameObject.SetActive(true);
            }

            TimersPool.GetInstance().StartTimer(EndMethod, GreatSelect, time);
            void GreatSelect(float progress)
            {
                text.SetAlpha(easing.Evaluate(progress));
            }

            void EndMethod()
            {
                if (isChangeActive)
                {
                    text.gameObject.SetActive(false);
                }
            }
        }

        public static void AnimationAphaDynamic(this TMP_Text text,
                                             float timeToVisable,
                                             float timeVisible,
                                             float timeToInvisable,
                                             bool isChangeActive = true)
        {
            if (isChangeActive)
            {
                text.gameObject.SetActive(true);
            }

            text.SetAlpha(0.0f);

            // To Visable
            TimersPool.GetInstance().StartTimer(Wait, (float progress) => text.SetAlpha(progress), timeToVisable);

            void Wait()
            {
                TimersPool.GetInstance().StartTimer(ToInvisible, timeVisible);
            }

            void ToInvisible()
            {
                TimersPool.GetInstance().StartTimer(EndMethod, (float progress) =>
                {
                    text.SetAlpha(1.0f - progress);
                }, timeToInvisable);
            }

            void EndMethod()
            {
                text.SetAlpha(0.0f);

                if (isChangeActive)
                {
                    text.gameObject.SetActive(false);
                }
            }
        }

        public static void SetTransformYDynamic(this TMP_Text text, float time, AnimationCurve easing, bool isChangeActive = true)
        {
            if (isChangeActive)
            {
                text.gameObject.SetActive(true);
            }

            TimersPool.GetInstance().StartTimer(EndMethod, TranslateUp, time);

            void TranslateUp(float progress)
            {
                text.rectTransform.localPosition = new Vector2(text.rectTransform.localPosition.x, easing.Evaluate(progress));
            }

            void EndMethod()
            {
                if (isChangeActive)
                {
                    text.gameObject.SetActive(false);
                }
            }
        }
    }
}
