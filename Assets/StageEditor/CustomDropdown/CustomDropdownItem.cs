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
        // �ڽ��� �θ��� �� ��° �ڽ����� Ȯ��
        int myIndex = transform.GetSiblingIndex();

        dropdownController.OnItemSelected(myIndex, itemText);
    }
}
