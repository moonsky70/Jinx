﻿namespace JinxMod.Modules.Achievements
{
    internal class MasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementSpriteName => "texMasteryAchievement";
        public override string AchievementTokenPrefix => JinxPlugin.DEVELOPER_PREFIX + "_JINX_BODY_MASTERY";
        public override string PrerequisiteUnlockableIdentifier => JinxPlugin.DEVELOPER_PREFIX + "_JINX_BODY_UNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => "JinxBody";

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}