namespace AoC2022.Day13;

public class ListItemList : List<IListItem>, IListItem
{
    public int CompareTo(IListItem? other)
    {
        if (other is null)
            return 1;

        if (other is not ListItemList otherList)
            otherList = new ListItemList() { other };

        for (var i = 0; i < Math.Min(Count, otherList.Count); i++)
        {
            var result = this[i].CompareTo(otherList[i]);

            if (result != 0)
                return result;
        }

        return Count < otherList.Count
            ? -1
            : Count > otherList.Count
                ? 1
                : 0;
    }

    public override string ToString() => $"[{string.Join<IListItem>(',', this.ToArray()[0..Count])}]";
}