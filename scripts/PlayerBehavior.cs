using System;
using UnityEngine;


public class PlayerBehavior : MonoBehaviour
{
    public static PlayerBehavior Instance {get; private set; }
    public event EventHandler<OnSelectedCounterChangeArgs> OnSelectedCounterChange;
    public class OnSelectedCounterChangeArgs: EventArgs{
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;

    private void Awake(){
        if(Instance!=null)
            Debug.Log("There is more than one player instance.");
        Instance = this;
    }

    private void Start(){
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e){
        if(selectedCounter!=null)
            selectedCounter.Interact();
    }

    private void Update(){
        Movements();
        Interactions();
    }

    public bool IsWalking(){
        return isWalking;
    }

    private void Interactions(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

        if(moveDir!=Vector3.zero) lastInteractDir = moveDir;

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)){
                if(clearCounter!=selectedCounter)
                    SetSelectedCounter(clearCounter);
            }
            else
                SetSelectedCounter(null);
        else
            SetSelectedCounter(null);
    }

    private void Movements(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        isWalking = inputVector != Vector2.zero;

        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if(!canMove){
            Vector3 moveDirX = new(moveDir.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if(canMove) moveDir = moveDirX;
            else{
                Vector3 moveDirZ = new(0f, 0f, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if(canMove) moveDir = moveDirZ;
            }
        }
        if(canMove) transform.position += moveDistance * moveDir;
    }

    private void SetSelectedCounter(ClearCounter selectedCounter){
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeArgs{
            selectedCounter = selectedCounter
        });
    }
}
