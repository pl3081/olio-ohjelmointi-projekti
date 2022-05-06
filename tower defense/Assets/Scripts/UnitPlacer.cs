using UnityEngine;

public class UnitPlacer : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    
    public GameObject Preview { get; private set; }
    Player _player;
    Camera _cam;
    [SerializeField] UnitControls unitControls;
    Player.UnitContainer _selectedContainer;
    
    void Awake()
    {
        _cam = GetComponent<Camera>();
        unitControls = GetComponent<UnitControls>();
        _player = Player.Instance;
    }

    public void SetTarget(GameObject target)
    {
        if (Preview)
        {
            Destroy(Preview);
        }
        Player.UnitContainer targetContainer = _player.FindContainer(target.name);
        if (targetContainer.amount <= 0) return;
        
        GameObject newPreview = Instantiate(target, Vector3.zero, Quaternion.identity);
        newPreview.GetComponent<Unit>().enabled = false;
        Preview = newPreview;
        _selectedContainer = targetContainer;
    }

    public void Clear()
    {
        Destroy(Preview);
        Clean();
    }

    void Clean()
    {
        Preview = null;
        _selectedContainer = null;
    }

    void Update()
    {
        if (!Preview) return;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = _cam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit rayHit, float.MaxValue, targetLayer))
        {
            Vector3 point = rayHit.point;
            Preview.transform.position = new Vector3(point.x, point.y + Preview.transform.localScale.y/2, point.z);
        }

        if (Input.GetKeyDown(KeyCode.Return)) //enter is return
        {
            print(_selectedContainer);
            if (!_player.RemoveUnit(_selectedContainer))
            {
                return;
            }

            Unit newUnit = Preview.GetComponent<Unit>();
            newUnit.enabled = true;
            Clean();
            print(Preview);
            unitControls.AddUnit(newUnit);
        }
    }
}