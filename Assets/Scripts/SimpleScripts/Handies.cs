using System;
using UnityEngine;


namespace SimpleScripts
{
    [Serializable]
    public struct MinMax
    {
        public float min, max;

        public float Evaluate(float t)
        {
            return Mathf.Lerp(min, max, t);
        }
    }

    [Serializable]
    public struct MinMax<T>
    {
        public T min, max;
    }

    static public class HandyFuncs
    {
        static public int str_eq_point(string name1, string name2)
        {
            int count = 0;
            int current_2index = 0;

            char[] array = name1.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                // update the current_2index
                for (; current_2index < name2.Length && char.ToLower(name1[i]) != char.ToLower(name2[current_2index]); current_2index++) ; //  ;)
                                                                                                                                           // check for end
                if (current_2index >= name2.Length) current_2index = 0;
                if (char.ToLower(name1[i]) == char.ToLower(name2[current_2index]))
                    count++;
            }

            // Debug.Log($"name1: {name1} , name2: {name2} , : {count}");
            return count;
        }
    }
}
