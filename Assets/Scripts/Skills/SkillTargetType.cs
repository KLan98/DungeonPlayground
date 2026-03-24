using System;

public enum SkillTargetType
{
	TARGET_TYPE_SINGLE, // single target
	TARGET_TYPE_MULTIPLE_POINT_AND_CLICK, // multiple targets, point and click
	TARGET_TYPE_MULTIPLE_AOE_CIRCLE, // MOBA circle AOE effects
	TARGET_TYPE_ARROW, // MOBA arrow 
	TARGET_TYPE_MULTIPLE_AOE_FAN, // Fan-shaped AOE effects
	TARGET_TYPE_AOE_SELF, // AOE circle, applied to self
}