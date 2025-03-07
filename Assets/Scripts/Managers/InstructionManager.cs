using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class InstructionManager
{
    public static string CurrentInstruction { get; set; }
    public static UnityAction onClickOKButton;
}
