using System;
using System.Collections.Generic;
public class CircleQueue<T> where T : class
{
    private T[] data;
    private int front = 0, rear = 0; 
    private int MaxSize = 0;
    public int Count 
    {
        get => (rear - front + MaxSize) % MaxSize;
    }
    public bool Full { get => Count == MaxSize; }

    public bool Empty { get => Count == 0; }
    public CircleQueue(int size)
    {
        data = new T[size];
        MaxSize = size;
    }
    public void Enqueue(T node)
    {
        if((rear + 1) % MaxSize == front)
        {
            // 队列满
            return;
        }
        data[rear] = node;
        rear = (rear + 1) % MaxSize;
    }
    public T Dequeue()
    {
        if(rear == front)
        {
            // 队列空
            return null; 
        } 
        T res = data[front];
        front = (front + 1) % MaxSize;
        return res;
    }
    public T Peek()
    {
        if(front != rear) return data[front];
        return null;
    }
}