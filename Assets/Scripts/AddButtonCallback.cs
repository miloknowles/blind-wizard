using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonCallback : MonoBehaviour
{
    [SerializeField]
    private int type;

    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => addCallback());
    }

    private void addCallback()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<SelectUnit>().selectAttack(this.type);
    }
}
