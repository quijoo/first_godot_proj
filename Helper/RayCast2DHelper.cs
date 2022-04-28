using Godot;
using System;
using System.Collections.Generic;
public static class ExtendRayCast2D{
    //传参进行类的实例化
    /// <summary>
    /// 写在作用域内的情况，不用事先声明 T 类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// 
    static public IEnumerable<T> GetRayCastCollision<T>(this RayCast2D ray) where T : Godot.Object
    {
        if(ray.IsColliding() && ray.CollideWithBodies) 
        {
            yield return (T)ray.GetCollider();
        }
        else yield break;
    }
    /// <summary>
    /// 写在作用域内的情况，不用事先声明 T 类型
    /// </summary>
    /// <returns></returns>
    static public IEnumerable<Godot.Object> GetRayCastCollision(this RayCast2D ray)
    {
        if(ray.IsColliding() && ray.CollideWithBodies) 
        {
            yield return ray.GetCollider();
        }
        else yield break;
    }

    static public bool GetRayCastCollision(this RayCast2D ray, out Godot.Object obj)
    {
        obj = null;
        if(ray.IsColliding() && ray.CollideWithBodies) 
        {
            obj = ray.GetCollider();
            return true;
        }
        return false;
    }
}
