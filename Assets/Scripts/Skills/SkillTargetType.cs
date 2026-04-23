using System;

public enum SkillTargetType
{
	TARGET_TYPE_SINGLE_ENEMY, // single target
	TARGET_TYPE_MULTIPLE_ENEMIES, // multiple targets, point and click
	TARGET_TYPE_SINGLE_WALKABLE_TILE,
	TARGET_TYPE_MULTIPLE_AOE,
	TARGET_TYPE_AOE_SELF, // AOE circle, applied to self
	TARGET_TYPE_SELF, // target is self
}