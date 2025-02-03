using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public string foodName; // Initial name of the bun

    public void ChangeFoodName(string newName)
    {
        foodName = newName;
        Debug.Log("Food name changed to: " + foodName);
    }
}
