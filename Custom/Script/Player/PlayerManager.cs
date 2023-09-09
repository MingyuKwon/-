using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    PlayerAnimation playerAnimation;
    PlayerMove playerMove;

    AIPath aIPath;
    AIDestinationSetter aIDestinationSetter;

    public Transform playerTransform;
    private Transform target;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(this.gameObject);
        }

        playerAnimation = GetComponent<PlayerAnimation>();
        playerMove = GetComponent<PlayerMove>();
        playerTransform = GetComponent<Transform>();

        aIPath = GetComponent<AIPath>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();

        playerTransform.position = new Vector3(0.5f, 0.5f, 0);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0))
        {
            aIDestinationSetter.target = this.target;
        }
    }

    private void OnEnable() {
        EventManager.instance.stageSendTargetToPlayerEvent += GetTarget;
    }

    private void OnDisable() {
        EventManager.instance.stageSendTargetToPlayerEvent -= GetTarget;
    }

    private void GetTarget(Transform target)
    {
        this.target = target;
        
    }

}
