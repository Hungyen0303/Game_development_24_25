using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public ISkill[] skills; // Danh sách kỹ năng

    public void UseSkill(int index, Transform playerTransform)
    {
        if (index < 0 || index >= skills.Length)
        {
            Debug.LogWarning("Invalid skill index");
            return;
        }

        ISkill skill = skills[index];

        if (!skill.IsUnlocked)
        {
            Debug.LogWarning($"Skill {skill.SkillName} is locked!");
            return;
        }

        skill.UseSkill(playerTransform);
    }
}
