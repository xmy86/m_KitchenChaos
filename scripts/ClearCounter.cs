using UnityEngine;
using UnityEngine.PlayerLoop;

public class ClearCounter : MonoBehaviour
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KichenObject kitchenObject;

    public void Interact(){
        if(kitchenObject==null){
            Transform kichenObjectTransform = Instantiate(kitchenObjectSO.prefabs, counterTopPoint);
            kichenObjectTransform.localPosition = Vector3.zero;

            kitchenObject = kichenObjectTransform.GetComponent<KichenObject>();
            kitchenObject.SetClearCounter(this);
        }
        else
            Debug.Log(kitchenObject.GetClearCounter());
    }
}
