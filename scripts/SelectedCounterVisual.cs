using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] ClearCounter clearCounter;
    [SerializeField] GameObject visualGameObject;

    private void Start(){
        PlayerBehavior.Instance.OnSelectedCounterChange += Instance_OnSelectedCounterChange;
    }

    private void Instance_OnSelectedCounterChange(object sender, PlayerBehavior.OnSelectedCounterChangeArgs e){
        if(e.selectedCounter==clearCounter)
            show();
        else
            hide();
    }

    private void show(){
        visualGameObject.SetActive(true);
    }

    private void hide(){
        visualGameObject.SetActive(false);
    }
}
