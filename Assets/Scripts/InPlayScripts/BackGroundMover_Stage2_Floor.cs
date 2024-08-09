using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BlockState { None, LeftBroken, RightBroken, LeftNormal, RightNormal }

public class BackGroundMover_Stage2_Floor : BackGroundMover
{
    Dictionary<int, BlockState> stateIndexDic = new();

    public Sprite normal;
    public Sprite leftNormal;
    public Sprite rightNormal;
    public Sprite leftBroken;
    public Sprite rightBroken;

    public override void AddRemoveIndex(int index)
    {
        if (stateIndexDic.ContainsKey(index) == false)
            stateIndexDic.Add(index, BlockState.None);
        else if (stateIndexDic[index] != BlockState.None)
            stateIndexDic[index] = BlockState.None;

        if (stateIndexDic.ContainsKey(index - 1) == false)
            stateIndexDic.Add(index - 1, BlockState.LeftNormal);
        if (stateIndexDic.ContainsKey(index + 1) == false)
            stateIndexDic.Add(index + 1, BlockState.RightNormal);

        SetBlockState(index);
        SetBlockState(index - 1);
        SetBlockState(index + 1);
    }

    public void AddBrokenIndex(int index)
    {
        if (stateIndexDic.ContainsKey(index) == false)
            stateIndexDic.Add(index, BlockState.None);
        else if (stateIndexDic[index] != BlockState.None)
            stateIndexDic[index] = BlockState.None;

        if (stateIndexDic.ContainsKey(index - 1) == false)
            stateIndexDic.Add(index - 1, BlockState.LeftBroken);
        if (stateIndexDic.ContainsKey(index + 1) == false)
            stateIndexDic.Add(index + 1, BlockState.RightBroken);

        SetBlockState(index);
        SetBlockState(index - 1);
        SetBlockState(index + 1);
    }

    public override void BackGroundUpdate(BackGroundBlock backGroundBlock, int toIndex)
    {
        backGroundBlock.block.transform.localPosition = bgLength * toIndex * Vector3.right;
        backGroundBlock.index = toIndex;

        SetBlockState(toIndex);
    }

    void SetBlockState(int index)
    {
        var backGroundBlock = backGroundList.Find(x => x.index == index);

        if (backGroundBlock == null)
            return;

        backGroundBlock.block.SetActive(true);

        if (stateIndexDic.ContainsKey(index))
        {
            switch (stateIndexDic[index])
            {
                case BlockState.None:
                    backGroundBlock.block.SetActive(false);
                    break;
                case BlockState.LeftNormal:
                    backGroundBlock.block.GetComponentInChildren<SpriteRenderer>().sprite = leftNormal;
                    break;
                case BlockState.RightNormal:
                    backGroundBlock.block.GetComponentInChildren<SpriteRenderer>().sprite = rightNormal;
                    break;
                case BlockState.LeftBroken:
                    backGroundBlock.block.GetComponentInChildren<SpriteRenderer>().sprite = leftBroken;
                    break;
                case BlockState.RightBroken:
                    backGroundBlock.block.GetComponentInChildren<SpriteRenderer>().sprite = rightBroken;
                    break;
            }
        }
        else
        {
            backGroundBlock.block.GetComponentInChildren<SpriteRenderer>().sprite = normal;
        }
    }

}
