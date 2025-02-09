using UnityEngine;

public class ProductLoader : MonoBehaviour
{
    public int products=1;
    
    public void addProduct(int amt)
    {
        products += amt;
    }
    public int LoadProducts()
    {
        return products;
    }
}
