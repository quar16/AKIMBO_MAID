using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeFunction : MonoBehaviour
{
    static Dictionary<FunctionID, NarrativeFunction> narrativeFunctionDic = new();
    public static IEnumerator RunNarrativeFunction(FunctionID id)
    {
        if (narrativeFunctionDic.ContainsKey(id))
            yield return narrativeFunctionDic[id].Process();

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

    protected virtual IEnumerator Process()
    {
        yield break;
    }
}
