using UnityEngine;

[CreateAssetMenu(fileName = "CampfireSprites", menuName = "Game Data/Campfire Sprites")]
public class CampfireSprites : ScriptableObject
{
    [Header("Campfire Stage Sprites")]
    public Sprite stage0_Empty;
    public Sprite stage1_TinderPlaced;
    public Sprite stage2_MaterialsArranged;
    public Sprite stage3_Burning;
    public Sprite stage4_Cooking; 
    public Sprite stage5_MealReady;
}
