using UnityEngine;

namespace ExtensionMethods
{
    public static class RandomExtension
    {
        public static Vector2 RandomOnCircle(this float value)
        {
            float randAng = UnityEngine.Random.Range(0, Mathf.PI * 2);
            return new Vector2(Mathf.Cos(randAng) * value, Mathf.Sin(randAng) * value);
        }
    }
}