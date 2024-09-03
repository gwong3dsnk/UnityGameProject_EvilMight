using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownCustomAttribute : PropertyAttribute
{
    public string[] options;

    public void DynamicDropdownAttribute(params string[] options)
    {
        this.options = options;
    }
}