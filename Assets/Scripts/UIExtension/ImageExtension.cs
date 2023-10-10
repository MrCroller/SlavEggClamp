using UnityEngine;
using UnityEngine.UI;
using UnityEngineTimers;

namespace TimersSystemUnity.Extension
{
    public static partial class Extension
    {
        public static Color SetAlpha(this Image image, float value)
        {
            Color color = image.color;
            color.a = Mathf.Lerp(0.0f, 1.0f, value);
            return image.color = color;
        }

        public static Color SetAlpha(this SpriteRenderer sprite, float value)
        {
            Color color = sprite.color;
            color.a = Mathf.Lerp(0.0f, 1.0f, value);
            return sprite.color = color;
        }

        public static void SetAplhaDynamic(this Image image, float time, AnimationCurve easing, bool isChangeActive = true)
        {
            if (isChangeActive)
            {
                image.gameObject.SetActive(true);
            }

            TimersPool.GetInstance().StartTimer(EndMethod, GreatSelect, time);
            void GreatSelect(float progress)
            {
                image.SetAlpha(easing.Evaluate(progress));
            }

            void EndMethod()
            {
                if (isChangeActive)
                {
                    image.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// The appearance of the picture for a while and the subsequent disappearance
        /// </summary>
        /// <param name="image"></param>
        /// <param name="timeToVisable">Appearance time</param>
        /// <param name="timeVisible">Picture show time</param>
        /// <param name="timeToInvisable">time of disappearance</param>
        /// <param name="isChangeActive">Whether to change the activity of the object?</param>
        public static void AnimationAphaDynamic(this Image image,
                                             float timeToVisable,
                                             float timeVisible,
                                             float timeToInvisable,
                                             bool isChangeActive = true)
        {
            if (isChangeActive)
            {
                image.gameObject.SetActive(true);
            }

            image.SetAlpha(0.0f);

            // To Visable
            TimersPool.GetInstance().StartTimer(Wait, (float progress) => image.SetAlpha(progress), timeToVisable);

            void Wait()
            {
                TimersPool.GetInstance().StartTimer(ToInvisible, timeVisible);
            }

            void ToInvisible()
            {
                TimersPool.GetInstance().StartTimer(EndMethod, (float progress) =>
                {
                    image.SetAlpha(1.0f - progress);
                }, timeToInvisable);
            }

            void EndMethod()
            {
                image.SetAlpha(0.0f);

                if (isChangeActive)
                {
                    image.gameObject.SetActive(false);
                }
            }
        }
    }
}
