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

    #endregion Private Fields

    #region Private Methods

    private void OnEnable()
    {
        Debug.Log("test");
        
        EventSystem.current.SetSelectedGameObject(_selected);

        _selected.GetComponent<UnityEngine.UI.Selectable>().Select();

        if (_backButton != null)
        {
            //EventSystem.current.currentInputModule).actionsAsset.FindAction("UI/GoBack").performed += ctx => GoBack();
            Debug.Log(FindObjectOfType<EventSystem>().GetComponentInChildren<InputSystemUIInputModule>().actionsAsset.FindAction("UI/GoBack"));
        }
    }

    private void Update()
    {
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