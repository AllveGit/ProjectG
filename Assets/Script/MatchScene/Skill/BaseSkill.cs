using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// BaseSkill의 변수 모음
//
public partial class BaseSkill
{
    protected BasePlayer ownerPlayer = null;
    protected string skillName = null;
    protected string toolTip = null;
}

//
// BaseSkill의 프로퍼티
//
public partial class BaseSkill
{
    public BasePlayer OwnerPlayer { get => ownerPlayer; }
    public string SkillName { get => skillName; }
    public string ToolTip { get => toolTip; }
}



public abstract partial class BaseSkill : MonoBehaviour
{
    public abstract void CastSkill();
}
