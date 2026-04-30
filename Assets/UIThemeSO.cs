using UnityEngine;

[CreateAssetMenu(fileName = "UITheme", menuName = "UI/Infernal Theme", order = 1)]
public class UIThemeSO : ScriptableObject
{
    public Color bloodRed = new Color(1f, 0f, 0f, 1f); // #FF0000
    public Color voidBlack = new Color(0f, 0f, 0f, 1f); // #000000
    public Color accentOrange = new Color(1f, 0.5f, 0f, 1f);
}
