using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridEditor : MonoBehaviour {

    private int counter = 0;
    public GameObject gridThatStoresTheItems;
    public Text itemPrefab;

    public void AddItemToGrid()
    {
        counter++;
        var newText = Instantiate(itemPrefab) as Text;
        //newText.text = string.Format("Item {0}", counter.ToString());
        newText.transform.parent = gridThatStoresTheItems.transform;
    }
}
