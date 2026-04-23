using System;

public class SkillRequirements
{
	private SkillTargetType skillTargetType;
	public SkillTargetType SkillTargetType 
	{ 
		get
		{
			return skillTargetType;	
		}
	}

	private SkillID skillID;
	public SkillID SkillID 
	{ 
		get
		{
			return skillID;
		}
	}

	private int targetCount;
	public int TargetCount 
	{ 
		get
		{
			return targetCount;
		}
	}

	private bool hasImpactPattern;
	public bool HasImpactPattern
	{
		get
		{ 
			return hasImpactPattern;
		}
	}

	private bool needDestination; // If this skill needs a destination in addition to the target to perform
	public bool NeedDestination 
	{ 
		get
		{
			return needDestination;
		}
	}

	public SkillRequirements(SkillData data)
	{
		skillTargetType = data.SkillTargetType;
		skillID = data.SkillID;

		switch (SkillTargetType)
		{
			case SkillTargetType.TARGET_TYPE_SINGLE_ENEMY:
				targetCount = 1;
				break;
			case SkillTargetType.TARGET_TYPE_MULTIPLE_ENEMIES:
				targetCount = data.NumberOfTargets;
				break;
			case SkillTargetType.TARGET_TYPE_SINGLE_WALKABLE_TILE:
				targetCount = 0;
				break;
		}

		switch (SkillID)
		{
			case SkillID.WIND_TELEPORTATION:
				needDestination = true; 
				hasImpactPattern = true;
				break;
			case SkillID.FIRE_FIREBALL:
				needDestination = false;
				hasImpactPattern = true;	
				break;
			case SkillID.BOMB:
				needDestination = false;
				hasImpactPattern = true;
				break;
		}
	}
}