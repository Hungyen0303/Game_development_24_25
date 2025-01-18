using UnityEngine;

public interface ISkill
{
    string SkillName { get; }
    Sprite Icon { get; }
    int ManaCost { get; }
    float Cooldown { get; }
    bool IsUnlocked { get; set; }

    void UseSkill(Transform playerTransform);
}
