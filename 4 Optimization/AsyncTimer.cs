using System;
using System.Threading.Tasks;
using UnityEngine;

namespace WOE
{
    /// <summary>
    /// Use tasks for update
    /// </summary>
    public static class AsyncTimer
    {
        public static async void Update(float delay, Func<int, bool> CheckCancellation, Action<int> OnUpdated = null, Action OnEnded = null)
        {
            if (delay <= 0)
                return;

            int delayInMilliseconds = ConvertToMilliseconds(delay);
            int counter = 0;

            while (true)
            {
                if (counter == int.MaxValue)
                    Debug.LogWarning("Update reached max int value");
                if (CheckCancellation.Invoke(counter) || counter == int.MaxValue)
                    break;

                OnUpdated?.Invoke(counter);

                await Task.Delay(delayInMilliseconds);

                counter++;
            }

            OnEnded?.Invoke();
        }

        public static void Update(float delay, Func<bool> CheckCancellation, Action OnUpdated = null)
        {
            Update(delay, (u) => CheckCancellation.Invoke(), (u) => OnUpdated?.Invoke());
        }

        public static async Task DelayWithUpdate(float time, int updatesCount = 1, Action<int> OnUpdated = null, Func<bool> CheckCancellation = null)
        {
            Update(time / updatesCount, (u) => updatesCount - u < 0 || (CheckCancellation != null && CheckCancellation.Invoke()), OnUpdated);
            await Delay(time);
        }

        public static async Task Delay(float time)
        {
            if (time <= 0)
                return;

            int timeInMilliseconds = ConvertToMilliseconds(time);
            await Task.Delay(timeInMilliseconds);
        }

        private static int ConvertToMilliseconds(float value) => Mathf.RoundToInt(value * 1000);
    }
}
