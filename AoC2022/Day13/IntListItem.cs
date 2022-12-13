namespace AoC2022.Day13;

public record struct IntListItem(int Value) : IListItem
{
    public int CompareTo(IListItem? other)
    {
        if (other is null)
            return 1;

        if (other is IntListItem otherValue)
        {
            return Value < otherValue.Value
                ? -1
                : Value > otherValue.Value
                    ? 1
                    : 0;
        }

        return new ListItemList { this }.CompareTo(other);
    }

    public override string ToString() => Value.ToString();
}
