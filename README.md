# 逃离鸭科夫&绝地潜兵2
Helldiver.cs:Mod主文件

IsGround.cs:挂载给实例化的手雷文件，监听信标球落地

Skill_HelldiverBeaconBall.cs:继承自SkillBase，大部分代码同手雷代码，为信标球专门添加了组件

StratagemInfo.cs:战略配备信息，在Helldiver.cs里实例化创建

StratagemInput.cs:UI面板获取输入，在战略配备被注册到UI面板里时同时被添加

StratagemPanel.cs:创建UI面板时一同添加，RegiStratagemToPanel方法将StratagemInfo里的信息加载到UI里

StratagemReady.cs:信标球呼叫战略配备事件

Unit.cs:工具类，用于调试

