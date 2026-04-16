using System;

public class SkillRequirements
{
	public SkillTargetType SkillTargetType { get; }

	public SkillID SkillID { get; }

	public int TargetCount { get; }

	public bool NeedPosition { get; }

	public SkillRequirements(SkillData data)
	{
		SkillTargetType = data.skillTargetType;
		SkillID = data.skillID;

		switch (SkillTargetType)
		{
			case SkillTargetType.TARGET_TYPE_SINGLE:
				TargetCount = 1;
				break;
			case SkillTargetType.TARGET_TYPE_MULTIPLE_POINT_AND_CLICK:
				TargetCount = data.numberOfTargets;
				break;
		}

		switch (SkillID)
		{
			case SkillID.WIND_TELEPORTATION:
				NeedPosition = true; 
				break;
			case SkillID.FIRE_FIREBALL:
				NeedPosition = false;
				break;
		}
	}
}
