using System;

public class Skill 
{
	public SkillData data { get; private set; }
	public int level { get; private set;  } // level is now just a counter, it has no practical meaning
	private readonly ISkillEffect effect;

	public Skill(SkillData data, ISkillEffect effect)
	{
		this.data = data;
		this.effect = effect;
		level = 1;
	}

	public void Upgrade(SkillData upgradeData)
	{
		data = upgradeData;
		level++;
	}
}
