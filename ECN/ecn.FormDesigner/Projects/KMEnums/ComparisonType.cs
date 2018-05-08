using System;

namespace KMEnums
{
    public enum ComparisonType
    {
        Is,
        IsNot,
        IsNull,
        IsNotNull,
        Contains,
        DoesNotContain,
        StartsWith,
        EndsWith,
        Before,
        After,
        Equals,
        LessThan,
        GreaterThan
    }

    public enum SelectionComparisonType 
    {
        Is = ComparisonType.Is,
        IsNot = ComparisonType.IsNot
    }

    public enum TextComparisonType 
    { 
        Is = ComparisonType.Is,
        IsNot = ComparisonType.IsNot,
        IsNull = ComparisonType.IsNull,
        IsNotNull = ComparisonType.IsNotNull,
        Contains = ComparisonType.Contains,
        DoesNotContain = ComparisonType.DoesNotContain,
        StartsWith = ComparisonType.StartsWith,
        EndsWith = ComparisonType.EndsWith
    }

    public enum DateComparisonType 
    {
        Is = ComparisonType.Is,
        IsNot = ComparisonType.IsNot,
        IsNull = ComparisonType.IsNull,
        IsNotNull = ComparisonType.IsNotNull,
        Before = ComparisonType.Before,
        After = ComparisonType.After
    }

    public enum NumberComparisonType 
    {
        IsNull = ComparisonType.IsNull,
        IsNotNull = ComparisonType.IsNotNull,
        Equals = ComparisonType.Equals,
        LessThan = ComparisonType.LessThan,
        GreaterThan = ComparisonType.GreaterThan
    }

    public enum NewsLetterComparisonType 
    {
        Is = ComparisonType.Is,
        IsNot = ComparisonType.IsNot
    }
}