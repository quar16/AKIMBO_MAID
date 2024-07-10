using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineUI : MonoBehaviour
{
    public Transform upperRow;
    public Transform LowerRow;

    List<RoundUI> roundList = new();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            roundList.Add(upperRow.GetChild(i).GetComponent<RoundUI>());
            roundList.Add(LowerRow.GetChild(i).GetComponent<RoundUI>());
        }
    }

    public void ShellEjection(int index)
    {
        roundList[index].AnimateShellEjectionAsyncall();
    }

    public void Reload(float reloadTime)
    {
        StartCoroutine(ReloadAsync(reloadTime));
    }

    IEnumerator ReloadAsync(float reloadTime)
    {
        for (int i = 1; i <= roundList.Count; i++)
        {
            int index = 20 - i;

            roundList[index].AnimateReloadAsyncall();

            yield return new WaitForSeconds(reloadTime / (roundList.Count - 1));
        }
    }

}
