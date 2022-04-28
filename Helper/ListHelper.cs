using Godot;
using System.Collections.Generic;
static public class ExtendList
{
    static public bool All<T>(this List<T> list, T other)
    {
        foreach(T obj in list)
        {
            if(!obj.Equals(other))
            {
                return false;
            }
        }
        return true;
    }

    static public bool Any<T>(this List<T> list, T other)
    {
        foreach(T obj in list)
        {
            if(obj.Equals(other))
            {
                return true;
            }
        }
        return false;
    }
}
