using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeFunction : MonoBehaviour
{
    static Dictionary<FunctionID, NarrativeFunction> narrativeFunctionDic = new();
    public static IEnumerator RunNarrativeFunction(FunctionID id, string name)
    {
        if (narrativeFunctionDic.ContainsKey(id))
            yield return narrativeFunctionDic[id].Process(name);

        yield break;
    }

    [SerializeField]
    private FunctionID functionID;
    public FunctionID FunctionID { get { return functionID; } }

    private void Start()
    {
        narrativeFunctionDic.Add(functionID, this);
    }

    private void OnDestroy()
    {
        narrativeFunctionDic.Remove(functionID);
    }

    protected virtual IEnumerator Process(string name)
    {
        yield break;
    }
}
