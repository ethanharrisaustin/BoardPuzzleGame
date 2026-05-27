using UnityEngine;
using UnityEngine.UI;

public interface IItem
{
    Image[] GetImages();
    Transform transform { get; }
    string unique_id {get;}       
}