using Cardboard;

public interface IDragOnto
{
    void OnDragHover(CardboardItemObject item);

    void OnDragUnhover();

    bool OnDropDraggedItem(CardboardItemObject item);

    bool enabled {get; set;}
}
