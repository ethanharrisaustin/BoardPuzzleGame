using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour, IButton3D
{
    public UnityEvent mouseOver, mouseOut, click;

    public void MouseOver()
    {
        mouseOver.Invoke();
    }

    public void MouseOut()
    {
        mouseOut.Invoke();
    }

    public void Click()
    {
        click.Invoke();
    }
}
