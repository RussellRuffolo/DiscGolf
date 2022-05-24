using System;

public class BasketScoreEventArgs : EventArgs
{
    public int BasketIndex { get; set; }

    public BasketScoreEventArgs(int basketIndex)
    {
        BasketIndex = basketIndex;
    }
}

public class DiscThrowEventArgs : EventArgs
{
    public DiscController Disc { get; set; }

    public DiscThrowEventArgs(DiscController disc)
    {
        Disc = disc;
    }
}