using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestTimelineTrigger : MonoBehaviour
{
    public GameObject timeline;

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Ĳ�oCutScene");
        timeline.SetActive(true);
        timeline.GetComponent<PlayableDirector>().Play();
        this.gameObject.SetActive(false);
    }
}
