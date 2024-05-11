using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts
{
    public static class GameUtils
    {
        private static readonly int ColorField = Shader.PropertyToID("_Color");
        
        public static void ColorObject(Renderer r, HexColor color)
        {
            var proBlock = new MaterialPropertyBlock();
            proBlock.SetColor(ColorField, GetConvertColor(color));
            r.SetPropertyBlock(proBlock);
            Color GetConvertColor(HexColor c)
            {
                return c switch
                {
                    HexColor.Blue  => Color.blue,
                    HexColor.Green => Color.green,
                    HexColor.Red   => Color.red,
                    HexColor.Pink  => Color.magenta,
                    _              => Color.white,
                };
            }
        }

        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
        
        public static Coroutine DelayExecute(this MonoBehaviour monoBehavior, Action functionWantDelay,
            float? timeDelay = null)
        {
            return monoBehavior.StartCoroutine(IEWait(functionWantDelay, timeDelay));
        }
        
        private static IEnumerator IEWait(Action functionWantDelay, float? timeDelay)
        {
            if (timeDelay.HasValue)
                yield return GetWait(timeDelay.Value);
            else
                yield return null;
            functionWantDelay?.Invoke();
        }
        
        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time,out var wait))
            {
                return wait;
            }

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time]; 
        }
    }
}