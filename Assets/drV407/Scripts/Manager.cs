using UnityEngine.UI;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Text thickness;

    public Text frying;

    public Text cooking;

    public Dropdown dropdown;

    public Recepie_data[] recepie_Datas;

    private void Start()
    {
        UpdateRecepieData();

        dropdown.onValueChanged.AddListener((value) =>
        {
            UpdateRecepieData();
        });
    }

    void UpdateRecepieData()
    {
        int i = dropdown.value;

        thickness.text = recepie_Datas[i].thickness.ToString();

        frying.text = recepie_Datas[i].frying.ToString();

        cooking.text = recepie_Datas[i].cooking.ToString();
    }
}
