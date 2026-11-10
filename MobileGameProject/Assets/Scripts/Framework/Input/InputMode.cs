[System.Flags]
public enum InputMode
{
    None = 0,
    Click = 1, 
    DoubleClick = 2,
    Drag = 4, 
    DragRelease = 8, 
    TwoFingerDrag = 16, 
    TwoFingerTap = 32,
    DirectionalButtons = 64,
    ButtonTap = 128,
    PointerMovement = 256
}
