using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomDropdown : MonoBehaviour
{
    public Button dropdownButton;
    public TMP_Text dropdownText;
    public CustomDropdownItem dropdownItemPrefab;
    public Scrollbar scrollbar;

    public RectTransform dropdownMask;
    public RectTransform dropdownContent;

    public float maxItemListRatio;

    public float dropDownMoveHeight;

    int value = 0;
    public int Value { get { return value; } }

    public string GetItemByIndex(int index)
    {
        return dropdownContent.GetChild(index).GetComponent<CustomDropdownItem>().ItemText;
    }
    public string GetNowItem()
    {
        return dropdownContent.GetChild(Value).GetComponent<CustomDropdownItem>().ItemText;
    }

    public bool IsItemExist(string name)
    {
        foreach (Transform v in dropdownContent.transform)
        {
            if (v.GetComponent<CustomDropdownItem>().ItemText == name)
                return true;
        }
        return false;
    }

    private void RefreshShownValue()
    {
        value = 0;

        dropdownContent.anchoredPosition = Vector3.zero;

        if (dropdownContent.childCount > maxItemListRatio)
        {
            scrollbar.gameObject.SetActive(true);
            scrollbar.value = 0;
            scrollbar.size = maxItemListRatio / dropdownContent.childCount;

            dropDownMoveHeight = (dropdownContent.childCount - maxItemListRatio) * dropdownButton.image.rectTransform.sizeDelta.y;
        }
        else
        {
            scrollbar.gameObject.SetActive(false);
        }

        if (dropdownContent.childCount != 0)
            dropdownText.text = GetItemByIndex(0);
    }

    public void MoveDropdownContent()
    {
        dropdownContent.anchoredPosition = Vector2.up * dropDownMoveHeight * scrollbar.value;
    }

    void Start()
    {
        dropdownButton.onClick.AddListener(ToggleDropdown);

        float itemX = dropdownButton.image.rectTransform.sizeDelta.x;
        float itemY = dropdownButton.image.rectTransform.sizeDelta.y;

        dropdownMask.gameObject.SetActive(false); // 처음에는 드롭다운 리스트를 숨김
        dropdownMask.sizeDelta = new Vector2(0, itemY * maxItemListRatio);

        dropdownItemPrefab.rectTransform.sizeDelta = new Vector2(itemX, itemY);
    }

    private void Update()
    {
        if (scrollbar.gameObject.activeSelf)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                scrollbar.value = Mathf.Clamp01(scrollbar.value - Input.mouseScrollDelta.y * 0.1f);
            }
        }
    }

    void ToggleDropdown()
    {
        dropdownMask.gameObject.SetActive(!dropdownMask.gameObject.activeSelf);
    }

    public void OnItemSelected(int index, string item)
    {
        value = index;
        dropdownText.text = item;

        ToggleDropdown(); // 아이템 선택 후 드롭다운을 닫음
    }

    public void AddItem(string itemText)
    {
        if (IsItemExist(itemText)) return;

        CustomDropdownItem newItem = Instantiate(dropdownItemPrefab, dropdownContent);

        newItem.gameObject.SetActive(true);
        newItem.Init(this, itemText);

        RefreshShownValue();
    }
}
