using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomDropdownItem : MonoBehaviour
{
    public RectTransform rectTransform { get { return GetComponent<RectTransform>(); } }

    CustomDropdown dropdownController;
    TMP_Text text;
    string itemText;
    public string ItemText { get { return itemText; } } 

    public void Init(CustomDropdown _dropdownController, string _itemText)
    {
        dropdownController = _dropdownController;
        itemText = _itemText;
        text = GetComponentInChildren<TMP_Text>();
        text.text = _itemText;
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnItemClick);
    }

    void OnItemClick()
    {
        // 자신이 부모의 몇 번째 자식인지 확인
        int myIndex = transform.GetSiblingIndex();

        dropdownController.OnItemSelected(myIndex, itemText);
    }
}
