using System.Collections.Generic;
using UnityEngine;

public class UnitPlacer : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] UnitControls unitControls;
    Player player;
    Camera cam;

    public class UnitSelection
    {
        public GameObject Preview { get; private set; }
        public Player.UnitContainer Container { get; private set; }

        public void Set(GameObject target)
        {
            if (Preview)
            {
                Destroy(Preview);
            }
            
            Container = Player.Instance.FindContainer(target.name);
            Preview = Instantiate(target, Vector3.zero, Quaternion.identity);
            Preview.GetComponent<Unit>().enabled = false;
        }
        
        public void Clear()
        {
            Destroy(Preview);
            Clean();
        }

        public void Clean()
        {
            Preview = null;
            Container = null;
        }
    }

    public readonly UnitSelection Selection = new UnitSelection();

    void Start()
    {
        cam = GetComponent<Camera>();
        unitControls = GetComponent<UnitControls>();
        player = Player.Instance;
    }

    public void SetTarget(GameObject target)
    {
        Player.UnitContainer targetContainer = Player.Instance.FindContainer(target.name);
        if (targetContainer.amount <= 0) return;
        Selection.Set(target);
    }
    
    void Update()
    {
        GameObject preview = Selection.Preview;
        if (!preview) return;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit rayHit, float.MaxValue, targetLayer))
        {
            Vector3 point = rayHit.point;
            preview.transform.position = new Vector3(point.x, point.y + preview.transform.localScale.y/2, point.z);
        }

        if (Input.GetKeyDown(KeyCode.Return)) //enter is return
        {
            if (!player.RemoveUnit(Selection.Container))
            { 
                return;
            }

            var newUnit = preview.GetComponent<Unit>();
            newUnit.enabled = true;
            
            Selection.Clean();
            unitControls.AddUnit(newUnit);
        }
    }
}