﻿//Adapted from code at http://stackoverflow.com/a/7120902

public class Tuple<T1, T2>
{
    public T1 Item1 { get; private set; }
    public T2 Item2 { get; private set; }
    internal Tuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}

public static class Tuple
{
    public static Tuple<T1, T2> New<T1, T2>(T1 item1, T2 item2)
    {
        var tuple = new Tuple<T1, T2>(item1, item2);
        return tuple;
    }
}