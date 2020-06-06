using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IPointerEnterHandler
{
    #region Private Fields

    [SerializeField]
    private GameObject _backButton;

    [SerializeField]
    private GameObject _selected;

    public GameObject ForceSet;

    #endregion Private Fields

    #region Private Methods

    private IEnumerator DelaySelect()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_selected);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnEnable()
    {
        if (_selected == null)
        {
            return;
        }

        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(_selected);

        ////_selected.GetComponent<UnityEngine.UI.Selectable>().Select();

        StopAllCoroutines();
        StartCoroutine(DelaySelect());

        if (_backButton != null)
        {
            //EventSystem.current.currentInputModule).actionsAsset.FindAction("UI/GoBack").performed += ctx => GoBack();
        }
    }

    private void Update()
    {
        if (ForceSet != null)
        {
            _selected = ForceSet;
            EventSystem.current.SetSelectedGameObject(ForceSet);
            ForceSet = null;
        }

        if (_selected == null)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject != _selected)
        {
            _selected = EventSystem.current.currentSelectedGameObject;
        }
        else if (_selected != null && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_selected);
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void GoBack()
    {
        Debug.Log("Test");
        _backButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerEnter.gameObject;

        if (gameObject.GetComponentInChildren<UnityEngine.UI.Button>() != null || gameObject.GetComponentInChildren<UnityEngine.UI.Slider>() != null)
        {
            _selected = gameObject;
            EventSystem.current.SetSelectedGameObject(_selected);
        }
    }

    #endregion Public Methods
}