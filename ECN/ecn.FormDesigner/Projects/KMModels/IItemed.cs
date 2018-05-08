using System;

namespace KMModels
{
    public interface IItemed
    {
        int? SelectableItemId { get; set; }
        SelectableItem[] SelectableItems { get; set; }
        IItemed GetItem(SelectableItem item);
    }
}