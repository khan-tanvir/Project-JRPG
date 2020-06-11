using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region Private Fields

    private IInteractable _currentInteractable;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private float radius = 1.0f;

    #endregion Private Fields

    #region Public Properties

    public bool InteractableObject
    {
        get => _currentInteractable != null;
    }

    public Vector3 PlayerPosition
    {
        internal get;
        set;
    }

    #endregion Public Properties

    #region Public Methods

    public void CallInteract()
    {
        _currentInteractable.OnInteract();
    }

    public Collider2D FindClosestObject()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.PlayerPosition, radius, _layerMask);

        const float minSqrDistance = Mathf.Infinity;

        for (int i = 0; i < colliders.Length; i++)
        {
            float sqrDistanceToPlayer = (this.PlayerPosition - colliders[i].transform.position).sqrMagnitude;

            if (sqrDistanceToPlayer < minSqrDistance)
            {
                return colliders[i];
            }
        }

        return null;
    }

    public void Raycast()
    {
        if (FindClosestObject())
        {
            IInteractable interactable = FindClosestObject().GetComponent<IInteractable>();

            if (interactable?.EnabledInteraction == true)
            {
                if (interactable == _currentInteractable)
                {
                    return;
                }
                else if (_currentInteractable != null)
                {
                    _currentInteractable.UnFocus();
                    _currentInteractable = interactable;
                    _currentInteractable.Focus();
                    return;
                }
                else
                {
                    _currentInteractable = interactable;
                    _currentInteractable.Focus();
                }
            }
            else
            {
                if (_currentInteractable != null)
                {
                    _currentInteractable.UnFocus();
                    _currentInteractable = null;
                    return;
                }
            }
        }
        else
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.UnFocus();
                _currentInteractable = null;
                return;
            }
        }
    }

    #endregion Public Methods
}