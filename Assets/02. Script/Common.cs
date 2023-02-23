/// <summary>
/// 공통으로 사용하는 문자열이나 enum 타입들을 모아놓는 스크립트
/// </summary>

#region 애니메이션 파라미터
public class AnimationParameters
{
    public const string WALK = "Walk";
    // ...
}
#endregion

#region 문자열 프로퍼티
public class StringPropertys
{
    public const string FILEPATH = "Resources";
    // ...
}
#endregion

#region Enums
public enum E_SoundType
{
    NONE,
}

public enum E_UIEvent
{
    Click,
    Drag,
    Enter,
    Exit,
    Down,
    Up
}

public enum E_State
{
    Success,
    Failure,
    Running,
}

// ...
#endregion
