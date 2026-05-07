public interface IDragOnto
{
    void OnDragHover(CardboardItemObject item);

    void OnDragUnhover(CardboardItemObject item);

    bool OnDropDraggedItem(CardboardItemObject item);

    bool enabled {get; set;}
}
