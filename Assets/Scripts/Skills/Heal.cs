using UnityEngine;

[System.Serializable]
public class HealingSkill : ISkill
{
    public string SkillName => "Healing";
    public Sprite Icon { get; set; }
    public int ManaCost { get; set; } = 15;
    public float Cooldown { get; set; } = 3f;
    public bool IsUnlocked { get; set; } = false;

    public HealingSkill(Sprite icon)
    {
        Icon = icon;
    }

    public void UseSkill(Transform playerTransform)
    {
        Debug.Log("Healing used! Player HP restored.");
        // Thêm logic hồi máu tại đây
    }
}
