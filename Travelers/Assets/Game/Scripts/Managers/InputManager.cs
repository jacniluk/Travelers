using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private LayerMask groundLayerMask;

	private void Update()
	{
		UpdateInput();
	}

	private void UpdateInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject() == false)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit))
				{
					if (Utilities.CompareLayers(raycastHit.transform.gameObject.layer, groundLayerMask))
					{
						if (NavigationManager.Instance.CanBeTarget(raycastHit.point))
						{
							TravelersManager.Instance.MoveToTarget(raycastHit.point);
							MapManager.Instance.ShowTargetMarker(raycastHit.point);
						}
					}
				}
			}
		}
	}
}
