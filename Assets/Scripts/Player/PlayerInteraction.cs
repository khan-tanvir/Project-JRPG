using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    
    private IInteractable _currentInteractable;

    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private float radius = 1.0f;

    public bool InteractableObject
    {
        get
        {
            return _currentInteractable != null;
        }
    }

    public Vector3 PlayerPosition
    {
        internal get;
        set;
    }

    public void CallInteract()
    {
        _currentInteractable.OnInteract();
    }

    public Collider2D FindClosestObject()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.PlayerPosition, radius, _layerMask);

        float minSqrDistance = Mathf.Infinity;

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

            if (interactable != null && interactable.EnabledInteraction)
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
}
