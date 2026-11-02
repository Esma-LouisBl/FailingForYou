using UnityEngine;

public class VFXTrigger : MonoBehaviour
{
    [SerializeField] private GameObject vfxInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            Instantiate(vfxInput, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)), Quaternion.identity);
        }
    }
}
